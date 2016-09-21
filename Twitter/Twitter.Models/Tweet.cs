using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Tweet
    {
        private ICollection<User> favoriteTweetUsers;
        private ICollection<Report> reports;
        private ICollection<Reply> replies;
        private ICollection<User> usersRetweeted;

        public Tweet()
        {
            this.favoriteTweetUsers = new HashSet<User>();
            this.reports = new HashSet<Report>();
            this.replies = new HashSet<Reply>();
            this.usersRetweeted = new HashSet<User>();
            this.CreatedOn = DateTime.Now;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool HasReported => this.reports.Count != 0;

        [Required]
        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public virtual ICollection<User> FavoriteTweetUsers
        {
            get
            {
                return this.favoriteTweetUsers;
            }

            set
            {
                this.favoriteTweetUsers = value;
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

        public virtual ICollection<User> UsersRetweeted
        {
            get
            {
                return this.usersRetweeted;
            }

            set
            {
                this.usersRetweeted = value;
            }
        }
    }
}
