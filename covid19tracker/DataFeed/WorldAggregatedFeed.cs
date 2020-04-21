using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using covid19tracker.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace covid19tracker.DataFeed
{
    public class WorldAggregatedFeed
    {
        private const string _url = "https://api.github.com/repos/datasets/covid-19/contents/data/worldwide-aggregated.csv";

        private ILogger<WorldAggregatedFeed> _logger;
        private WorldAggregatedContext _dbContext;

        public WorldAggregatedFeed(WorldAggregatedContext dbContext, ILogger<WorldAggregatedFeed> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IList<WorldAggregated>> GetData()
        {
            var downloadUrl = await this.GetDownloadUrl();
            //return Enumerable.Empty<WorldAggregated>().ToList();
            var result = await _dbContext.WorldData.ToListAsync();
            return result;
        }

        private async Task<string> GetDownloadUrl()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("covid19tracker app");

                using (var response = await httpClient.GetAsync(_url))
                {
                    using (var content = response.Content)
                    {
                        //get the json result from your api
                        var result = await content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode) throw new InvalidOperationException(result);

                        using (JsonDocument document = JsonDocument.Parse(result))
                        {
                            JsonElement root = document.RootElement;
                            var downloadUrl = root.GetProperty("download_url").GetString();
                            return downloadUrl;
                        }
                    }
                }
            }
        }
    }
}
