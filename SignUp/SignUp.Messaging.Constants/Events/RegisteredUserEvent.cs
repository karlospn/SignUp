using SignUp.Entities;

namespace SignUp.Messaging.Constants.Events
{
    public class RegisteredUserEvent
    {
        public User User { get; set; }
    }
}
