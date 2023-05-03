using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.GetAllRecords;

/// <summary>
/// Get all records handler
/// </summary>
public class GetAllRecordsQueryHandler
{
    private readonly IRecordRepository _recordRepository;

    /// <summary>
    /// Indicate database context
    /// </summary>
    public GetAllRecordsQueryHandler(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    /// <summary>
    /// Return all records from database
    /// </summary>
    public IReadOnlyCollection<GetRecordDto> Handle(GetAllRecordsQuery request)
    {
        var records = _recordRepository.Records.Select(record => new GetRecordDto()
        {
            Id = record.Id,
            Description = record.Description,
            Tags = record.Tags
        });
        return records.ToList();
    }
}
