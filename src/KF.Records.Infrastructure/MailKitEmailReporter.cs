using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using MailKit.Security;
using System.Security.Authentication;
using KF.Records.Infrastructure.Abstractions;
using KF.Records.Domain;

namespace KF.Records.Infrastructure;

public class MailKitEmailReporter : IRecordEmailReporter
{
    private readonly string SmtpAddress;
    private readonly string Email;
    private readonly string UserName;
    private readonly string Password;

    public MailKitEmailReporter(string smptAddress, string email, string username, string password) 
    {
        SmtpAddress = smptAddress;
        Email = email;
        UserName = username;
        Password = password;
    }

    public bool TrySendRecords(List<Record> records)
    {
        if (records == null)
        {
            throw new ArgumentNullException(nameof(records));
        }

        if (records.Any() == false) 
        {
            Console.WriteLine("No records to sent");
            return false;
        }

        if ( String.IsNullOrWhiteSpace(UserName) || String.IsNullOrWhiteSpace(Password) || String.IsNullOrWhiteSpace(Email))
        { 
            Console.WriteLine("Username, parssword or email not provided");
            return false;
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Records", Email));
        message.To.Add(new MailboxAddress("", Email));
        message.Subject = "Records " + DateTime.Today.ToShortDateString();

        StringBuilder messageTextBody = new();

        foreach (var record in records)
        {
            messageTextBody.AppendLine(record.Description + "\n");
        }

        message.Body = new TextPart()
        {
            Text = messageTextBody.ToString(),
        };

        try
        {
            using var client = new SmtpClient();
            client.Connect(SmtpAddress, 587, SecureSocketOptions.StartTls);
            client.Authenticate(UserName, Password);
            client.Send(message);
            client.Disconnect(true);
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
            throw;
        }

        return true;
    }

}
