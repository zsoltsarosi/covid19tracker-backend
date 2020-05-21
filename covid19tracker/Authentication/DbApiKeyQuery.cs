using covid19tracker.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace covid19tracker.Authentication
{
    public class DbApiKeyQuery : IApiKeyQuery
    {
        private readonly ApiKeyContext _context;

        public DbApiKeyQuery(ApiKeyContext context, ILogger<DbApiKeyQuery> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<ApiKey> Execute(string apiKeyGuid)
        {
            if (string.IsNullOrEmpty(apiKeyGuid))
            {
                return null;
            }

            var apiKey = _context.ApiKeys.FirstOrDefault(x => x.Key == Guid.Parse(apiKeyGuid) && x.Expiration > DateTime.UtcNow);
            return Task.FromResult(apiKey);
        }

    }
}
