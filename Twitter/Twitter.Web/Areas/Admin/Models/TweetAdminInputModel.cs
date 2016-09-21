using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter.Web.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;

    using Twitter.Models;

    public class TweetAdminInputModel
    {
        [Required]
        [MinLength(1, ErrorMessage = "The {0} length must be at least {1} symbols")]
        public string Content { get; set; }

    }
}