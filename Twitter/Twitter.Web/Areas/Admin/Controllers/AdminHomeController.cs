using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Twitter.Web.Areas.Admin.Controllers
{
    using Twitter.Data;
    using Twitter.Web.Controllers;

    public class AdminHomeController : AdminController
    {

        public AdminHomeController(TwitterData data) : base(data)
        {
        }

        public AdminHomeController() : base(new TwitterData(new TwitterContext()))
        {
        }

        // GET: Admin/AdminHome
        public ActionResult AdminIndex()
        {
            return View();
        }
    }
}