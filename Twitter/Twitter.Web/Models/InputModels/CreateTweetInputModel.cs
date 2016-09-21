using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter.Web.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class CreateTweetInputModel
    {
        [Required]
        [MinLength(1, ErrorMessage = "The {0} length must be at least {1} symbols")]
        public string Content { get; set; }
    }
}