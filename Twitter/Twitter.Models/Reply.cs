using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Reply
    {
        public Reply()
        {
            this.CreatedOn = DateTime.Now;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public int TweetId { get; set; }

        public virtual Tweet Tweet { get; set; }
    }
}
