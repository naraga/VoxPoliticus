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
        public ActionResult Index(string id)
        {
            IEnumerable<Story> stories = null;

            if (string.IsNullOrEmpty(id))
                stories = VoxPoliticusDatabase.Users.SelectMany(u => u.GetStories());
            else
            {
                var user = GetUser(id);
                if (user != null)
                    stories = user.GetStories();
                else
                {
                    var searchTags = id.Split(',', ';', '+');
                    stories = VoxPoliticusDatabase.Users.SelectMany(u => u.GetStories()).Where(s => s.Tags.Intersect(searchTags).Count() == searchTags.Count());
                }
            }

            return View(new FeedViewModel(stories.OrderByDescending(s => s.PublDate).Take(50)));
        }

        private static User GetUser(string id)
        {
            return VoxPoliticusDatabase.Users.SingleOrDefault(u => u.Id == id);
        }
    }
}
