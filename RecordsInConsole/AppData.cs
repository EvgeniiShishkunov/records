using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsInConsole
{
	internal class AppData
	{
		public List<Record> records { get; private set; }

		private int _assignId;

		public AppData()
		{
			records = new List<Record>();
			_assignId = 0;
		}

		public void AddRecord(Record record)
		{
			if (records == null)
				return;

			record.id = _assignId++;
			records.Add(record);
		}

		public bool DeleteRecord(int id)
		{
			Record removingRecord = records.Find(r => r.id == id);
			if (removingRecord != null)
			{
				records.Remove(removingRecord);
				return true;
			}
			else { return false; }
		}
	}
}
