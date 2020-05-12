using CsvHelper.Configuration.Attributes;
using System;
using System.Text.Json.Serialization;

namespace covid19tracker.Model
{
    public class CountryAggregated
    {
        [Index(0)]
        [JsonPropertyName("t")]
        public DateTime Date { get; set; }

        [Index(1)]
        [JsonPropertyName("n")]
        public string Country { get; set; }

        [Index(2)]
        [JsonPropertyName("c")]
        public int Confirmed { get; set; }

        [Index(3)]
        [JsonPropertyName("r")]
        public int Recovered { get; set; }

        [Index(4)]
        [JsonPropertyName("d")]
        public int Deaths { get; set; }
    }
}
