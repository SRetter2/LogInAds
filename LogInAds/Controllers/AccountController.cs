using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LogInAds.Data;
using LogInAds.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace LogInAds.Controllers
{
    public class Account : Controller
    {
        private string _connection = "Data Source=.\\sqlexpress;Initial Catalog=ToDoItem;Integrated Security=True;";
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(string email, string password)
        {
            var pwDb = new PasswordDbMgr(_connection);
            pwDb.AddUser(email, password);
            return Redirect("/");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LogIn(string email, string password)
        {
            var pwDb = new PasswordDbMgr(_connection);
            var user = pwDb.Login(email, password);
            if (user == null)
            {
                return Redirect("/account/login");
            }           

            var claims = new List<Claim>
            {
                new Claim("user", email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
        public IActionResult MyAccount()
        {
            var adDb = new AdDbMgr(_connection);
            var pwDb = new PasswordDbMgr(_connection);
            string email = User.Identity.Name;
            var user = pwDb.GetUserByEmail(email);
            var vm = new MyAccountViewModel
            {
                Ads = adDb.GetAdsForUser(user.Id)
            };
            return View(vm);
        }
    }
}
