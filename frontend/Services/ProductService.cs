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

        public Task<ServiceResult<Product>> PostProductAsync(Product product) =>
            PostAsync<Product, Product>("Products", product);

        public Task<ServiceResult<Product>> PutProductAsync(Product product) =>
            PutAsync<Product, Product>($"Products/{product.Id}", product);

        public Task<ServiceResult> DeleteProductAsync(Product product) =>
            DeleteAsync($"Products/{product.Id}");

        public Task<ServiceResult<List<Product>>> GetProductsAsync() =>
            GetAsync<List<Product>>("Products");

        public Task<ServiceResult<List<Product>>> GetProductsByOrderAsync(int id) =>
            GetAsync<List<Product>>($"Products/Orders/{id}");
    }
}
