namespace Twitter.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using Microsoft.AspNet.Identity.EntityFramework;

    using Twitter.Data.Contracts;
    using Twitter.Data.Migrations;
    using Twitter.Models;

    public class TwitterContext : IdentityDbContext<User>, ITwitterContext
    {
       
        public TwitterContext()
            : base("name=TwitterContext")
        {
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<TwitterContext, Configuration>());
        }

        public IDbSet<Tweet> Tweets { get; set; }

        public IDbSet<Report> Reports { get; set; }

        public IDbSet<Reply> Replies { get; set; }

        public IDbSet<Notification> Notifications { get; set; }

        public static TwitterContext Create()
        {
            return new TwitterContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.Tweets).WithRequired(t => t.Author).WillCascadeOnDelete(false);
            modelBuilder.Entity<User>().HasMany(u => u.ReTweets).WithMany(t => t.UsersRetweeted).Map(
                x =>
                    {
                        x.MapLeftKey("UserId");
                        x.MapRightKey("RetweetedTweetId");
                        x.ToTable("UsersRetweetedTweets");
                    });

            modelBuilder.Entity<User>().HasMany(u => u.FavoriteTweets).WithMany(t => t.FavoriteTweetUsers).Map(
                x =>
                    {
                        x.MapLeftKey("UserId");
                        x.MapRightKey("FavoriteTweetId");
                        x.ToTable("UsersFavoriteTweets");
                    });

            modelBuilder.Entity<User>().HasMany(u => u.Followers).WithMany().Map(
                x =>
                    {
                        x.MapLeftKey("UserId");
                        x.MapRightKey("FollowerId");
                        x.ToTable("UsersFollowers");
                    });

            modelBuilder.Entity<User>().HasMany(u => u.FollowingUsers).WithMany().Map(
                x =>
                    {
                        x.MapLeftKey("UserId");
                        x.MapRightKey("FollowingUserId");
                        x.ToTable("UsersFollowingUsers");
                    });

            base.OnModelCreating(modelBuilder);
        }
    }

}