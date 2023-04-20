namespace RecordsInConsole;

internal class Program
{
	static void Main(string[] args)
	{
		CommandHandler commandHandler = new CommandHandler(new AppData());

		while (true) 
		{
			try
			{
                commandHandler.HandleCommand(Console.ReadLine());
                Console.WriteLine();
            }
			catch (Exception ex) 
			{
                Console.WriteLine("Что-то пошло не так");
                Console.WriteLine(ex.ToString());
			}

		}
	}
}