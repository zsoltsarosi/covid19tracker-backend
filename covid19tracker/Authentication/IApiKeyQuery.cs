using covid19tracker.Model;
using System.Threading.Tasks;

namespace covid19tracker.Authentication
{
    public interface IApiKeyQuery
    {
        Task<ApiKey> Execute(string apiKey);
    }
}
