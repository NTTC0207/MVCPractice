using MVCTutorial.Models;

namespace MVCTutorial.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);

        bool Save();
        bool Update(AppUser appUser);
        bool Delete(AppUser appUser);
        bool Add(AppUser appUser);
    }
}
