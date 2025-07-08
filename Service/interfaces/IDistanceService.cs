using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.interfaces
{
    public interface IDistanceService
    {
        Task<int?> GetDistanceInMetersAsync(double originLat, double originLng, double destLat, double destLng);
    }
}
