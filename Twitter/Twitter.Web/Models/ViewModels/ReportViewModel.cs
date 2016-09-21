using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter.Web.Models.ViewModels
{
    using System.Linq.Expressions;

    using Twitter.Models;

    public class ReportViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public SimpleTweetViewModel Tweet { get; set; }

        public SimpleUserViewModel Author { get; set; }

        public static Expression<Func<Report, ReportViewModel>> Create
        {
            get
            {
                return x => new ReportViewModel()
                                {
                                    Id = x.Id,
                                    Content = x.Content,
                                    CreatedOn = x.CreatedOn,
                                    Tweet = new SimpleTweetViewModel()
                                    {
                                        Id = x.TweetId,
                                        Content = x.Tweet.Content,
                                        CreatedOn = x.Tweet.CreatedOn,
                                        Author = new SimpleUserViewModel()
                                        {
                                            Id = x.Tweet.AuthorId,
                                            Username = x.Tweet.Author.UserName,
                                            Fullname = x.Tweet.Author.Fullname
                                        }
                                    },
                                    Author = new SimpleUserViewModel()
                                    {
                                        Id = x.UserId,
                                        Username = x.User.UserName,
                                        Fullname = x.User.Fullname
                                    }
                };
            }
        }
    }
}