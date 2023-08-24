using KF.Records.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Infrastructure.Abstractions.Export;

/// <summary>
/// Record model for sending to email
/// </summary>
public class RecordEmailModel
{
    /// <summary>
    /// Record Description.
    /// </summary>
    required public string Description { get; set; }

    /// <summary>
    /// Record tags.
    /// </summary>
    required public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
}
