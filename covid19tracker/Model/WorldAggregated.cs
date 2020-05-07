using CsvHelper.Configuration.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace covid19tracker.Model
{
    public class WorldAggregated
    {
        [Key]
        [Index(0)]
        [JsonPropertyName("t")]
        // TODO: serialize only date part
        public DateTime Date { get; set; }

        [Index(1)]
        [JsonPropertyName("c")]
        public int Confirmed { get; set; }

        [Index(2)]
        [JsonPropertyName("r")]
        public int Recovered { get; set; }

        [Index(3)]
        [JsonPropertyName("d")]
        public int Deaths { get; set; }

        [Index(4)]
        [JsonPropertyName("i")]
        public float? IncreaseRate { get; set; }
    }
}
