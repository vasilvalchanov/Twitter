using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter.Web.Models.ViewModels
{
    using System.Linq.Expressions;

    using Twitter.Models;

    public class NotificationViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public bool IsRead { get; set; }

        public SimpleUserViewModel User { get; set; }

        public static Expression<Func<Notification, NotificationViewModel>> Create
        {
            get
            {
                return x => new NotificationViewModel()
                                {
                                    Id = x.Id,
                                    Content = x.Content,
                                    Date = x.Date,
                                    IsRead = x.IsRead,
                                    User = new SimpleUserViewModel()
                                               {
                                                   Id = x.User.Id,
                                                   Username = x.User.UserName,
                                                   Fullname = x.User.Fullname
                                               }
                                };
            }
        }
    }
}