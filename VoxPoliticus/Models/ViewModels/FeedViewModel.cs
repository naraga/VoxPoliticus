using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoxPoliticus.Models.ViewModels
{
    public class FeedViewModel
    {
        public IEnumerable<FeedItemViewModel> FeedItems { get; set; }

        public FeedViewModel(IEnumerable<Story> stories)
        {
            FeedItems = stories.Select(s => new FeedItemViewModel(s));
        }
    }

    public class FeedItemViewModel
    {
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime PubDate { get; set; }
        public FeedItemViewModel(Story story)
        {
            UserName = story.User.Name;
            Title = story.Title;
            PubDate = story.PublDate;
        }
    }
    
}