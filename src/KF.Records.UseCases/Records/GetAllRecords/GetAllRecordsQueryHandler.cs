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
public class GetAllRecordsQueryHandler : IRequestHandler<GetAllRecordsQuery, IList<GetRecordDto>>
{
    private readonly IReadWriteDbContext _readWriteDbContext;

    /// <summary>   
    /// Indicate database context
    /// </summary>
    public GetAllRecordsQueryHandler(IReadWriteDbContext readWriteDbContext)
    {
        _readWriteDbContext = readWriteDbContext;
    }

    /// <summary>
    /// Return all records from database
    /// </summary>
    public Task<IList<GetRecordDto>> Handle(GetAllRecordsQuery request, CancellationToken cancellationToken)
    {
        var records = _readWriteDbContext.Records.Select(record => new GetRecordDto()
        {
            Id = record.Id,
            Description = record.Description,
            Tags = record.Tags.ToList()
        });
        _readWriteDbContext.SaveChanges();
        return Task.FromResult((IList<GetRecordDto>)records.ToList());
    }
}
