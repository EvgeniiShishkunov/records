using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.RemoveRecord;

internal class RemoveRecordCommand
{
    public required int Id { get; init; }
}
