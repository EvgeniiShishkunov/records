using KF.Records.Infrastructure.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

    /// <summary>
    /// Indicate database context
    /// </summary>
    public RemoveRecordCommandHandler(IReadWriteDbContext readWriteDbContext)
    {
        _readWriteDbContext = readWriteDbContext;
    }

    /// <summary>
    /// Reamove record from database. Indicate record id
    /// </summary>
    public Task Handle(RemoveRecordCommand request, CancellationToken cancellationToken)
    {
        var removingRecord = _readWriteDbContext.Records.FirstOrDefault(r => r.Id == request.Id);
        _readWriteDbContext.Records.Remove(removingRecord);
        _readWriteDbContext.SaveChanges();
        return Task.CompletedTask;
    }
}
