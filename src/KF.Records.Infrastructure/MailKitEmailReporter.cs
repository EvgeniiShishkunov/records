using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Infrastructure;

/// <summary>
/// Represent the ability to send notes by email
/// </summary>
public class MailKitEmailReporter : IRecordEmailReporter
{
    private readonly string SmtpAddress;
    private readonly string Email;
    private readonly string UserName;
    private readonly string Password;

    /// <summary>
    /// Indicate smpt server address, email by send records, username and password for smpt service
    /// </summary>
    public MailKitEmailReporter(string smptAddress, string email, string username, string password)
    {
        SmtpAddress = smptAddress;
        Email = email;
        UserName = username;
        Password = password;
    }

    /// <summary>
    /// Return true if records have been sent
    /// </summary>
    public bool TrySendRecords(List<Record> records)
    {
        if (records == null)
        {
            throw new ArgumentNullException(nameof(records));
        }

        if (records.Any() == false)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Email))
        {
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
        catch (Exception)
        {
            throw;
        }

        return true;
    }

}
