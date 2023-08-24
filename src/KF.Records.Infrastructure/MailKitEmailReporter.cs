using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions.Export;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
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

    private readonly ILogger<MailKitEmailReporter> logger;

    /// <summary>
    /// Indicate smpt server address, email by send records, username and password for smpt service
    /// </summary>
    public MailKitEmailReporter(string smptAddress, string email, string username, string password, ILogger<MailKitEmailReporter> logger)
    {
        SmtpAddress = smptAddress;
        Email = email;
        UserName = username;
        Password = password;
        this.logger = logger;
    }

    /// <summary>
    /// Return true if records have been sent
    /// </summary>
    public async Task<bool> TrySendRecordsAsync(List<RecordEmailModel> records, CancellationToken cancellationToken)
    {
        if (records == null)
        {
            logger.LogError(new ArgumentNullException(nameof(records)), "Failed to sent email");
            return false;
        }

        if (records.Any() == false)
        {
            logger.LogWarning("Failed to sent email. Nothing to send");
            return false;
        }

        if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Email))
        {
            logger.LogError("Failed to sent email. No username or password or email");
            return false;
        }

        logger.LogInformation("Start trying to sent records on {Email}", Email);

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
            await client.ConnectAsync(SmtpAddress, 587, SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(UserName, Password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError( ex, "Failed to sent email");
            return false;
        }

        logger.LogInformation("Records was sent to {Email}", Email);
        return true;
    }

}
