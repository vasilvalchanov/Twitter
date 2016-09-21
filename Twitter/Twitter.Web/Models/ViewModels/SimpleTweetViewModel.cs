using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter.Web.Models.ViewModels
{
    using System.Linq.Expressions;

    using Twitter.Models;

    public class SimpleTweetViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public SimpleUserViewModel Author { get; set; }

        public static Expression<Func<Tweet, SimpleTweetViewModel>> Create
        {
            get
            {
                return x => new SimpleTweetViewModel()
                {
                    Id = x.Id,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn,
                    Author = new SimpleUserViewModel()
                    {
                        Id = x.AuthorId,
                        Username = x.Author.UserName,
                        Fullname = x.Author.Fullname
                    }
                };
            }
        }
    }
}