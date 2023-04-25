namespace RecordsInConsole;

internal class Program
{
    static void Main(string[] args)
    {
        string email = "";
        string username = "";
        string password = "";

        try
        { 
            email = args[0];
            username = args[1];
            password = args[2];
        }
        catch
        {
            Console.WriteLine("Attention, mail, login and password were not entered. To send notes by mail, enter the previously mentioned data");
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