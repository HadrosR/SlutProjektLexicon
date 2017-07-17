using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LexiconLMSPortal.Models.Classes;
using LexiconLMSPortal.Models.Identity;
using LexiconLMSPortal.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace LexiconLMSPortal.Controllers
{
    public class StudentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult _StudentListPartial()
        {
            UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(db);
            UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);
            var courseID = userManager.FindByName(User.Identity.Name).CourseId.Id;
            List<_StudentListPartial> sl = new List<_StudentListPartial>();
            var students = db.Courses.FirstOrDefault(t => t.Id == courseID).Students;
            
            foreach (var s in students)
            {
                sl.Add(new _StudentListPartial
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    EMail = s.Email,
                    Id = s.Id

                });
            }
            return View(sl);
        }

        public ActionResult _ActivityListPartial(int id)
        {
            UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(db);
            UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);
            var courseID = userManager.FindByName(User.Identity.Name).CourseId.Id;
            List<ActivityViewModel> activityList = new List<ActivityViewModel>();
            
            var module = db.Courses.FirstOrDefault(t => t.Id == courseID).Modules.FirstOrDefault(m => m.Id == id);

            foreach (var m in module.Activities)
            {
                activityList.Add(new ActivityViewModel
                {
                    Name = m.Name,
                    StartDate = m.StartDate
                });
            }
            return View(activityList);
        }

        // GET: Student
        public ActionResult Index()
        {
            return View("");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
