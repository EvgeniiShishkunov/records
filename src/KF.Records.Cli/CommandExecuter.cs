using AutoMapper;
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
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace KF.Records.Cli;

internal class CommandExecuter
{
    private readonly IRecordEmailReporter emailService;
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    private string command;
    private List<string> commandWords = new();

    private readonly Dictionary<string, Func<CancellationToken, Task>> _actionDelegates;

    public CommandExecuter(IRecordEmailReporter emailService, IMediator mediator, IMapper mapper)
    {
        this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _actionDelegates = new Dictionary<string, Func<CancellationToken, Task>>()
        {
            {"add", AddCommandAsync },
            {"list", ListCommandAsync },
            {"delete", DeleteCommandAsync }
        };
    }

    public async Task CancelKeyPress(CancellationToken cancellationToken)
    {
        Console.WriteLine("Sending notes by email");

        var getAllRecordsQuery = new GetAllRecordsQuery();
        var recordsDto = await mediator.Send(getAllRecordsQuery, cancellationToken);
        var records = mapper.Map<List<Record>>(recordsDto);

        bool emailSendResult = await emailService.TrySendRecordsAsync(records, cancellationToken);
        if (emailSendResult == true)
        {
            Console.WriteLine("Message with notes was sent");
        }
        else
        {
            Console.WriteLine("Error. Message with notes was not sent");
        }
    }

    public async Task HandleCommandAsync(string command, CancellationToken cancellationToken)
    {
        if (command == null || command.Trim() == string.Empty)
        {
            return;
        }

        this.command = command;
        commandWords = command.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        string firstWord = commandWords[0].ToLowerInvariant();

        if (_actionDelegates.TryGetValue(firstWord, out var action) == true)
        {
            await action.Invoke(cancellationToken);
        }
        else
        {
            Console.WriteLine("Unknown command: " + firstWord);
        }
    }

    private async Task AddCommandAsync(CancellationToken cancellationToken)
    {
        if (commandWords.Count < 2)
        {
            Console.WriteLine("No note content. Use add \"(Your description)\"");
            return;
        }

        int startTextDescriptionIndex = command.IndexOf('"');
        int endTextDescriptionIndex = command.LastIndexOf('"');
        string recordDescription;

        if (startTextDescriptionIndex != -1 && endTextDescriptionIndex != -1 && startTextDescriptionIndex != endTextDescriptionIndex)
        {
            recordDescription = command[(startTextDescriptionIndex + 1)..endTextDescriptionIndex];
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


        List<string> tagNames = command[(endTextDescriptionIndex + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

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
        await mediator.Send(addRecordCommand, cancellationToken);

        Console.WriteLine("Record added");
    }

    private async Task ListCommandAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("All records");
        var getAllRecordsQuery = new GetAllRecordsQuery();
        var records = await mediator.Send(getAllRecordsQuery, cancellationToken);

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

    private async Task DeleteCommandAsync(CancellationToken cancellationToken)
    {
        if (commandWords.Count < 2)
        {
            Console.WriteLine("ID not specified");
            return;
        }

        List<string> parametrWords = commandWords.GetRange(2, commandWords.Count - 2);

        if (parametrWords.Any() == true)
        {
            Console.Write("Unknown command words: ");
            foreach (string parametrWord in parametrWords)
            {
                Console.Write(parametrWord + " ");
            }
            return;
        }

        if (int.TryParse(commandWords[1], out var id) == false)
        {
            Console.WriteLine("The ID is in the wrong format. Use natural number");
            return;
        }

        try
        {
            var removeRecordCommand = new RemoveRecordCommand() { Id = id };
            await mediator.Send(removeRecordCommand, cancellationToken);
            Console.WriteLine("Record removed");
        }
        catch (Exception)
        {
            Console.WriteLine("Record with given ID not found");
        }
    }
}
