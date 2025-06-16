using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;

namespace Service.interfaces
{
    public interface IEmbeddingAlgorithmService
    {
        Task<List<VolunteerDto>> FilterVolunteersByDistanceAsync(double helpedLat, double helpedLng);
 
    }

}
