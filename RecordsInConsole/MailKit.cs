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

namespace RecordsInConsole
{
    internal class MailKit : IEmailService
    {
        private readonly string SmtpAddress;
        private readonly string Email;
        private readonly string UserName;
        private readonly string Password;

        public MailKit(string smptAddress, string email, string username, string password) 
        {
            SmtpAddress = smptAddress;
            Email = email;
            UserName = username;
            Password = password;
        }

        public bool TrySendRecords(List<Record> records)
        {
            if (records.Any() == false) 
            {
                Console.WriteLine("No records to sent");
                return false;
            }
            if (UserName == String.Empty || Password == String.Empty || Email == String.Empty) 
            { 
                Console.WriteLine("No username, password or email entered");
                return false;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Records", Email));
            message.To.Add(new MailboxAddress("", Email));
            message.Subject = "Records " + DateTime.Today.ToShortDateString();

            string messageTextBody = "";
            foreach (var record in records)
            {
                messageTextBody += record.Description + "\n";
            }

            message.Body = new TextPart("plain")
            {
                Text = messageTextBody
            };

            try
            {
                using (var client = new SmtpClient(new ProtocolLogger(Console.OpenStandardOutput())))
                {
                    client.Connect(SmtpAddress, 587, SecureSocketOptions.StartTls);
                    client.Authenticate(UserName, Password);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex) 
            {
                return false;
            }

            return true;
        }

    }
}
