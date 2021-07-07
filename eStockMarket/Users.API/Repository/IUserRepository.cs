using Users.API.Models;

namespace Users.API.Repository
{
    public interface IUserRepository
    {
        void AddUser(UserDetails user);
        void ChangePassword(UserDetails user);
        UserDetails GetUser(UserDetails user);
        UserDetails GetUserByMail(string email);
        UserDetails GetUserByCredential(UserDetails user);
    }
}