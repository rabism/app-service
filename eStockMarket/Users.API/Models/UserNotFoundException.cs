using System;

namespace Users.API.Models
{
    public class UserNotFoundException : ApplicationException
    {
        public UserNotFoundException(string message) : base(message) { }
    }
}
