using System;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Mvc;
using SignUp.Entities;
using SignUp.Web.Models;
using UserContext = SignUp.Web.Context.UserContext;

namespace SignUp.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserContext _ctx;

        public RegisterController(UserContext _ctx)
        {
            this._ctx = _ctx;
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(UserViewModel vm)
        {
            var user = MapUser(vm);
            _ctx.Users.Add(user);
            _ctx.SaveChanges();

            return View(user);
        }


        private User MapUser(UserViewModel vm)
        {
            return new User
            {

                CreatedDate = DateTime.UtcNow,
                Email = vm.Email,
                LastLoginDate = null,
                Password = vm.Password,
                Username = vm.Username

            };
        }
    }
}
