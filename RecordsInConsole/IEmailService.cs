using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsInConsole
{
    internal interface IEmailService
    {
        public string CreateEmailFromRecords(List<Record> records); 
        public bool TrySendEmail(string email);
    }
}
