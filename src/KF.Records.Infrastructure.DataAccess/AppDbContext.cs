using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Infrastructure.DataAccess;

/// <summary>
/// Class provide access to database
/// </summary>
public class AppDbContext : DbContext, IReadWriteDbContext, IRecordRepository
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    /// <summary>
    /// Provide access to records in database
    /// </summary>
    public DbSet<Record> Records { get; set; }
    public DbSet<Tag> Tags { get; set; }

    IReadOnlyCollection<Record> IRecordRepository.Records => Records.ToList();

    /// <summary>
    /// Add record into database
    /// </summary>
    public void AddRecord(Record record)
    {
        List<Tag> atachedTags = new List<Tag>();
        foreach (var tag in record.Tags)
        {
            var exiistingTag = Tags.FirstOrDefault(t => t.Name == tag.Name);
            if (exiistingTag != null)
            {
                atachedTags.Add(exiistingTag);
            }
            else
            {
                atachedTags.Add(tag);
            }
        }
        record.Tags = atachedTags;
        Records.Add(record);
        SaveChanges();
    }

    /// <summary>
    /// Remove  record from database by id
    /// </summary>
    public void RemoveRecordById(int id)
    {
        Records.Remove(Records.Find(id));
        SaveChanges();
    }
}
