using System;

namespace SignUp.Entities
{
    public  class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}
