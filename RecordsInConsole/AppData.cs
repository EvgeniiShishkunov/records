using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsInConsole;

internal class AppData
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

    public bool DeleteRecord(int id)
    {
        var removingRecord = _records.Find(r => r.Id == id);
        return _records.Remove(removingRecord);
    }
}
