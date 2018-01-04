using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignUp.Web.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}
