using KF.Records.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Infrastructure.Abstractions;
internal interface IReadWriteDbContext
{
    public DbSet<Record> records { get; set; }
}
