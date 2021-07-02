using Users.API.Models;
using Users.API.Repository;

namespace Users.API.Services
{
    public class UserService : IUserService
    {
        readonly IUserRepository repository;
        public UserService(IUserRepository userRepository)
        {
            repository = userRepository;
        }

        public void Register(UserDetails user)
        {
            repository.AddUser(user);
        }

        public void ChangePassword(UserDetails user)
        {
            var _user = repository.GetUser(user);
            if (_user != null)
            {
                _user.Password = user.Password;
                repository.ChangePassword(_user);
            }
            else
                throw new UserNotFoundException("This user id does not exists");
        }

        public bool Login(UserDetails user)
        {
            var _user = repository.GetUser(user);
            if (_user != null)
                return true;
            else
                throw new UserNotFoundException("Invalid credentials");
        }
        public void AddUser(UserDetails usr)
        {
            if (repository.GetUserByMail(usr.Email) != null)
            {
                throw new UserAlreadyExistsException("User with given email exists");
            }
            repository.AddUser(usr);
        }

    }
}
