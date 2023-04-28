using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsInConsole;

internal interface IRecordEmailReporter
{
    bool TrySendRecords(List<Record> records);
}
