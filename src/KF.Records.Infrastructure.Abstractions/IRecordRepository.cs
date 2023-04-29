﻿using KF.Records.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.Infrastructure.Abstractions
{
    /// <summary>
    /// Represent record repository
    /// </summary>
    public interface IRecordRepository
    {
        /// <summary>
        /// Represent read only records data
        /// </summary>
        public IReadOnlyCollection<Record> Records { get; }

        /// <summary>
        /// Add record into database
        /// </summary>
        public void AddRecord(Record record)
        {
        }

        /// <summary>
        /// Remove record from database by id
        /// </summary>
        public void RemoveRecordByID(int id)
        {
        }
    }
}
