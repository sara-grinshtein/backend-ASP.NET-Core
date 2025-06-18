using Repository.Entites;
using System.Collections.Generic;

namespace Service.Algorithm
{
    public interface IDataFetcher
    {
        List<Message> GetOpenMessages();
        List<Volunteer> GetAvailableVolunteers();
    }
}
