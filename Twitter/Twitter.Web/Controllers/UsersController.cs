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
    public class UsersController : BaseController
    {
        public UsersController(ITwitterData data)
            : base(data)
        {
        }

        public UsersController() : base(new TwitterData(new TwitterContext()))
        {

        }

        // GET: Users
        public ActionResult PublicProfile(string username)
        {
            var user =
                this.TwitterData.Users.All()
                    .Include(u => u.Tweets)
                    .Include(u => u.FavoriteTweets)
                    .Include(u => u.Followers)
                    .Include(u => u.FollowingUsers)
                    .Where(u => u.UserName == username)
                    .Select(UserViewModel.Create)
                    .FirstOrDefault();

            if (user == null)
            {
                return this.HttpNotFound();
            }

            this.ViewBag.Notifications = user.Notifications.Count(n => n.IsRead == false);

            return View(user);
        }

        public ActionResult ShowFollowers(string username)
        {
            var user =
                this.TwitterData.Users
                    .All()
                    .Include(u => u.Followers)
                    .FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return this.HttpNotFound();
            }

            var followers = user.Followers.AsQueryable().Select(u => u.UserName);

            return this.PartialView("_ShowFollowers", followers);
        }

        [HttpGet]
        public ActionResult ShowFollowingUsers(string username)
        {
            var user =
                this.TwitterData.Users
                    .All()
                    .Include(u => u.FollowingUsers)
                    .FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return this.HttpNotFound();
            }

            var followingUsers = user.FollowingUsers.AsQueryable().Select(u => u.UserName);

            return this.PartialView("_ShowFollowingUsers", followingUsers);
        }

        public ActionResult FollowMe(string username)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var currentUser = this.TwitterData.Users.All().Include(u => u.FollowingUsers).FirstOrDefault(u => u.Id == currentUserId);

            if (currentUser == null)
            {
                return this.HttpNotFound();
            }

            var userToFollow =
                this.TwitterData.Users.All().Include(u => u.Followers).FirstOrDefault(u => u.UserName == username);

            if (userToFollow == null)
            {
                return this.HttpNotFound();
            }

            if (!currentUser.FollowingUsers.Any(u => u.UserName == username))
            {
                currentUser.FollowingUsers.Add(userToFollow);
                userToFollow.Followers.Add(currentUser);
                var notification = new Notification()
                {
                    Content = string.Format("{0} follows you", currentUser.UserName),
                    Date = DateTime.Now,
                    IsRead = false,
                    UserId = userToFollow.Id
                };
                userToFollow.Notifications.Add(notification);
                this.TwitterData.Notifications.Add(notification);

                this.TwitterData.SaveChanges();
                this.AddNotification("Successfully added to Following Users", NotificationType.INFO);
            }
            else
            {
                this.AddNotification("User is already followed", NotificationType.ERROR);
            }

            return this.RedirectToAction("Index", "Home");
        }

        public ActionResult Search(string query)
        {
            var searchedUsers = this.TwitterData.Users.All()
                .Where(u => u.UserName.Contains(query) || u.Email.Contains(query))
                .OrderBy(u => u.UserName)
                .Select(SimpleUserViewModel.Create);

            return this.View(searchedUsers);

        }
    }
}


