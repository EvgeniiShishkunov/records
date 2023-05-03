using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.GetAllRecords;

/// <summary>
/// Record dto
/// </summary>
public class GetRecordDto
{
    /// <summary>
    /// Record id
    /// </summary>
    public required int Id { get; init; }
    /// <summary>
    /// Record description
    /// </summary>
    public required string Description { get; init; }
    /// <summary>
    /// Record tags
    /// </summary>
    public required IReadOnlyCollection<string> Tags { get; init; }
}
