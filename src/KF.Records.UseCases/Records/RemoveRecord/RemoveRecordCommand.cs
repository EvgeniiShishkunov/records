using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.RemoveRecord;

/// <summary>
/// Remove record from data base
/// </summary>
public class RemoveRecordCommand : IRequest
{
    /// <summary>
    /// Id
    /// </summary>
    public required int Id { get; init; }
}
