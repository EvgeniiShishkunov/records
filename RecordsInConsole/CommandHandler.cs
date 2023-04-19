using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsInConsole
{
	internal class CommandHandler
	{
		AppData _appData;

		List<string> _commandWords;

		delegate void ActionDelegate();
		Dictionary<string, ActionDelegate> _actionDelegates;

		public CommandHandler(AppData appData)
		{
			_appData = appData;
			_commandWords = new List<string>();
			_actionDelegates = new Dictionary<string, ActionDelegate>()
			{
				{"add", AddCommand },
				{"list", ListCommand },
				{"delete", DeleteCommand }
			};
		}

		public void HandleCommand(string command)
		{
			_commandWords = command.Split(' ').ToList();

			if (_commandWords[0] == "")
				return;

			string firstWord = _commandWords[0].ToLower();

			if (_actionDelegates.ContainsKey(firstWord) == true)
			{
				_actionDelegates[_commandWords.First().ToLower()].Invoke();
			}
			else
			{
				Console.WriteLine("Неизвестная команда: " + firstWord);
			}
		}

		void AddCommand()
		{
			if (_commandWords.Count < 2)
			{
				Console.WriteLine("ГДЕ БЛЯТЬ ОПИСАНИЕ КОМАНДЫ, ММММ????");
				return;
			}

			List<string> parametrWords = _commandWords.GetRange(2, _commandWords.Count-2);
			if (parametrWords.Count>0)
			{
				Console.Write("НЕИЗВЕСТНЫЕ СЛОВА В ТВОЕЙ КОМАНДЕ, ЕБАЧОСИНА: ");
				foreach (string parametrWord in parametrWords)
				{
					Console.Write(parametrWord + " ");
					Console.WriteLine();
				}
				return;
			}

			string recordDescription = _commandWords[1];
			if (recordDescription.First() != '"' && recordDescription.Last() != '"')
			{
				Console.WriteLine("ПОСТАВЬ КАВЫЧКИ ПИДОР!!");
				return;
			}

			recordDescription = recordDescription.Substring(1, recordDescription.Length - 2);
			if (recordDescription == "")
			{
				Console.WriteLine("ГДЕ БЛЯТЬ ОПИСАНИЕ ЗАПИСИ, ММММ????");
				return;
			}

			Record record = new Record();
			record.description = recordDescription;
			_appData.AddRecord(record);
			Console.WriteLine("Запись добавлена");
		}
		
		void ListCommand()
		{
			Console.WriteLine("Все записи");
			foreach (Record record in _appData.records)
			{
				Console.WriteLine(record.description + "\tid: " + record.id);
			}
		}
		void DeleteCommand()
		{
			if (_commandWords.Count < 2)
			{
				Console.WriteLine("ГДЕ ID ТВАРЬ????");
				return;
			}

			List<string> parametrWords = _commandWords.GetRange(2, _commandWords.Count - 2);
			if (parametrWords.Count > 0)
			{
				Console.Write("НЕИЗВЕСТНЫЕ СЛОВА В ТВОЕЙ КОМАНДЕ, ЕБАЧОСИНА: ");
				foreach (string parametrWord in parametrWords)
				{
					Console.Write(parametrWord + " ");
					Console.WriteLine();
				}
				return;
			}

			int id;
			if( Int32.TryParse(_commandWords[1], out id) == false)
			{
				Console.WriteLine("БЛЯТЬ, ЦИФИРКУ НАПИШИ");
				return;
			}

			if (_appData.DeleteRecord(id) == true)
			{
				Console.WriteLine("Запись удалена");
			}
			else
			{
				Console.WriteLine("НЕТУ ТАКОГО ID, УРОД!!!!");
			}
		}

	}
}
