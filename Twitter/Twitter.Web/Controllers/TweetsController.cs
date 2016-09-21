using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Twitter.Web.Controllers
{
    using System.Data.Entity;

    using Microsoft.AspNet.Identity;

    using Twitter.Data;
    using Twitter.Data.Contracts;
    using Twitter.Models;
    using Twitter.Web.Extensions;
    using Twitter.Web.Models.InputModels;
    using Twitter.Web.Models.ViewModels;

    [Authorize]
    public class TweetsController : BaseController
    {
        public TweetsController(ITwitterData data)
            : base(data)
        {
        }

        public TweetsController()
            : base(new TwitterData(new TwitterContext()))
        {

        }

        [HttpGet]
        public ActionResult CreateTweet()
        {
            return this.View("_CreateTweetPartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTweet(CreateTweetInputModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var tweet = new Tweet() { Content = model.Content, AuthorId = this.User.Identity.GetUserId(), };

                this.TwitterData.Tweets.Add(tweet);
                this.TwitterData.SaveChanges();
                this.AddNotification("Tweet created successfully", NotificationType.SUCCESS);

                return this.RedirectToAction("Index", "Home");
            }

            return this.View("_CreateTweetPartial", model);
        }

        [HttpGet]
        public ActionResult MyTweets(string username)
        {
            var user =
                this.TwitterData.Users.All()
                    .Include(u => u.Tweets)
                    .Include(u => u.FavoriteTweets)
                    .Include(u => u.Followers)
                    .Include(u => u.FollowingUsers)
                    .FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return this.HttpNotFound();
            }

            var tweets = user.Tweets.AsQueryable().Select(TweetViewModel.Create);

            this.ViewBag.Notifications = user.Notifications.Count(n => n.IsRead == false);

            return this.View(tweets);
        }

        [HttpGet]
        public ActionResult ShowFavorites(string username)
        {
            var user =
                this.TwitterData.Users.All()
                    .Include(u => u.Tweets)
                    .Include(u => u.FavoriteTweets)
                    .Include(u => u.Followers)
                    .Include(u => u.FollowingUsers)
                    .FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return this.HttpNotFound();
            }

            var tweets = user.FavoriteTweets.AsQueryable().Select(TweetViewModel.Create);

            return this.View(tweets);
        }

        public ActionResult AddToFavorites(int id)
        {
            var tweet =
                this.TwitterData.Tweets.All()
                    .Include(t => t.Author)
                    .Include(t => t.FavoriteTweetUsers)
                    .Include(t => t.Author.Notifications)
                    .FirstOrDefault(t => t.Id == id);

            var currentUserId = this.User.Identity.GetUserId();
            var user = this.TwitterData.Users.Find(currentUserId);

            if (user == null)
            {
                return this.HttpNotFound();
            }



            if (tweet == null)
            {
                return this.HttpNotFound();
            }

            var hasAlreadyAdded = user.FavoriteTweets.Any(t => t.Id == id);

            if (!hasAlreadyAdded)
            {
                tweet.FavoriteTweetUsers.Add(user);
                user.FavoriteTweets.Add(tweet);
                var notification = new Notification()
                {
                    Content = string.Format("{0} added your Tweet#{1} to favorites", user.UserName, tweet.Id),
                    Date = DateTime.Now,
                    IsRead = false,
                    UserId = user.Id
                };
                tweet.Author.Notifications.Add(notification);
                this.TwitterData.Notifications.Add(notification);
                this.TwitterData.SaveChanges();
                
            }

            return this.Content(user.FavoriteTweets.Count.ToString());
        }

        [HttpGet]
        public ActionResult Reply()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reply(int id, ReplyTweetInputModel model)
        {
            var tweet = this.TwitterData.Tweets.All().Include(t => t.Author).FirstOrDefault(t => t.Id == id);

            if (tweet == null)
            {
                return this.HttpNotFound();
            }

            if (model != null && this.ModelState.IsValid)
            {
                var currentUserId = this.User.Identity.GetUserId();

                var reply = new Reply()
                {
                    Content = model.Content,
                    TweetId = tweet.Id,
                    UserId = currentUserId
                };

                tweet.Replies.Add(reply);
                this.TwitterData.Replies.Add(reply);
                this.TwitterData.SaveChanges();
                this.AddNotification("Replied successfully", NotificationType.SUCCESS);

                return this.RedirectToAction("Index", "Home");
            }

            return this.View("Reply", model);
        }

        [HttpGet]
        public ActionResult Report()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report(int id, ReportInputModel model)
        {
            var tweet = this.TwitterData.Tweets.All().Include(t => t.Author).FirstOrDefault(t => t.Id == id);

            if (tweet == null)
            {
                return this.HttpNotFound();
            }

            if (model != null && this.ModelState.IsValid)
            {
                var currentUserId = this.User.Identity.GetUserId();

                var hasAlreadyReported = tweet.Reports.Any(r => r.UserId == currentUserId);

                if (!hasAlreadyReported)
                {
                    var report = new Report()
                    {
                        Content = model.Content,
                        TweetId = tweet.Id,
                        UserId = currentUserId
                    };

                    tweet.Reports.Add(report);
                    this.TwitterData.Reports.Add(report);
                    this.TwitterData.SaveChanges();
                    this.AddNotification("The Tweet was reported successfully", NotificationType.SUCCESS);
                }
                else
                {
                    this.AddNotification("The Tweet has been already reported", NotificationType.ERROR);
                }
               

                return this.RedirectToAction("Index", "Home");
            }

            return this.View("Report", model);
        }

        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var tweet = this.TwitterData.Tweets.All()
                .Where(t => t.Id == id)
                .Select(TweetDetailsViewModel.Create).FirstOrDefault();

            if (tweet == null)
            {
                return this.HttpNotFound();
            }

            return this.PartialView("_TweetDetails", tweet);
        }


        public ActionResult Delete(int id)
        {
            var tweet = this.TwitterData.Tweets.Find(id);

            if (tweet == null)
            {
                return this.HttpNotFound();
            }

            if (tweet.AuthorId == this.User.Identity.GetUserId())
            {
                this.TwitterData.Tweets.Delete(tweet);
                this.TwitterData.SaveChanges();

                this.AddNotification(string.Format("Tweet #{0} was deleted successfully", id), NotificationType.SUCCESS);

                return this.RedirectToAction("Index", "Home");
            }

            return null;
        }

        public ActionResult Confirm(int id)
        {
            var tweet = this.TwitterData.Tweets.All()
               .Where(t => t.Id == id)
               .Select(TweetViewModel.Create).FirstOrDefault();

            if (tweet == null)
            {
                return this.HttpNotFound();
            }

            return this.PartialView("_ConfirmDeletePartial", tweet);
        }
    }
}
