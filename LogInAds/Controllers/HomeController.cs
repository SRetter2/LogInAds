using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LogInAds.Models;
using LogInAds.Data;
using Microsoft.AspNetCore.Authorization;

namespace LogInAds.Controllers
{
    public class HomeController : Controller
    {
        private string _connection = "Data Source=.\\sqlexpress;Initial Catalog=ToDoItem;Integrated Security=True;";
        public IActionResult Index()
        {
            AdDbMgr adDb = new AdDbMgr(_connection);
            PasswordDbMgr pwDb = new PasswordDbMgr(_connection);
            var vm = new HomePageViewModel();
            vm.Ads = adDb.GetAllAds();
            vm.LoggedIn = User.Identity.IsAuthenticated;
            if (vm.LoggedIn)
            {
                string email = User.Identity.Name;
                var user = pwDb.GetUserByEmail(email);
                vm.UserId = user.Id;
            }
            
            return View(vm);
        }

        [Authorize]
        public IActionResult NewAd()
        {
            var pwDb = new PasswordDbMgr(_connection);
            string email = User.Identity.Name;
            var user = pwDb.GetUserByEmail(email);
            return View(user.Id);
        }
        [HttpPost]
        public IActionResult NewAd(Ad ad)
        {
            var adDb = new AdDbMgr(_connection);
            adDb.AddAd(ad);
            return Redirect("/");
        }
        [HttpPost]
        public IActionResult DeleteAd(int id)
        {
            var adDb = new AdDbMgr(_connection);
            adDb.DeleteAd(id);
            return Redirect("/");
        }


    }
}
