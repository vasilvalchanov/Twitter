using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter.Web.Models.ViewModels
{
    using System.Linq.Expressions;

    using Twitter.Models;

    public class TweetDetailsViewModel
    {
        public int Id { get; set; }

        public IEnumerable<ReplyViewModel> Replies { get; set; }

        public IEnumerable<ReportViewModel> Reports { get; set; }

        public static Expression<Func<Tweet, TweetDetailsViewModel>> Create
        {
            get
            {
                return x => new TweetDetailsViewModel()
                {
                    Id = x.Id,
                    Replies = x.Replies.AsQueryable().Select(ReplyViewModel.Create),
                    Reports = x.Reports.AsQueryable().Select(ReportViewModel.Create)
                };
            }
        }
    }
}