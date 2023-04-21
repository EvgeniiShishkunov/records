using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsInConsole
{
    internal interface IEmailService
    {
        public bool TrySendRecords(List<Record> records);
    }
}
