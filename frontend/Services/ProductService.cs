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

        public Task<ServiceResult<List<Product>>> GetAllAsync() =>
            GetAsync<List<Product>>("Products");
    }
}
