using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using MediatR;
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
public class GetAllRecordsQueryHandler : IRequestHandler<GetAllRecordsQuery, List<GetRecordDto>>
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
    public Task<List<GetRecordDto>> Handle(GetAllRecordsQuery request, CancellationToken cancellationToken)
    {
        var records = _recordRepository.Records.Select(record => new GetRecordDto()
        {
            Id = record.Id,
            Description = record.Description,
            Tags = record.Tags
        });
        return Task.FromResult(records.ToList());
    }
}
