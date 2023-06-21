﻿using KF.Records.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Infrastructure.Abstractions;

/// <summary>
/// Interface to read and write entity in database
/// </summary>
public interface IReadWriteDbContext
{
    /// <summary>
    /// Provide records in db
    /// </summary>
    public DbSet<Record> Records { get; set; }
}
