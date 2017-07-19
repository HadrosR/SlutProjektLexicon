using LexiconLMSPortal.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LexiconLMSPortal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(User.IsInRole("Teacher"))
            {
                return RedirectToRoute(new { controller = "Teacher", action = "Index" });
            }

           else if (User.IsInRole("Student"))
            {
                return RedirectToRoute(new { controller = "Student", action = "Index" });
            }

            return RedirectToRoute(new { controller = "Account", action = "LogIn" });
        }

    }
}