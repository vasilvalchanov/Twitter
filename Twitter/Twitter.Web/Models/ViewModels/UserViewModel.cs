using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter.Web.Models.ViewModels
{
    using System.Linq.Expressions;

    using Twitter.Models;

    public class UserViewModel : SimpleUserViewModel
    {
        public IEnumerable<SimpleUserViewModel> Followers { get; set; }

        public IEnumerable<SimpleUserViewModel> FollowingUsers { get; set; }

        public IEnumerable<TweetViewModel> Tweets { get; set; }

        public IEnumerable<TweetViewModel> ReTweets { get; set; }

        public IEnumerable<TweetViewModel> FavoriteTweets { get; set; }

        public IEnumerable<ReplyViewModel> Replies { get; set; }

        public IEnumerable<ReportViewModel> Reports { get; set; }

        public IEnumerable<NotificationViewModel> Notifications { get; set; }

        public static Expression<Func<User, UserViewModel>> Create
        {
            get
            {
                return x => new UserViewModel()
                {
                   Id = x.Id,
                   Username = x.UserName,
                   Fullname = x.Fullname,
                   Followers = x.Followers.AsQueryable().Select(SimpleUserViewModel.Create),
                    FollowingUsers = x.FollowingUsers.AsQueryable().Select(SimpleUserViewModel.Create),
                    Tweets = x.Tweets.AsQueryable().Select(TweetViewModel.Create),
                    ReTweets = x.ReTweets.AsQueryable().Select(TweetViewModel.Create),
                    FavoriteTweets = x.FavoriteTweets.AsQueryable().Select(TweetViewModel.Create),
                    Replies = x.Replies.AsQueryable().Select(ReplyViewModel.Create),
                    Reports = x.Reports.AsQueryable().Select(ReportViewModel.Create),
                    Notifications = x.Notifications.AsQueryable().Select(NotificationViewModel.Create),
                };
                
            }
        }
    }
}