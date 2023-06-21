using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.AddRecord;

/// <summary>
/// Add record handler
/// </summary>
public class AddRecordCommandHandler : IRequestHandler<AddRecordCommand>
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
    public Task Handle(AddRecordCommand request, CancellationToken cancellationToken)
    {
        var record = new Record()
        {
            Description = request.Description,
            Tags = request.Tags.ToHashSet(),
        };

        foreach (var tag in request.Tags)
        {
            var isStringValid = tag.Name.All(symbol => char.IsLetterOrDigit(symbol) == true);
            if (isStringValid == false)
            {
                throw new ArgumentException("Incorrect tag name, use symbols and or numbers");
            }
        }

        _recordRepository.AddRecord(record);
        return Task.CompletedTask;
    }
}
