using CsvHelper.Configuration.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace covid19tracker.Model
{
    public class WorldAggregated
    {
        [Key]
        [Index(0)]
        public DateTime Date { get; set; }

        [Index(1)]
        public int Confirmed { get; set; }

        [Index(2)]
        public int Recovered { get; set; }

        [Index(3)]
        public int Deaths { get; set; }

        [Index(4)]
        public float? IncreaseRate { get; set; }
    }
}
