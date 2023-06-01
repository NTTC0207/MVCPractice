using Microsoft.EntityFrameworkCore;
using MVCTutorial.Data;
using MVCTutorial.Interfaces;
using MVCTutorial.Models;

namespace MVCTutorial.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;    
        }
        public bool Add(AppUser appUser)
        {
         _context.Add(appUser);
            return Save();
        }

        public bool Delete(AppUser appUser)
        {
            _context.Remove(appUser);
            return Save();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            var getUser = await _context.Users.ToListAsync();
            return getUser;

        }

        public async Task<AppUser> GetUserById(string id )
        {
            return await _context.Users.FindAsync(id);
        }

        public bool Save()
        {
          var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser appUser)
        {
            _context.Update(appUser);
            return Save();
        }
    }
}
