using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Data.Contracts
{
    using System.Data.Entity;

    using Twitter.Models;

    public interface ITwitterContext
    {
        IDbSet<Tweet> Tweets { get; set; }

        IDbSet<Report> Reports { get; set; }

        IDbSet<Reply> Replies { get; set; }

        IDbSet<Notification> Notifications { get; set; }

        int SaveChanges();
    }
}
