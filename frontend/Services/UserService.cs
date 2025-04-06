using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using shared.Enums;
using shared.Models;

namespace frontend.Services
{
    public class UserService : BaseService
    {
        public UserService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory) { }

        public Task<ServiceResult<User>> GetUserAsync(int id) => GetAsync<User>($"Users/${id}");

        public Task<ServiceResult<User>> GetAuthedUserAsync() => GetAsync<User>("Users/me");

        public Task<ServiceResult<List<User>>> GetUsersAsync() => GetAsync<List<User>>("Users");

        public Task<ServiceResult<List<Order>>> GetOrdersByUserIdAsync(
            int id,
            OrderStatus? orderStatus = null
        )
        {
            var baseUrl = $"Users/{id}/Orders";
            var query = HttpUtility.ParseQueryString(string.Empty);

            if (orderStatus.HasValue)
            {
                query["status"] = ((int)orderStatus.Value).ToString();
            }

            return GetAsync<List<Order>>($"{baseUrl}?{query}");
        }
    }
}
