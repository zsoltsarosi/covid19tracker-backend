using System;
using System.ComponentModel.DataAnnotations;

namespace covid19tracker.Model
{
    public class VersionUpdate
    {
        [Key]
        public string Version { get; set; }

        [Required]
        public DateTime Expiration { get; set; }
    }
}
