using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Cli;

public class Record
{
    public int Id { get; set; }
    public string Description { get; set; }
    public HashSet<string> Tags { get; set; }
}
