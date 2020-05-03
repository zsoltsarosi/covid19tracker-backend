
namespace covid19tracker.Workers
{
    public class RssNewsServiceSettings
    {
        public int CheckIntervalInMinutes { get; set; }

        public RssNewsServiceSettings()
        {
            this.CheckIntervalInMinutes = 30;
        }
    }
}
