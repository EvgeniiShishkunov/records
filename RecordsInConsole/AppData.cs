using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsInConsole;

internal class AppData
{
	public readonly List<Record> Records = new();

	private int _assignId;

	public void AddRecord(Record record)
	{
		if (record == null)
			return;

		record.Id = _assignId++;
        Records.Add(record);
	}

	public bool DeleteRecord(int id)
	{
		var removingRecord = Records.Find(r => r.Id == id);

		if (removingRecord != null)
		{
            Records.Remove(removingRecord);
			return true;
		}
		else
			return false;
	}
}
