using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace covid19tracker.Model
{
    public class ApiKey
    {
        [Key]
        public Guid Key { get; set; }

        [Required]
        public string Owner { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Expiration { get; set; }

        [Required]
        public IEnumerable<string> Roles { get; set; }

        internal static TimeSpan CDefaultExpiration = TimeSpan.FromDays(2 * 365);
    }

    internal enum Roles
    {
        WorldData,
        CountryData,
        News,
    }
}
