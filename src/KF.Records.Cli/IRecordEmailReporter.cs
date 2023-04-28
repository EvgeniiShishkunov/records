using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Cli;

internal interface IRecordEmailReporter
{
    bool TrySendRecords(List<Record> records);
}
