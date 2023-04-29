using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using KF.Records.Infrastructure.DataAccess;
using KF.Records.UseCases.Records.AddRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace KF.Records.Cli;

internal class CommandHandler
{
    private readonly IRecordRepository _recordsRepository;
    private readonly IRecordEmailReporter _emailService;

    private string _command;
    private List<string> _commandWords = new();

    private Dictionary<string, Action> _actionDelegates;

    public CommandHandler(IRecordRepository recordsRepository, IRecordEmailReporter emailService)
    {
        if (recordsRepository == null)
        {
            throw new ArgumentNullException(nameof(recordsRepository));
        }
        if (emailService == null)
        {
            throw new ArgumentNullException(nameof(emailService));
        }

        _recordsRepository = recordsRepository;
        _emailService = emailService;
        _actionDelegates = new Dictionary<string, Action>()
        {
            {"add", AddCommand },
            {"list", ListCommand },
            {"delete", DeleteCommand }
        };
    }

    public void CancelKeyPress()
    {
        Console.WriteLine("Sending notes by email");

        bool emailSendResult = _emailService.TrySendRecords(_recordsRepository.Records.ToList());
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
        if (command == null || command.Trim() == String.Empty)
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

    private void AddCommand()
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

        if (recordDescription == String.Empty)
        {
            Console.WriteLine("Write something in the post description");
            return;
        }

        Record record = new Record();
        List<string> tags = _command[(endTextDescriptionIndex + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        foreach (var tag in tags)
        {
            var isStringValid = tag.All(symbol => char.IsLetterOrDigit(symbol) == true);
            if (isStringValid == false)
            {
                Console.WriteLine("Incorrect tag name, use symbols and or numbers");
                return;
            }
        }


        if (tags.Any() == true)
        {
            record.Tags = tags.ToHashSet();
        } 

        record.Description = recordDescription;

        //_recordsRepository.AddRecord(record);
        //Console.WriteLine("Record added");

        AddRecordCommand addRecordCommand = new() { Description = recordDescription, Tags = tags.ToHashSet() };
        AddRecordCommandHandler addRecordCommandHandler = new(_recordsRepository);
        addRecordCommandHandler.Handle(addRecordCommand);
    }

    private void ListCommand()
    {
        Console.WriteLine("All records");

        foreach (Record record in _recordsRepository.Records)
        {
            Console.WriteLine(record.Description + "\tid: " + record.Id);
            Console.Write("Tags: ");
            if (record.Tags != null)
            {
                foreach (string tag in record.Tags)
                {
                    Console.Write(tag + " ");
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    private void DeleteCommand()
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

        if (Int32.TryParse(_commandWords[1], out var id) == false)
        {
            Console.WriteLine("The ID is in the wrong format. Use natural number");
            return;
        }

        if (_recordsRepository.Records.FirstOrDefault(record => record.Id == id) != null)
        {
            _recordsRepository.RemoveRecordByID(id);
            Console.WriteLine("Record removed");
        }
        else
        {
            Console.WriteLine("Record with given ID not found");
        }
    }
}
