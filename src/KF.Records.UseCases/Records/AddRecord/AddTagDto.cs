using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.AddRecord;

/// <summary>
/// Tag dto
/// </summary>
public class AddTagDto
{
    /// <summary>
    /// Tag name
    /// </summary>
    required public string Name { get; set; }
}
