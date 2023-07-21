using KF.Records.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Infrastructure.Abstractions;

/// <summary>
/// Represent the ability to sent records by email
/// </summary>
public interface IRecordEmailReporter
{
    /// <summary>
    /// Return true if records have been sent
    /// </summary>
    Task<bool> TrySendRecordsAsync(List<Record> records, CancellationToken cancellationToken);
}
