using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter.Web.Models.ViewModels
{
    using System.Linq.Expressions;

    using Twitter.Models;

    public class TweetViewModel : SimpleTweetViewModel
    {

        public IEnumerable<SimpleUserViewModel> UsersFavorites { get; set; }

        public IEnumerable<SimpleUserViewModel> UsersRetweeted { get; set; }

        public IEnumerable<ReplyViewModel> Replies { get; set; }

        public IEnumerable<ReportViewModel> Reports { get; set; }

        public static Expression<Func<Tweet, TweetViewModel>> Create
        {
            get
            {
                return x => new TweetViewModel()
                {
                    Id = x.Id,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn,
                    Author = new SimpleUserViewModel()
                    {
                        Id = x.AuthorId,
                        Username = x.Author.UserName,
                        Fullname = x.Author.Fullname
                    },
                    UsersFavorites = x.FavoriteTweetUsers.AsQueryable().Select(SimpleUserViewModel.Create),
                    UsersRetweeted = x.UsersRetweeted.AsQueryable().Select(SimpleUserViewModel.Create),
                    Replies = x.Replies.AsQueryable().Select(ReplyViewModel.Create),
                    Reports = x.Reports.AsQueryable().Select(ReportViewModel.Create)
                };
            }
        }
    }
}