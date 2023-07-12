using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using KF.Records.UseCases.Records.AddRecord;
using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<GetAllRecordsQueryHandler> logger;

    /// <summary>   
    /// Indicate database context
    /// </summary>
    public GetAllRecordsQueryHandler(IReadWriteDbContext readWriteDbContext, ILogger<GetAllRecordsQueryHandler> logger)
    {
        _readWriteDbContext = readWriteDbContext;
        this.logger = logger;
    }

    /// <summary>
    /// Return all records from database
    /// </summary>
    public Task<IList<GetRecordDto>> Handle(GetAllRecordsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Request for all records");
        var records = _readWriteDbContext.Records.Select(record => new GetRecordDto()
        {
            Id = record.Id,
            Description = record.Description,
            Tags = record.Tags.ToList()
        });
        var recordCount = records.Count();
        logger.LogInformation("Request for all records completed. Total count {recordCount}", recordCount);
        return Task.FromResult((IList<GetRecordDto>)records.ToList());
    }
}
