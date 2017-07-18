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
    [Authorize(Roles = "Student")]
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
            ModulesViewModel activityList = new ModulesViewModel()
            {
                Activities = new List<ActivityViewModel>()
            };

            var act = db.Modules.FirstOrDefault(m => m.Id == id).Activities;

            foreach (var m in act)
            {
                activityList.Activities.Add(new ActivityViewModel
                {
                    Name = m.Name,
                    StartDate = m.StartDate,
                    Description = m.Description,
                    EndDate = m.EndDate
                });
            }
            return View(activityList);
        }

        // GET: Student Index
        public ActionResult Index()
        {
            return View();
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
            return PartialView("_StudentModulepartial", aktivmoduls);
        }

        //Returns the Scedule in a partialview
        public ActionResult Schedule()
        {
            string student = User.Identity.Name;
            UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(db);
            UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);
            ApplicationUser currentUser = userManager.FindByName(student);

            int CourseID = currentUser.CourseId.Id;
            var course = db.Courses.FirstOrDefault(c => c.Id == CourseID);

            if (course == null)
            {
                return HttpNotFound();
            }

            ModulesViewViewModel vm = new ModulesViewViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Modules = new List<ModulesViewModel>()
            };

            ViewBag.Course = course.Name;
            foreach (var m in course.Modules)
            {
                List<ActivityViewModel> newActivityList = new List<ActivityViewModel>();
                foreach (var a in m.Activities)
                {
                    newActivityList.Add(new ActivityViewModel
                    {
                        Name = a.Name,
                        StartDate = a.StartDate,
                        Description = a.Description,
                        EndDate = a.EndDate
                    });
                }
                vm.Modules.Add(new ModulesViewModel
                {
                    Name = m.Name,
                    Description = m.Description,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,

                    Activities = newActivityList
                });
            }

           
            return PartialView("_Schedule", vm);
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
