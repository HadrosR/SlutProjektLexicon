using LexiconLMSPortal.Models.Identity;
using LexiconLMSPortal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using LexiconLMSPortal.Models.Classes;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;

namespace LexiconLMSPortal.Controllers
{
    public class TeacherController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        //GET: AddTeacher
        [Authorize(Roles = "Teacher")]
        public ActionResult AddTeacher()
        {
            return View();
        }
        
        //POST: AddTeacher //UNFINISHED!
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public void AddTeacher(_TeacherListPartialModel arg)
        {
            if ( arg != null )
            {
                if ( !arg.FirstName.Equals("") && !arg.LastName.Equals(""))
                {
                    //Add contents of arg to a new post in the database
                    UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(context);
                    UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);
                    Models.Identity.ApplicationUser user = new Models.Identity.ApplicationUser { UserName = arg.EMail, Email = arg.EMail, FirstName = arg.FirstName, LastName = arg.LastName };
                    var result = userManager.Create(user, "victor");
                    if (result.Succeeded)
                    {
                        context.Users.Add(user);
                    }
                }
            }
        }

        //GET: DeleteTeacher
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteTeacher(_TeacherListPartialModel arg)
        {
            return View(arg);
        }

        //GET: EditTeacher
        [Authorize(Roles = "Teacher")]
        public ActionResult EditTeacher(_TeacherListPartialModel arg)
        {
            return View(arg);
        }
        
        // GET: Teacher
        [Authorize(Roles ="Teacher")]
        public ActionResult Index()
        {
            
            var courses = context.Courses;
            //skapar en ny lista
            List<CourseViewModel> aktivCourses = new List<CourseViewModel>();
            //fyller den nya listan med alla curser från CourseViewModel
            if (courses == null)
            {
                return HttpNotFound();
            }
            foreach (var c in courses)
            {
                aktivCourses.Add(new CourseViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate

                });
            }
            return View(aktivCourses);
        }
        //Get: Course Create
        [Authorize(Roles ="Teacher")]
        public ActionResult CreateCourse()
        {
            return View();
        }
        //Post: Course Create
        [HttpPost]
        [Authorize(Roles ="Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCourse([Bind(Include = "Id,Name,Description,StartDate,EndDate")] CourseModels course)
        {
            if (ModelState.IsValid)
            {

                context.Courses.Add(course);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }
        //GET: Course Edit
        [Authorize(Roles ="Teacher")]
        public ActionResult EditCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseModels course = context.Courses.Find(id);
            if (course==null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        //Post: Course Edit
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse([Bind(Include = "Id,Name,Description,StartDate,EndDate")] CourseModels course)
        {
            if (ModelState.IsValid)
            {
                context.Entry(course).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }
        //GET: Course delete
        [Authorize(Roles ="Teacher")]
        public ActionResult DeleteCourse(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseModels course = context.Courses.Find(id);
            if (course==null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        //Post: Course Delete
        [HttpPost, ActionName("DeleteCourse")]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseModels course = context.Courses.Find(id);
            context.Courses.Remove(course);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Teacher/Courses/id
        [Authorize(Roles = "Teacher")]
        public ActionResult Courses(int? id)
        {
            // Get the specifik course
            var course = context.Courses.FirstOrDefault(n => n.Id == id);

            // Wrong id check
            if(course == null)
            {
                return HttpNotFound();
            }

            // Create a ModulesViewViewModel
            ModulesViewViewModel vm = new ModulesViewViewModel
            {
                Name = course.Name,
                Description = course.Description,
                Modules = new List<ModulesViewModel>()
            };

            // Add viewmodels for every module
            foreach(var m in course.Modules)
            {
                List<ActivityViewModel> newActivityList = new List<ActivityViewModel>();
                vm.Modules.Add(new ModulesViewModel
                {
                    Name = m.Name,
                    Description = m.Description,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,

                    /* Activities here */
                    Activities = newActivityList
                });

                //Add viewmodels for every activity in a module
                foreach ( var t in m.Activities )
                {
                    newActivityList.Add(new ActivityViewModel
                    {
                        Name = t.Name,
                        Description = t.Description,
                        StartDate = t.StartDate,
                        EndDate = t.EndDate,
                    });
                }

            }

            return View("Course", vm);
        }
        [Authorize(Roles = "Teacher")]
        public ActionResult _TeacherListPartial()
        {
            //Creates a list of _TeacherListparialModel
            List<_TeacherListPartialModel> tl = new List<_TeacherListPartialModel>();

            // Checks the database for all users with the role of "Teacher"
            var teachers = context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(context.Roles.FirstOrDefault(z => z.Name == "Teacher").Id)).ToList();

            //All the users that was found above are put in to a list
            foreach (var t in teachers)
            {
                tl.Add(new _TeacherListPartialModel
                {
                    FirstName = t.FirstName,
                    LastName = t.LastName
                });
            }

            //returns the list to the partial view
            return View(tl);
        }
    }
}