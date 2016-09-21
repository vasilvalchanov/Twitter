using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter.Web.Models.ViewModels
{
    using System.Linq.Expressions;

    using Twitter.Models;

    public class SimpleUserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Fullname { get; set; }

        public static Expression<Func<User, SimpleUserViewModel>> Create
        {
            get
            {
                return x => new SimpleUserViewModel()
                {
                    Id = x.Id,
                    Username = x.UserName,
                    Fullname = x.Fullname
                };
            }
        }
    }
}