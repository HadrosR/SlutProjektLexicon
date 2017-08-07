using System;
using System.Globalization;
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
            return PartialView(activityList);
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

            // Wrong id check
            if (course == null)
            {
                return HttpNotFound();
            }

            // Create a ModulesViewViewModel
            CourseViewModel vm = new CourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                StartDate = course.StartDate,
                EndDate = course.EndDate
            };

            //Sends the course name to a viewbag
            //ViewBag.course = course.Name;
            return View("ModuleStudent", vm);
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

            //Shows the name of the coruse for the student
            ViewBag.Course = course.Name;

            Calendar cal = new GregorianCalendar();

            // This should be "DateTime datevalue = DateTime.Now" but for presentation purpurses its hard coded :P
            DateTime datevalue = new DateTime(2017, 10, 09);
            DayOfWeek firstDay = DayOfWeek.Monday;
            CalendarWeekRule rule;
            rule = CalendarWeekRule.FirstFourDayWeek;

            //Selects all activities for the students cours
            var activity = course.Modules.SelectMany(l => l.Activities).ToList();

            List<ScheduleActivityViewModel> savm = new List<ScheduleActivityViewModel>();

            DateTime[] afk = new DateTime[activity.Count];

            //Adds them to an array
            int x = 0;
            foreach (var s in activity)
            {
                savm.Add(new ScheduleActivityViewModel()
                {
                    Name = s.Name,
                    Description = s.Description,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate
                    
                });

                afk[x] = s.StartDate;
                x++;
            }

            ScheduleViewModel schedule = new ScheduleViewModel();

            //Loops through the array of activities and puts them in seperate model list for there specific day
            for (int i = 0; i < savm.Count; i++)
            {
                DateTime activityDate = afk[i];
                //Gets what week it is "now"
                int weekNbr = cal.GetWeekOfYear(datevalue, rule, firstDay);
                //Gets the week of the activity
                int activityWeekNbr = cal.GetWeekOfYear(afk[i], rule, firstDay);
                ViewBag.Week = activityWeekNbr;
                if (activityWeekNbr == weekNbr)
                {
                    if (afk[i].DayOfWeek == DayOfWeek.Monday)
                    {
                        ViewBag.MondayDate = afk[i].Date.ToString("dd/MM");
                        schedule.Monday.Add(savm.ElementAt(i));
                    }
                    else if (afk[i].DayOfWeek == DayOfWeek.Tuesday)
                    {
                        ViewBag.TuedayDate = afk[i].Date.ToString("dd/MM");
                        schedule.Tuesday.Add(savm.ElementAt(i));
                    }
                    else if (afk[i].DayOfWeek == DayOfWeek.Wednesday)
                    {
                        ViewBag.WednesdayDate = afk[i].Date.ToString("dd/MM");
                        schedule.Wednesday.Add(savm.ElementAt(i));
                    }
                    else if (afk[i].DayOfWeek == DayOfWeek.Thursday)
                    {
                        ViewBag.ThursDate = afk[i].Date.ToString("dd/MM");
                        schedule.Thursday.Add(savm.ElementAt(i));
                    }
                    else if (afk[i].DayOfWeek == DayOfWeek.Friday)
                    {
                        ViewBag.FridayDate = afk[i].Date.ToString("dd/MM");
                        schedule.Friday.Add(savm.ElementAt(i));
                    }
                }
            }
            return PartialView("_Schedule", schedule);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        public ActionResult _StudentCourseModulesPartial(int? id)
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

            // Get the specifik course
            //var course = db.Courses.FirstOrDefault(n => n.Id == id);

            // Wrong id check
            if (course == null)
            {
                return HttpNotFound();
            }

            // Create a ModulesViewViewModel
            ModulesViewViewModel vm = new ModulesViewViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Modules = new List<ModulesViewModel>()
            };

            // Add viewmodels for every module
            foreach (var m in course.Modules)
            {
                List<ActivityViewModel> newActivityList = new List<ActivityViewModel>();
                vm.Modules.Add(new ModulesViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,

                });

            }
            return PartialView("_StudentCourseModulesPartial", vm);
        }

        public ActionResult _StudentCourseActivitiesPartial(int id)
        {
            // Get the specifik module
            var module = db.Modules.FirstOrDefault(n => n.Id == id);

            // List to store activities
            List<ActivityViewModel> newActivityList = new List<ActivityViewModel>();

            // Info about the module
            ModulesViewModel vm = new ModulesViewModel()
            {
                Id = id,
                Name = module.Name,
                Description = module.Description,
                StartDate = module.StartDate,
                EndDate = module.EndDate,
                Activities = newActivityList
            };

            //Add viewmodels for every activity in a module
            foreach (var t in module.Activities)
            {
                newActivityList.Add(new ActivityViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate
                });
            }

            return PartialView("_StudentCourseActivitiesPartial", vm);
        }
        
    }
}
