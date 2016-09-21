using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Twitter.Web.Controllers
{
    using Microsoft.AspNet.Identity;

    using Twitter.Data;
    using Twitter.Data.Contracts;
    using Twitter.Web.Models.ViewModels;

    [Authorize]
    public class NotificationsController : BaseController
    {
        public NotificationsController(ITwitterData data)
           : base(data)
        {
        }

        public NotificationsController() : base(new TwitterData(new TwitterContext()))
        {

        }

        [HttpGet]
        public ActionResult MyNotifications(string username)
        {
            var currentUser = this.TwitterData.Users.All().FirstOrDefault(u => u.UserName == username);

            if (currentUser == null)
            {
                return this.HttpNotFound();
            }

            var notifications = currentUser.Notifications.AsQueryable().Select(NotificationViewModel.Create).OrderByDescending(n => n.Date);

            this.ViewBag.Notifications = currentUser.Notifications.Count(n => n.IsRead == false);

            return View(notifications);
        }

        public ActionResult ChangeNotificationStatus(int id)
        {
            var notification = this.TwitterData.Notifications.Find(id);

            if (notification == null)
            {
                return this.HttpNotFound();
            }

            if (notification.IsRead)
            {
                notification.IsRead = false;
                this.TwitterData.SaveChanges();
                return this.Content("No");
            }
            else
            {
                notification.IsRead = true;
                this.TwitterData.SaveChanges();
                return this.Content("Yes");
            }
        }
    }
}