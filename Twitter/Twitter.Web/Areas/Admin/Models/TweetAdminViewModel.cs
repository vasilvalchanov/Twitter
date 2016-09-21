using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter.Web.Areas.Admin.Models
{
    using System.Linq.Expressions;

    using Twitter.Models;
    using Twitter.Web.Models.ViewModels;

    public class TweetAdminViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Author { get; set; }

        public static Expression<Func<Tweet, TweetAdminViewModel>> Create
        {
            get
            {
                return x => new TweetAdminViewModel()
                {
                    Id = x.Id,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn,
                    Author = x.Author.UserName    
                };
            }
        }

    }
}