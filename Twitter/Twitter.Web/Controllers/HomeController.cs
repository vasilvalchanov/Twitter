using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Twitter.Web.Controllers
{
    using System.Data.Entity;

    using Microsoft.AspNet.Identity;

    using Ninject;

    using Twitter.Data;
    using Twitter.Data.Contracts;
    using Twitter.Web.Models.ViewModels;

    public class HomeController : BaseController
    {
        public HomeController(ITwitterData data)
           : base(data)
        {
        }

        public HomeController() : base(new TwitterData(new TwitterContext()))
        {
            
        }

        public ActionResult Index()
        {
            IQueryable<TweetViewModel> tweets;

            if (!this.User.Identity.IsAuthenticated)
            {
                tweets =
               this.TwitterData.Tweets.All()
                   .OrderByDescending(t => t.CreatedOn)
                   .Include(t => t.FavoriteTweetUsers)
                   .Include(t => t.UsersRetweeted)
                   .Include(t => t.Replies)
                   .Include(t => t.Reports)
                   .Select(TweetViewModel.Create);
            }
            else
            {
                var currentUserId = this.User.Identity.GetUserId();
                var currentUser = this.TwitterData.Users.Find(currentUserId);

                if (currentUser == null)
                {
                    return this.HttpNotFound();
                }

                tweets = currentUser.FollowingUsers
                    .SelectMany(f => f.Tweets)
                    .OrderByDescending(t => t.CreatedOn)
                    .AsQueryable().Select(TweetViewModel.Create);

                this.ViewBag.Notifications = currentUser.Notifications.Count(n => n.IsRead == false);
            }

            return View(tweets.ToList());
        }
    }
}