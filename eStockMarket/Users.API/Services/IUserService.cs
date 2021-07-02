using Users.API.Models;

namespace Users.API.Services
{
    public interface IUserService
    {
        void ChangePassword(UserDetails user);
        bool Login(UserDetails user);
        void Register(UserDetails user);
        void AddUser(UserDetails usr);
    }
}