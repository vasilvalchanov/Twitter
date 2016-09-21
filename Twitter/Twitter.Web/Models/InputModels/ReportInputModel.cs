using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter.Web.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class ReportInputModel
    {
        [Required]
        [MinLength(1, ErrorMessage = "The content is required and must be at least 1 character long")]
        public string Content { get; set; }
    }
}