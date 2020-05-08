
namespace covid19tracker.Workers
{
    public class CountriesAggregatedServiceSettings
    {
        public int CheckIntervalInHours { get; set; }

        public CountriesAggregatedServiceSettings()
        {
            this.CheckIntervalInHours = 12;
        }
    }
}
