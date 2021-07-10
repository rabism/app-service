using Users.API.Models;

namespace Users.API.Services
{
    public interface IUserService
    {
        void ChangePassword(UserDetails user);
        UserDetails Login(UserDetails user);
        void Register(UserDetails user);
        void AddUser(UserDetails usr);
    }
}