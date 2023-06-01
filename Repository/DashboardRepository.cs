using MVCTutorial.Data;
using MVCTutorial.Interfaces;
using MVCTutorial.Models;

namespace MVCTutorial.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DashboardRepository(ApplicationDbContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
       public async Task<List<Club>> GetAllUserClubs()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userClub = _context.Clubs.Where(r => r.AppUser.Id == curUser);
            return userClub.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userRace = _context.Races.Where(r => r.AppUser.Id == curUser);
            return userRace.ToList();
        }
    }
}
