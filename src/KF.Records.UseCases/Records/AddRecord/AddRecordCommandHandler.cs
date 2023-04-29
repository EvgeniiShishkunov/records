using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.AddRecord;

/// <summary>
/// Add record handler
/// </summary>
public class AddRecordCommandHandler
{
    private readonly IRecordRepository _recordRepository;

    /// <summary>
    /// Indicate database context
    /// </summary>
    public AddRecordCommandHandler(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    /// <summary>
    /// Add record into databse
    /// </summary>
    public void Handle(AddRecordCommand request)
    {
        var record = new Record()
        {
            Description = request.Description,
            Tags = request.Tags.ToHashSet(),
        };
        _recordRepository.AddRecord(record);
    }
}
