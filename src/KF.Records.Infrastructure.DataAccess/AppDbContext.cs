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
public class AppDbContext : DbContext, IReadWriteDbContext
{
    /// <summary>
    /// Indicate database connection options
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    /// <summary>
    /// Provide access to records in database
    /// </summary>
    public DbSet<Record> Records { get; set; }
    /// <summary>
    /// Provide access to tags in database
    /// </summary>
    public DbSet<Tag> Tags { get; set; }
}
