using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter.Web.Controllers
{
    using System.Web.Mvc;

    using Twitter.Data;
    using Twitter.Data.Contracts;

    public abstract class BaseController : Controller
    {
       
        protected BaseController(ITwitterData data)
        {
            this.TwitterData = data;
        }

        protected BaseController() : this(new TwitterData(new TwitterContext()))
        {  
        }

        protected ITwitterData TwitterData { get; set; }
    }
}