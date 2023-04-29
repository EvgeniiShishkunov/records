using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.GetAllRecords;

public class GetAllRecordsQueryHandler
{
    private readonly IRecordRepository _recordRepository;

    public GetAllRecordsQueryHandler (IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    public IReadOnlyCollection<GetRecordDto> Handle (GetAllRecordsQuery request)
    {
        var records = from r in _recordRepository.Records
            select new GetRecordDto()
            {
                Id = r.Id,
                Description = r.Description,
                Tags = r.Tags
            }; ;
        return records.ToList();
    }
}
