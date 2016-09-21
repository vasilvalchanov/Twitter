using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Data.Contracts
{
    using Twitter.Models;

    public interface ITwitterData
    {
        IRepository<Tweet> Tweets { get; }

        IRepository<User> Users { get; }

        IRepository<Report> Reports { get; }

        IRepository<Reply> Replies { get; }

        IRepository<Notification> Notifications { get; }

        int SaveChanges();
    }
}
