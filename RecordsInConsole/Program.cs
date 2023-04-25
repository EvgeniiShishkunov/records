namespace RecordsInConsole;

internal class Program
{
    static void Main(string[] args)
    {
        string email = "";
        string username = "";
        string password = "";

        if (args.Length == 1)
        {
            email = args[0];
        }
        if (args.Length == 2)
        {
            email = args[0];
            username = args[1];
        }
        if (args.Length == 3)
        {
            email = args[0];
            username = args[1];
            password = args[2];
        }

        CommandHandler commandHandler = new CommandHandler(new AppData(), new MailKit("smtp.gmail.com", email, username, password));
        Console.CancelKeyPress += new ConsoleCancelEventHandler(commandHandler.CancelKeyPress);

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