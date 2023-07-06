using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using KF.Records.UseCases.Records.AddRecord;
using KF.Records.UseCases.Records.GetAllRecords;
using KF.Records.UseCases.Records.RemoveRecord;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace KF.Records.Cli;

internal class CommandExecuter
{
    private readonly IRecordEmailReporter _emailService;
    private readonly IMediator _mediator;

    private string _command;
    private List<string> _commandWords = new();

    private readonly Dictionary<string, Action> _actionDelegates;

    public CommandExecuter(IRecordEmailReporter emailService, IMediator mediator)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _actionDelegates = new Dictionary<string, Action>()
        {
            {"add", AddCommand },
            {"list", ListCommand },
            {"delete", DeleteCommand }
        };
    }

    public async void CancelKeyPress()
    {
        Console.WriteLine("Sending notes by email");

        var getAllRecordsQuery = new GetAllRecordsQuery();
        var records = await _mediator.Send(getAllRecordsQuery);

        bool emailSendResult = _emailService.TrySendRecords((List<Record>)records);
        if (emailSendResult == true)
        {
            Console.WriteLine("Message with notes was sent");
        }
        else
        {
            Console.WriteLine("Error. Message with notes was not sent");
        }
    }

    public void HandleCommand(string command)
    {
        if (command == null || command.Trim() == string.Empty)
        {
            return;
        }

        _command = command;
        _commandWords = command.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        string firstWord = _commandWords[0].ToLowerInvariant();

        if (_actionDelegates.TryGetValue(firstWord, out var action) == true)
        {
            action.Invoke();
        }
        else
        {
            Console.WriteLine("Unknown command: " + firstWord);
        }
    }

    private async void AddCommand()
    {
        if (_commandWords.Count < 2)
        {
            Console.WriteLine("No note content. Use add \"(Your description)\"");
            return;
        }

        int startTextDescriptionIndex = _command.IndexOf('"');
        int endTextDescriptionIndex = _command.LastIndexOf('"');
        string recordDescription;

        if (startTextDescriptionIndex != -1 && endTextDescriptionIndex != -1 && startTextDescriptionIndex != endTextDescriptionIndex)
        {
            recordDescription = _command[(startTextDescriptionIndex + 1)..endTextDescriptionIndex];
        }
        else
        {
            Console.WriteLine("The description of the note is not in quotes. Use add \"(Your description)\"");
            return;
        }

        if (recordDescription == string.Empty)
        {
            Console.WriteLine("Write something in the post description");
            return;
        }


        List<string> tagNames = _command[(endTextDescriptionIndex + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        foreach (var tag in tagNames)
        {
            var isStringValid = tag.All(symbol => char.IsLetterOrDigit(symbol) == true);
            if (isStringValid == false)
            {
                Console.WriteLine("Incorrect tag name, use symbols and or numbers");
                return;
            }
        }

        var tags = new HashSet<AddTagDto>();
        if (tagNames.Any() == true)
        {
            foreach (var tag in tagNames)
            {
                tags.Add(new AddTagDto() { Name = tag });
            }
        }
        var record = new AddRecordCommand() { Description = recordDescription, Tags = tags };

        var addRecordCommand = new AddRecordCommand() { Description = record.Description, Tags = record.Tags.ToList() };
        await _mediator.Send(addRecordCommand);

        Console.WriteLine("Record added");
    }

    private async void ListCommand()
    {
        Console.WriteLine("All records");
        var getAllRecordsQuery = new GetAllRecordsQuery();
        var records = await _mediator.Send(getAllRecordsQuery);

        foreach (var record in records)
        {
            Console.WriteLine(record.Description + "\tid: " + record.Id);
            Console.Write("Tags: ");
            if (record.Tags != null)
            {
                foreach (var tag in record.Tags)
                {
                    Console.Write(tag.Name + " ");
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    private async void DeleteCommand()
    {
        if (_commandWords.Count < 2)
        {
            Console.WriteLine("ID not specified");
            return;
        }

        List<string> parametrWords = _commandWords.GetRange(2, _commandWords.Count - 2);

        if (parametrWords.Any() == true)
        {
            Console.Write("Unknown command words: ");
            foreach (string parametrWord in parametrWords)
            {
                Console.Write(parametrWord + " ");
            }
            return;
        }

        if (int.TryParse(_commandWords[1], out var id) == false)
        {
            Console.WriteLine("The ID is in the wrong format. Use natural number");
            return;
        }

        try
        {
            var removeRecordCommand = new RemoveRecordCommand() { Id = id };
            await _mediator.Send(removeRecordCommand);
            Console.WriteLine("Record removed");
        }
        catch (Exception)
        {
            Console.WriteLine("Record with given ID not found");
        }
    }
}
