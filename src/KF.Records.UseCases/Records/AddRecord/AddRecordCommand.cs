﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.AddRecord;

/// <summary>
/// Parametrs for adding record
/// </summary>
public class AddRecordCommand
{
    /// <summary>
    /// Record description
    /// </summary>
    public required string Description { get; init; }
    /// <summary>
    /// Record tags
    /// </summary>
    public required IReadOnlyCollection<string> Tags { get; init; }
}