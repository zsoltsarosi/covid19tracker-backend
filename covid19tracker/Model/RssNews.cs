using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace covid19tracker.Model
{
    public class RssNews
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [JsonIgnore]
        public string FeedId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Link { get; set; }

        public string SourceName { get; set; }

        public string SourceUrl { get; set; }
    }
}
