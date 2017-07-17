using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using LexiconLMSPortal.Models.Classes;
using LexiconLMSPortal.Models.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using LexiconLMSPortal.Models.ViewModels;

namespace LexiconLMSPortal.Controllers
{
    [Authorize (Roles ="Student")]
    public class StudentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Student
        public ActionResult Index()
        {
            return View(db.Modules.ToList());
        }
        // GET: Student modul view 
        public ActionResult ModuleStudent()
        {
            //Gets the users name
            string student = User.Identity.Name;
            UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(db);
            UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);

            //Finds the user in the database
            ApplicationUser currentUser = userManager.FindByName(student);

            //Finds the course by the students courseID
            int CourseID = currentUser.CourseId.Id;

            //Finds the right course with ID
            var course = db.Courses.FirstOrDefault(c => c.Id == CourseID);

            //Sends the course name to a viewbag
            ViewBag.Course = course.Name;
            return View();
        }
        //GET ModulList for students
        public ActionResult _StudentModulsPartial()
        {
            //Gets the users name
            string student = User.Identity.Name;
            UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(db);
            UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);

            //Finds the user in the database
            ApplicationUser currentUser = userManager.FindByName(student);

            //Finds the course by the students courseID
            int CourseID = currentUser.CourseId.Id;

            //Finds the right course with ID
            var course = db.Courses.FirstOrDefault(c => c.Id == CourseID);

            //Creates a new list to store all the moduls
            List<_StudentModuleList> aktivmoduls = new List<_StudentModuleList>();

            if (course == null)
            {
                return HttpNotFound();
            }
            //Populate the new list of moduls
            foreach (var m in course.Modules)
            {
                aktivmoduls.Add(new _StudentModuleList
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate
                });
            }
            return PartialView("_StudentModulepartial",aktivmoduls);
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
