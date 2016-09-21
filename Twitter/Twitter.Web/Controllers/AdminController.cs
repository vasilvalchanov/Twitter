using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Twitter.Web.Controllers
{
    using Twitter.Data;
    using Twitter.Data.Contracts;

    [Authorize(Roles = "Administrator")]
    public abstract class AdminController : BaseController
    {

        protected AdminController(ITwitterData data) : base(data)
        {    
        }

        protected AdminController() : base(new TwitterData(new TwitterContext()))
        { 
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}