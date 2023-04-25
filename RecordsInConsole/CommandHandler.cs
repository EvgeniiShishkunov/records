using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsInConsole;

internal class CommandHandler
{
    private readonly AppData _appData;

    private string _command;
    private List<string> _commandWords = new();

    private Dictionary<string, Action> _actionDelegates;

    public CommandHandler(AppData appData)
    {
        _appData = appData;
        _actionDelegates = new Dictionary<string, Action>()
        {
            {"add", AddCommand },
            {"list", ListCommand },
            {"delete", DeleteCommand }
        };
    }

    public void HandleCommand(string command)
    {
        _command = command;
        _commandWords = command.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        if (_commandWords.Any() == false)
            return;

        string firstWord = _commandWords[0].ToLowerInvariant();

        if (_actionDelegates.TryGetValue(firstWord, out var action) == true)
        {
            action.Invoke();
        }
        else
        {
            Console.WriteLine("Неизвестная команда: " + firstWord);
        }
    }

    private void AddCommand()
    {
        if (_commandWords.Count < 2)
        {
            Console.WriteLine("Нет содержания заметки. Используйте add \"(Ваше описание)\"");
            return;
        }

        int startDescrIndex = _command.IndexOf('"');
        int endDescrIndex = _command.LastIndexOf('"');
        string recordDescription;

        if (startDescrIndex != -1 && endDescrIndex != -1)
            recordDescription = _command[(startDescrIndex + 1)..endDescrIndex];
        else
        {
            Console.WriteLine("Описание заметки не под кавычками. Используйте add \"(Ваше описание)\"");
            return;
        }

        if (recordDescription == String.Empty)
        {
            Console.WriteLine("Напишите что-то в описании записи");
            return;
        }

        Record record = new Record();
        HashSet<string> tags = _command[(endDescrIndex + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet();

        if (tags.Any() == true)
        {
            record.Tags = tags;
        }

        record.Description = recordDescription;
        _appData.AddRecord(record);
        Console.WriteLine("Запись добавлена");
    }

    private void ListCommand()
    {
        Console.WriteLine("Все записи");

        foreach (Record record in _appData.Records)
        {
            Console.WriteLine(record.Description + "\tid: " + record.Id);
            Console.Write("Тэги: ");
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
            Console.WriteLine("Не указан ID");
            return;
        }

        List<string> parametrWords = _commandWords.GetRange(2, _commandWords.Count - 2);

        if (parametrWords.Any() == true)
        {
            Console.Write("Неизвестные слова в команде: ");
            foreach (string parametrWord in parametrWords)
            {
                Console.Write(parametrWord + " ");
            }
            return;
        }

        if (Int32.TryParse(_commandWords[1], out var id) == false)
        {
            Console.WriteLine("ID указан в неверном формате. Используйте натуральное число");
            return;
        }

        if (_appData.DeleteRecord(id) == true)
        {
            Console.WriteLine("Запись удалена");
        }
        else
        {
            Console.WriteLine("Запись с данным ID не найдена");
        }
    }

}
