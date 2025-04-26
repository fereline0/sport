using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using shared.Enums;
using shared.Models;

namespace frontend.Services
{
    public class PickupPointService : BaseService
    {
        public PickupPointService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory) { }

        public Task<ServiceResult<List<PickupPoint>>> GetPickupPointsAsync() =>
            GetAsync<List<PickupPoint>>("PickupPoints");
    }
}
