using System;
using System.ComponentModel.DataAnnotations;

namespace covid19tracker.Model
{
    public class LastUpdate
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }

    public enum DataFeedType
    {
        CountriesAggregated,
        WorldAggregated,
        RssNews
    }
}
