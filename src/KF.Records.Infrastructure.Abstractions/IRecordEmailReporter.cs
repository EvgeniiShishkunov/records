using KF.Records.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Infrastructure.Abstractions;

public interface IRecordEmailReporter
{
    bool TrySendRecords(List<Record> records);
}
