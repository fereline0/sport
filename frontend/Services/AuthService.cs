using System.Net.Http;
using System.Threading.Tasks;
using shared.Models;

namespace frontend.Services
{
    public class AuthService : BaseService
    {
        public AuthService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory) { }

        public Task<ServiceResult<Auth>> PostAuthAsync(User user) =>
            PostAsync<User, Auth>("Auth/login", user);
    }
}
