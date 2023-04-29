using KF.Records.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.RemoveRecord;

public class RemoveRecordCommandHandler
{
    private readonly IRecordRepository _recordRepository;

    public RemoveRecordCommandHandler(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    public void Handle(RemoveRecordCommand request)
    {
        _recordRepository.RemoveRecordByID(request.Id);
    }
}
