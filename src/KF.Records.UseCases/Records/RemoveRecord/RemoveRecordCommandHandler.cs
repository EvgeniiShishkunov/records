using KF.Records.Infrastructure.Abstractions;
using KF.Records.UseCases.Records.GetAllRecords;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.RemoveRecord;

/// <summary>
/// Remove record handler
/// </summary>
public class RemoveRecordCommandHandler : IRequestHandler<RemoveRecordCommand>
{
    private readonly IReadWriteDbContext _readWriteDbContext;
    private readonly ILogger<RemoveRecordCommandHandler> logger;

    /// <summary>
    /// Indicate database context
    /// </summary>
    public RemoveRecordCommandHandler(IReadWriteDbContext readWriteDbContext, ILogger<RemoveRecordCommandHandler> logger)
    {
        _readWriteDbContext = readWriteDbContext;
        this.logger = logger;
    }

    /// <summary>
    /// Reamove record from database. Indicate record id
    /// </summary>
    public Task Handle(RemoveRecordCommand request, CancellationToken cancellationToken)
    {
        var id = request.Id;
        logger.LogInformation("Request to delete record with id {id} ", id);
        var removingRecord = _readWriteDbContext.Records.FirstOrDefault(r => r.Id == request.Id);
        _readWriteDbContext.Records.Remove(removingRecord);
        _readWriteDbContext.SaveChanges();
        logger.LogInformation("Record with id {id} have been deleted", id);
        return Task.CompletedTask;
    }
}
