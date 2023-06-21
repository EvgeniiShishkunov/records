using KF.Records.Infrastructure;
using KF.Records.Infrastructure.Abstractions;
using KF.Records.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace KF.Records.Cli;

internal class Program
{
    static void Main(string[] args)
    {
        string email = "";
        string username = "";
        string password = "";
        string smptpServerAddress = "smtp.gmail.com";

        try
        {
            email = args[0];
            username = args[1];
            password = args[2];
        }
        catch
        {
            Console.WriteLine("Attention! Mail, login and password were not entered. To send notes by mail, enter the previously mentioned data \n");
        }

        try
        {
            smptpServerAddress = args[3];
        }
        catch
        {
            Console.WriteLine("You use " + smptpServerAddress + " by default \n");
        }

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IRecordEmailReporter, MailKitEmailReporter>(provider => new MailKitEmailReporter(smptpServerAddress, email, username, password));
        serviceCollection.AddSingleton<IRecordRepository, AppData>();
        serviceCollection.AddSingleton<CommandExecuter>();
        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(KF.Records.UseCases.Records.AddRecord.AddRecordCommand).Assembly));

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var commandExecuter = serviceProvider.GetService<CommandExecuter>();
        Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPressHandler);

        void CancelKeyPressHandler(object sender, ConsoleCancelEventArgs args)
        {
            try
            {
                commandExecuter.CancelKeyPress();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error. Message with notes was not sent");
                Console.WriteLine(ex.Message);
            }
        }

        while (true)
        {
            try
            {
                commandExecuter.HandleCommand(Console.ReadLine());
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong");
                Console.WriteLine(ex.Message);
            }
        }
    }
}