
namespace covid19tracker.Workers
{
    public class RssNewsServiceSettings
    {
        public int CheckIntervalInMinutes { get; set; }

        public int RetentionInDays { get; set; }

        public RssNewsServiceSettings()
        {
            this.CheckIntervalInMinutes = 30;
            this.RetentionInDays = 3;
        }
    }
}
