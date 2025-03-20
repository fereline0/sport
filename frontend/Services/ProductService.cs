using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using shared.Models;

namespace frontend.Services
{
    public class ProductService : BaseService
    {
        public ProductService(HttpClient httpClient)
            : base(httpClient) { }

        public Task<List<Product>> GetAllAsync() => GetAsync<List<Product>>("Products");
    }
}
