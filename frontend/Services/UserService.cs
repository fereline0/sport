using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using shared.Models;

namespace frontend.Services
{
    public class UserService : BaseService
    {
        public UserService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory) { }

        public Task<ServiceResult<List<User>>> GetAllAsync() => GetAsync<List<User>>("Users");
    }
}
