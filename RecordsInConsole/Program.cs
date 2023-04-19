namespace RecordsInConsole
{
	internal class Program
	{
		static void Main(string[] args)
		{
			CommandHandler commandHandler = new CommandHandler(new AppData());

			while (true) 
			{
				commandHandler.HandleCommand(Console.ReadLine());
			}
		}
	}
}