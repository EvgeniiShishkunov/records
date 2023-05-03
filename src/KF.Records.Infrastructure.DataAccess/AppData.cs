using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Infrastructure.DataAccess;

/// <summary>
/// Storage application memory
/// </summary>
public class AppData : IRecordRepository
{
    private readonly List<Record> _records = new();
    private int _assignId;

    /// <summary>
    /// Represent readonly records
    /// </summary>
    public IReadOnlyCollection<Record> Records => _records;

    /// <summary>
    /// Add record into database
    /// </summary>
    public void AddRecord(Record record)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }
        record.Id = _assignId++;
        _records.Add(record);
    }

    /// <summary>
    /// Remove  record from database by id
    /// </summary>
    public void RemoveRecordById(int id)
    {
        var removingRecord = _records.Find(r => r.Id == id);
        if (_records.Remove(removingRecord) == false)
        {
            throw new Exception();
        }
    }
}
