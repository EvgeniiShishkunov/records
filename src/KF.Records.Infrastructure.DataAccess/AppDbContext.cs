using KF.Records.Domain;
using KF.Records.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Infrastructure.DataAccess;

/// <summary>
/// Class provide access to database
/// </summary>
public class AppDbContext : IReadWriteDbContext
{
    /// <summary>
    /// Provide access to records in database
    /// </summary>
    public DbSet<Record> Records { get; set; }
}
