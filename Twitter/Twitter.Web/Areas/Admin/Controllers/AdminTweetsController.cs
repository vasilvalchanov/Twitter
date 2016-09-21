using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Twitter.Web.Areas.Admin.Controllers
{
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using Microsoft.AspNet.Identity;

    using Twitter.Data;
    using Twitter.Models;
    using Twitter.Web.Areas.Admin.Models;
    using Twitter.Web.Controllers;

    public class AdminTweetsController : AdminController
    {
        public AdminTweetsController(TwitterData data) : base(data)
        {
        }

        public AdminTweetsController() : base(new TwitterData(new TwitterContext()))
        {
        }

        // GET: Admin/Tweets
        public ActionResult TweetsIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var tweets = this.TwitterData.Tweets.All().Select(TweetAdminViewModel.Create);

            return this.Json(tweets.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, TweetAdminViewModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var tweet = new Tweet()
                {
                    Content = model.Content,
                    AuthorId = this.User.Identity.GetUserId()
                };

                this.TwitterData.Tweets.Add(tweet);
                this.TwitterData.SaveChanges();
            }

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, TweetAdminViewModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var tweet = this.TwitterData.Tweets.Find(model.Id);

                var newAuthor = this.TwitterData.Users.All().FirstOrDefault(u => u.UserName == model.Author);


                tweet.Id = model.Id;
                tweet.Content = model.Content;
                tweet.CreatedOn = model.CreatedOn;
                if (newAuthor == null)
                {
                    return this.HttpNotFound();
                }

                tweet.Author = newAuthor;
                this.TwitterData.SaveChanges();
            }

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, TweetAdminViewModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var tweetToDelete = this.TwitterData.Tweets.Find(model.Id);

                if (tweetToDelete == null)
                {
                    return this.HttpNotFound();
                }

                this.TwitterData.Tweets.Delete(tweetToDelete);
                this.TwitterData.SaveChanges();
            }


            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }
    }
}