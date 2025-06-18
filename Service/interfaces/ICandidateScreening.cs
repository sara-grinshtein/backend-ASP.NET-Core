using Common.Dto;
using Repository.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Algorithm
{
    public interface ICandidateScreening
    {
        List<Volunteer> GetVolunteersAvailableNow();

        Task<List<VolunteerDto>> FilterVolunteersByDistanceAsync(double helpedLat, double helpedLng);

        List<VolunteerDto> FilterByKnowledge(List<VolunteerDto> filteredVolunteers, Message message);
    }
}
