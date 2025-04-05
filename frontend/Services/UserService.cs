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

        public Task<ServiceResult<User>> GetUserAsync(int userId) =>
            GetAsync<User>($"Users/${userId}");

        public Task<ServiceResult<User>> GetAuthedUserAsync() => GetAsync<User>("Users/me");

        public Task<ServiceResult<List<User>>> GetUsersAsync() => GetAsync<List<User>>("Users");

        public Task<ServiceResult<List<Order>>> GetOrdersByUserIdAsync(
            int userId,
            OrderStatus? orderStatus = null
        )
        {
            var baseUrl = $"Users/{userId}/Orders";
            var query = HttpUtility.ParseQueryString(string.Empty);

            if (orderStatus.HasValue)
            {
                query["status"] = ((int)orderStatus.Value).ToString();
            }

            return GetAsync<List<Order>>($"{baseUrl}?{query}");
        }
    }
}
