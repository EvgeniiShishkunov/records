using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.AddRecord;

public class AddRecordCommand
{
    public required string Description { get; init; }
    public required IReadOnlyCollection<string> Tags { get; init; }
}
