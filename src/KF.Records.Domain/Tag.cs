using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Domain;

/// <summary>
/// Tags to record.
/// </summary>
public class Tag
{
    /// <summary>
    /// Tag name.
    /// </summary>
    [Key]
    [MaxLength(150)]
    required public string Name { get; set; }

    /// <summary>
    /// Records with this tag.
    /// </summary>
    public IReadOnlyCollection<Record> Records { get; set; }
}
