using AutoMapper;
using AutoMapper.QueryableExtensions;
using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using KF.Records.UseCases.Records.AddRecord;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    private readonly IReadWriteDbContext readWriteDbContext;
    private readonly ILogger<GetAllRecordsQueryHandler> logger;
    private readonly IMapper mapper;

    /// <summary>   
    /// Indicate database context
    /// </summary>
    public GetAllRecordsQueryHandler(IReadWriteDbContext readWriteDbContext, ILogger<GetAllRecordsQueryHandler> logger, IMapper mapper)
    {
        this.readWriteDbContext = readWriteDbContext;
        this.logger = logger;
        this.mapper = mapper;
    }

    /// <summary>
    /// Return all records from database
    /// </summary>
    public async Task<IList<GetRecordDto>> Handle(GetAllRecordsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Request for all records");
        var records = await readWriteDbContext.Records.ProjectTo<GetRecordDto>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        var RecordCount = records.Count;
        logger.LogInformation("Request for all records completed. Total count {RecordCount}", RecordCount);
        return records;
    }
}
