using KF.Records.Infrastructure;
using KF.Records.Infrastructure.Abstractions;
using KF.Records.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System.Threading;

namespace KF.Records.Cli;

internal class Program
{
    static async Task Main(string[] args)
    {
        using CancellationTokenSource cancellationTokenSource = new();

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

        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        IConfiguration config = builder.Build();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(loggingBuilder =>
        {
            // configure Logging with NLog
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            loggingBuilder.AddNLog(config);
        });
        serviceCollection.AddTransient<IRecordEmailReporter, MailKitEmailReporter>(provider => new MailKitEmailReporter(smptpServerAddress, email, username, password, provider.GetRequiredService<ILogger<MailKitEmailReporter>>()));
        serviceCollection.AddSingleton<CommandExecuter>();
        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(KF.Records.UseCases.Records.AddRecord.AddRecordCommand).Assembly));
        serviceCollection.AddScoped<IReadWriteDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        serviceCollection.AddDbContext<AppDbContext>(options => options.UseNpgsql(config.GetSection("ConnectionStrings").GetSection("Default").Value));

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var commandExecuter = serviceProvider.GetService<CommandExecuter>();
        Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPressHandlerAsync);

        async void CancelKeyPressHandlerAsync(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            try
            {
                cancellationTokenSource.Cancel();
                var cancelKeyPressCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                await commandExecuter.CancelKeyPress(cancelKeyPressCancellationTokenSource.Token);
                args.Cancel = false;
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
                await commandExecuter.HandleCommandAsync(Console.ReadLine(), cancellationTokenSource.Token);
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