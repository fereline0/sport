using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using shared.Models;

namespace frontend.Services
{
    class UserService : BaseService
    {
        public UserService(HttpClient httpClient)
            : base(httpClient) { }

        public Task<List<User>> GetAllAsync() => GetAsync<List<User>>("Users");
    }
}
