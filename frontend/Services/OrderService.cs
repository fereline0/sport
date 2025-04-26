using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using shared.Enums;
using shared.Models;

namespace frontend.Services
{
    public class OrderService : BaseService
    {
        public OrderService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory) { }

        public Task<ServiceResult<OrderItem>> GetOrderItemByOrderAndProductIdAsync(
            int id,
            int productId
        ) => GetAsync<OrderItem>($"Orders/{id}/OrderItems/{productId}");

        public Task<ServiceResult<Order>> PostOrderAsync(Order order) =>
            PostAsync<Order, Order>("Orders", order);

        public Task<ServiceResult<Order>> PutOrderAsync(Order order) =>
            PutAsync<Order, Order>($"Orders/{order.Id}", order);
    }
}
