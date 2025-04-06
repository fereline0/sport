using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using shared.Models;

namespace frontend.Services
{
    public class ProductService : BaseService
    {
        public ProductService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory) { }

        public Task<ServiceResult<Product>> GetProductAsync(int id) =>
            GetAsync<Product>($"Products/{id}");

        public Task<ServiceResult<List<Product>>> GetProductsAsync() =>
            GetAsync<List<Product>>("Products");
    }
}
