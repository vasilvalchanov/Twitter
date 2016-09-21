using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Remoting;
    using System.Security.Claims;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<User> followers;
        private ICollection<User> followingUsers;
        private ICollection<Tweet> tweets;
        private ICollection<Notification> notifications;
        private ICollection<Report> reports;
        private ICollection<Tweet> favoriteTweets;
        private ICollection<Reply> replies;
        private ICollection<Tweet> reTweets;

        public User()
        {
            this.followers = new HashSet<User>();
            this.followingUsers = new HashSet<User>();
            this.tweets = new HashSet<Tweet>();
            this.notifications = new HashSet<Notification>();
            this.reports = new HashSet<Report>();
            this.favoriteTweets = new HashSet<Tweet>();
            this.replies = new HashSet<Reply>();
            this.reTweets = new HashSet<Tweet>();
        }

        [Required]
        public string Fullname { get; set; }

        public virtual ICollection<User> Followers
        {
            get
            {
                return this.followers;
            }

            set
            {
                this.followers = value;
            }
        }

        public virtual ICollection<User> FollowingUsers
        {
            get
            {
                return this.followingUsers;
            }

            set
            {
                this.followingUsers = value;
            }
        }

        public virtual ICollection<Tweet> Tweets
        {
            get
            {
                return this.tweets;
            }

            set
            {
                this.tweets = value;
            }
        }

        public virtual ICollection<Notification> Notifications
        {
            get
            {
                return this.notifications;
            }

            set
            {
                this.notifications = value;
            }
        }

        public virtual ICollection<Report> Reports
        {
            get
            {
                return this.reports;
            }

            set
            {
                this.reports = value;
            }
        }

        public virtual ICollection<Tweet> FavoriteTweets
        {
            get
            {
                return this.favoriteTweets;
            }

            set
            {
                this.favoriteTweets = value;
            }
        }

        public virtual ICollection<Reply> Replies
        {
            get
            {
                return this.replies;
            }

            set
            {
                this.replies = value;
            }
        }

        public virtual ICollection<Tweet> ReTweets
        {
            get
            {
                return this.reTweets;
            }

            set
            {
                this.reTweets = value;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

    }
}
