using System;
using Microsoft.AspNetCore.Mvc;
using SignUp.Entities;
using SignUp.Messaging.Constants.Events;
using SignUp.Messaging.Constants.RabbitMqManager;
using SignUp.Web.Models;

namespace SignUp.Web.Controllers
{
    public class RegisterController : Controller
    {
       
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(UserViewModel vm)
        {

            var manager = new RabbitMqManager<RegisteredUserEvent>();
            manager.Send(new RegisteredUserEvent
            {
                User = MapUser(vm)
            });

            return View();
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
