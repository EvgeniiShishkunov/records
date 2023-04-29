using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.AddRecord;

public class AddRecordCommandHandler
{
    private readonly IRecordRepository _recordRepository;

    public AddRecordCommandHandler(IRecordRepository recordRepository) 
    {
        _recordRepository = recordRepository;
    }

    public void Handle(AddRecordCommand request)
    {
        var record = new Record();
        record.Description = request.Description;
        record.Tags = request.Tags.ToHashSet();
        _recordRepository.AddRecord(record);
    }
}
