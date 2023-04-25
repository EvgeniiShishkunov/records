using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RecordsInConsole;

internal class CommandHandler
{
    private readonly AppData _appData;
    private readonly IEmailService _emailService;

    private string _command;
    private List<string> _commandWords = new();

    private Dictionary<string, Action> _actionDelegates;

    public CommandHandler(AppData appData, IEmailService emailService)
    {
        if (appData == null)
        {
            throw new ArgumentNullException(nameof(appData));
        }
        if (emailService == null)
        {
            throw new ArgumentNullException(nameof(emailService));
        }

        _appData = appData;
        _emailService = emailService;
        _actionDelegates = new Dictionary<string, Action>()
        {
            {"add", AddCommand },
            {"list", ListCommand },
            {"delete", DeleteCommand }
        };
    }

    public void CancelKeyPress(object sender, ConsoleCancelEventArgs args)
    {
        Console.WriteLine("Sending notes by email");

        bool emailSendResult = _emailService.TrySendRecords(_appData.Records.ToList());
        if (emailSendResult == true)
        {
            Console.WriteLine("Message with notes was sent");
        }
        else
        {
            Console.WriteLine("Error. Message with notes was not sent");
        }
        Environment.Exit(0);
    }

    public void HandleCommand(string command)
    {
        if (command == null || command.Replace(" ", String.Empty) == String.Empty)
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

        int startDescrIndex = _command.IndexOf('"');
        int endDescrIndex = _command.LastIndexOf('"');
        string recordDescription;

        if (startDescrIndex != -1 && endDescrIndex != -1 && startDescrIndex != endDescrIndex)
        {
            recordDescription = _command[(startDescrIndex + 1)..endDescrIndex];
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
        List<string> tags = _command[(endDescrIndex + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        if (tags.Any() == true)
        {
            record.Tags = new List<string>();
            record.Tags.AddRange(tags);
        }

        record.Description = recordDescription;
        _appData.AddRecord(record);
        Console.WriteLine("Record added");
    }

    private void ListCommand()
    {
        Console.WriteLine("All records");

        foreach (Record record in _appData.Records)
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

        if (_appData.DeleteRecord(id) == true)
        {
            Console.WriteLine("Record removed");
        }
        else
        {
            Console.WriteLine("Record with given ID not found");
        }
    }

}
