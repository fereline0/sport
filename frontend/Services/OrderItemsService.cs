using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using shared.Enums;
using shared.Models;

namespace frontend.Services
{
    public class OrderItemsService : BaseService
    {
        public OrderItemsService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory) { }

        public Task<ServiceResult<OrderItem>> PostOrderItemAsync(OrderItem orderItem) =>
            PostAsync<OrderItem, OrderItem>("OrderItems", orderItem);

        public Task<ServiceResult> DeleteOrderItemAsync(int id) => DeleteAsync($"OrderItems/{id}");
    }
}
