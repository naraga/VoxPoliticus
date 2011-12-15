﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace VoxPoliticus.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public IEnumerable<Source> Sources { get; set; }
        public IEnumerable<Story> GetStories()
        {
            foreach (var story in Sources.SelectMany(source => source.GetStories()))
            {
                story.User = this;
                yield return story;
            }
        }
    }

    public abstract class Source
    {
        public abstract IEnumerable<Story> GetStories();
    }


    public class RssSource: Source
    {
        public RssSource(string url)
        {
            Url = url;
        }

        public User User { get; set; }
        public string Url { get; set; }

        public override IEnumerable<Story> GetStories()
        {
            var reader = XmlReader.Create(Url);
            var feed = SyndicationFeed.Load(reader);

            if (feed != null)
                foreach (var item in feed.Items)
                {
                    DateTime? purlDate = item.ElementExtensions.ReadElementExtensions<DateTime>("date", "http://purl.org/dc/elements/1.1/").FirstOrDefault();
                    yield return new Story 
                    {
                        Url = item.Links[0].Uri.ToString(),
                        Title = item.Title.Text,
                        PublDate = item.PublishDate.DateTime != DateTime.MinValue ? item.PublishDate.DateTime : purlDate.Value,
                        Description = item.Summary.Text
                    };
                }
        }
    }

    public class Story
    {
        public User User { get; set; }
        public string Url { get; set; }
        public DateTime PublDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

}