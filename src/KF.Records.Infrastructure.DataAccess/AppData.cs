using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Infrastructure.DataAccess;

public class AppData: IRecordRepository
{
    public IReadOnlyCollection<Record> Records => _records;
    private readonly List<Record> _records = new();

    private int _assignId;

    public void AddRecord(Record record)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }
        record.Id = _assignId++;
        _records.Add(record);
    }

    public void RemoveRecordByID(int id)
    {
        var removingRecord = _records.Find(r => r.Id == id);
        _records.Remove(removingRecord);
    }
}
