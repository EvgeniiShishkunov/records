using KF.Records.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.RemoveRecord;

/// <summary>
/// Remove record handler
/// </summary>
public class RemoveRecordCommandHandler
{
    private readonly IRecordRepository _recordRepository;

    /// <summary>
    /// Indicate database context
    /// </summary>
    public RemoveRecordCommandHandler(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    /// <summary>
    /// Reamove record from database. Indicate record id
    /// </summary>
    public void Handle(RemoveRecordCommand request)
    {
        _recordRepository.RemoveRecordByID(request.Id);
    }
}
