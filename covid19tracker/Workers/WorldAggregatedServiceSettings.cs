
namespace covid19tracker.Workers
{
    public class WorldAggregatedServiceSettings
    {
        public int CheckIntervalInHours { get; set; }

        public WorldAggregatedServiceSettings()
        {
            this.CheckIntervalInHours = 12;
        }
    }
}
