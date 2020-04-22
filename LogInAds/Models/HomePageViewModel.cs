using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogInAds.Data;

namespace LogInAds.Models
{
    public class HomePageViewModel
    {
         public List<Ad> Ads { get; set; }
         public int? UserId { get; set; }
         public bool LoggedIn { get; set; }
    }
}
