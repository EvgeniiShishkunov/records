using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsInConsole
{
    internal class MailKit : IEmailService
    {
        public bool TrySendRecords(List<Record> records)
        {
            return true;
        }
    }
}
