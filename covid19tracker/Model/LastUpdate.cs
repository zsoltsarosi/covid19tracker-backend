using System;
using System.ComponentModel.DataAnnotations;

namespace covid19tracker.Model
{
    public class LastUpdate
    {
        [Key]
        public DataFeedType DataFeed { get; set; }

        public DateTime Date { get; set; }
    }

    public enum DataFeedType
    {
        WorldAggregated,
    }
}
