using MVCTutorial.Models;

namespace MVCTutorial.Interfaces
{
    public interface IDashboardRepository
    {

        Task<List<Race>> GetAllUserRaces();
        Task<List<Club>> GetAllUserClubs();
    }
}
