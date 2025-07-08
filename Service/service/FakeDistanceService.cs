using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.interfaces;

namespace Service.service
{
    public class FakeDistanceService : IDistanceService
    {
        public Task<int?> GetDistanceInMetersAsync(double originLat, double originLng, double destLat, double destLng)
        {
            return Task.FromResult<int?>(5000); // תמיד יחזיר מרחק "תקין"
        }
    }

}
