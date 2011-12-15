using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoxPoliticus.Models;
using VoxPoliticus.Models.ViewModels;

namespace VoxPoliticus.Controllers
{
    public class FeedController : Controller
    {
        public ActionResult Index()
        {
            var stories = VoxPoliticusDatabase.Users.SelectMany(u => u.GetStories())
                .OrderByDescending(s => s.PublDate).Take(30);

            return View(new FeedViewModel(stories));
        }
    }
}
