using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Domain;

/// <summary>
/// Record with description and tags.
/// </summary>
public class Record
{
    /// <summary>
    /// Record Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Record Description.
    /// </summary>
    required public string Description { get; set; }

    /// <summary>
    /// Record tags.
    /// </summary>
    required public HashSet<Tag> Tags { get; set; } = new HashSet<Tag>();
}
