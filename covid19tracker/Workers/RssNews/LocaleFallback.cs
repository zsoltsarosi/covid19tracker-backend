using System;
using System.Linq;

namespace covid19tracker.Workers.RssNews
{
    public class LocaleFallback
    {
        private string[] country = new[] { "HU", "DE", "AT", "CH", "BE", "UK", "JP", "US" };
        private string[] languages = new[] { "hu", "de", "fr", "nl", "jp", "en" };

        public Tuple<string, string> GetBestLocaleAndCountry(string language, string country)
        {
            // fallback
            if (string.IsNullOrEmpty(language)) language = "en";
            if (string.IsNullOrEmpty(country)) country = "US";

            language = language.ToLower();
            country = country.ToUpper();

            // fallback
            if (!languages.Contains(language)) return new Tuple<string, string>("en-US", "US");

            if (language == "hu") return new Tuple<string, string>("hu-HU", "HU");
            if (language == "jp") return new Tuple<string, string>("jp-JP", "JP");
            if (language == "fr") return new Tuple<string, string>("fr-BE", "BE");
            if (language == "nl") return new Tuple<string, string>("nl-BE", "BE");

            if (language == "de")
            {
                if (country == "DE") return new Tuple<string, string>("de-DE", "DE");
                if (country == "AT") return new Tuple<string, string>("de-AT", "AT");
                if (country == "CH") return new Tuple<string, string>("de-CH", "CH");

                // fallback
                return new Tuple<string, string>("de-DE", "DE");
            }

            if (language == "en")
            {
                if (country == "US") return new Tuple<string, string>("en-US", "US");
                if (country == "UK" || country == "GB") return new Tuple<string, string>("en-GB", "UK");

                // fallback
                return new Tuple<string, string>("en-US", "US");
            }

            // fallback
            return new Tuple<string, string>("en-US", "US");
        }
    }
}
