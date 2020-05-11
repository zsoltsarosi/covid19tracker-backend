
namespace covid19tracker.Workers
{
    public class RssNewsServiceSettings
    {
        public int CheckIntervalInMinutes { get; set; } = 30;

        public int RetentionInDays { get; set; } = 3;

        public int MaxNewsInsertAtOnce { get; set; } = 10;
    }
}
