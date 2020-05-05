using CsvHelper.Configuration.Attributes;
using System;

namespace covid19tracker.Model
{
    public class CountryAggregated
    {
        [Index(0)]
        public DateTime Date { get; set; }

        [Index(1)]
        public string Country { get; set; }

        [Index(2)]
        public int Confirmed { get; set; }

        [Index(3)]
        public int Recovered { get; set; }

        [Index(4)]
        public int Deaths { get; set; }
    }
}
