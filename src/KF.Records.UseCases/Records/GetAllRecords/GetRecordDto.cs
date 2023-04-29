using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.GetAllRecords;

public class GetRecordDto
{
    public required int Id { get; init; }
    public required string Description { get; init; }
    public required IReadOnlyCollection<string> Tags { get; init; }
}
