using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsInConsole
{
    internal class MailKit : IEmailService
    {
        public string CreateEmailFromRecords(List<Record> records)
        {
            throw new NotImplementedException();
        }

        public bool TrySendEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
