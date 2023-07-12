using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly IReadWriteDbContext _readWriteDbContext;
    private readonly ILogger<AddRecordCommandHandler> logger;

    /// <summary>
    /// Indicate database context
    /// </summary>
    public AddRecordCommandHandler(IReadWriteDbContext readWriteDbContext, ILogger<AddRecordCommandHandler> logger)
    {
        _readWriteDbContext = readWriteDbContext;
        this.logger = logger;
    }

    /// <summary>
    /// Add record into databse
    /// </summary>
    public Task Handle(AddRecordCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Request to add record.");
        foreach (var tag in request.Tags)
        {
            var isStringValid = tag.Name.All(symbol => char.IsLetterOrDigit(symbol) == true);
            if (isStringValid == false)
            {
                logger.LogError("Records have not been added. Incorrect tag name, use symbols and or numbers");
                throw new ArgumentException("Records have not been added. Incorrect tag name, use symbols and or numbers");
            }
        }

        var atachedTags = new List<Tag>();
        var tagNames = request.Tags.Select(tag => tag.Name);
        var existingTags = _readWriteDbContext.Tags.Where(t => tagNames.Contains(t.Name)).ToList();
        var existingTagNames = existingTags.Select(t => t.Name).ToList();
        var newTags = request.Tags.Where(t => !existingTagNames.Contains(t.Name))
            .Select(t => new Tag() { Name = t.Name });

        var record = new Record()
        {
            Description = request.Description,
            Tags = existingTags.Union(newTags).ToList(),
        };

        _readWriteDbContext.Records.Add(record);
        _readWriteDbContext.SaveChanges();
        logger.LogInformation("Records have been added.");
        return Task.CompletedTask;
    }
}
