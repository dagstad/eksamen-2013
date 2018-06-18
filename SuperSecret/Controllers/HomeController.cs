using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SuperSecret.DAL;
using SuperSecret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperSecret.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            // return View("~/Views/Account/Login.cshtml");
            return View();
        }


    }
}