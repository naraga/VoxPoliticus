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
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime PubDate { get; set; }
        public string PhotoUrl { get; set; }
        public string Source { get; set; }
        public string[] Tags { get; set; }
        public string Url { get; set; }

        public FeedItemViewModel(Story story)
        {
            UserId = story.User.Id;
            UserName = story.User.Name;
            Title = story.Title;
            Summary = story.Description;
            Tags = story.Tags;
            PubDate = story.PublDate;
            PhotoUrl = story.User.PhotoUrl;
            Source = story.Source.ToString();
            Url = story.Url;
        }
    }
    
}