﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;
using TweetSharp;

namespace VoxPoliticus.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string PhotoUrl { get; set; }
        public string[] Tags { get; set; }

        public IEnumerable<Source> Sources { get; set; }
        public IEnumerable<Story> GetStories()
        {
            foreach (var story in Sources.SelectMany(source => source.GetStories()))
            {
                story.User = this;
                story.Tags = story.Tags.Union(Tags).ToArray();
                story.InferTags();
                yield return story;
            }
        }
    }

    public abstract class Source
    {
        public User User { get; set; }
        public string Url { get; set; }

        protected Source(string url)
        {
            Url = url;
        }

        public abstract IEnumerable<Story> GetStories();
    }


    public class RssSource: Source
    {
        public RssSource(string url) : base(url)
        {
        }

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
                        Description = item.Summary.Text,
                        Tags = new[] { "blog" },
                        Source = StorySource.Blog
                    };
                }
        }
    }

    public class TwitterSource : Source
    {
        public TwitterSource(string url): base(url){}

        public override IEnumerable<Story> GetStories()
        {
            var reader = XmlReader.Create(Url);
            var feed = SyndicationFeed.Load(reader);

            if (feed != null)
                foreach (var item in feed.Items)
                {
                    yield return new Story
                    {
                        Url = item.Links[0].Uri.ToString(),
                        Title = item.Title.Text,
                        PublDate = item.PublishDate.DateTime,
                        Tags = new[]{"twitter"},
                        Source = StorySource.Twitter
                        
                    };
                }
        }
    }


    public class FacebookSource : Source
    {
        public FacebookSource(string url) : base(url) { }

        public override IEnumerable<Story> GetStories()
        {
            XmlReader reader;
            if (Url.StartsWith("http"))
            {
                var req = (HttpWebRequest) WebRequest.Create(Url);
                req.Method = "GET";
                req.UserAgent = "Fiddler";

                var rep = req.GetResponse();
                reader = XmlReader.Create(rep.GetResponseStream());
            }
            else
                reader = XmlReader.Create(Url);

            
            var feed = SyndicationFeed.Load(reader);

            //var writer = XmlWriter.Create("c:\\Proj\\VoxPoliticus\\VoxPoliticus\\Content\\LocalData\\atom10.atom");
            //feed.SaveAsAtom10(writer);
            //writer.Flush();
            //writer.Close();
            

            if (feed != null)
                foreach (var item in feed.Items)
                {
                yield return new Story
                {
                    Url = item.Links[0].Uri.ToString(),
                    Title = item.Title.Text,
                    Description = item.Content as TextSyndicationContent != null ? ((TextSyndicationContent)item.Content).Text: string.Empty,
                    PublDate = item.PublishDate.DateTime,
                    Tags = new[] { "facebook" },
                    Source = StorySource.Facebook
                };
            }
        }
    }

    public enum StorySource
    {
        Blog, Twitter, Facebook
    }

    public class Story
    {
        public User User { get; set; }
        public string Url { get; set; }
        public DateTime PublDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public StorySource Source { get; set; }
        public string[] Tags { get; set; }

        public void InferTags()
        {
            if (StoryContains("ESFS") || StoryContains("euroval"))
                AddTag("euroval");
        }

        void AddTag(string tag)
        {
            Tags = Tags.Union(new[]{tag}).ToArray();
        }

        bool StoryContains(string s)
        {
            if (Title != null)
                return Title.ToUpper().Contains(s.ToUpper());
            
            if (Description != null)
                return Description.ToUpper().Contains(s.ToUpper());

            return false;
        }
    }

}