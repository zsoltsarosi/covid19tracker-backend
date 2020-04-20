
using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace covid19tracker.Model
{
    public class Country
    {
        [Key]
        [Index(0)]
        public string Iso2 { get; set; }

        [Index(1)]
        public string Iso3 { get; set; }

        [Required]
        [Index(2)]
        public string Name { get; set; }

        [Index(3)]
        public float Lat { get; set; }

        [Index(4)]
        public float Long { get; set; }

        [Index(5)]
        public int Population { get; set; }
    }
}
