
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

namespace LexiconLMSPortal.Controllers
{
    public class TeacherController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Teacher
        public ActionResult Index()
        {

            var courses = context.Courses;
            //skapar en ny lista
            List<CourseViewModel> aktivCourses = new List<CourseViewModel>();
            //fyller den nya listan med alla curser från CourseViewModel
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Teacher/Courses/id
        public ActionResult Courses(int id)
        {
            // Get the specifik course
            var course = context.Courses.FirstOrDefault(n => n.Id == id);

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
                    Name = m.Name,
                    Description = m.Description,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,

                    /* Activities here */
                    Activities = newActivityList
                });

                //Add viewmodels for every activity in a module
                foreach (var t in m.Activities)
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

        public ActionResult _StudentListPartial(int id)
        {

            List<_StudentListPartial> sl = new List<_StudentListPartial>();
            var students = context.Courses.FirstOrDefault(t => t.Id == id).Students;

            foreach (var s in students)
            {
                sl.Add(new _StudentListPartial
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName
                });
            }
            return View(sl);
        }

        [HttpGet]
        //this action result returns the partial containing the modal
        public ActionResult CreateStudent(int id)
        {
            CreateSudentViewModel csvm = new CreateSudentViewModel
            {
                CourseModel_Id = id
            };

            return View("CreateStudent", csvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStudent([Bind(Include = "Id,FirstName,LastName,Email,Password,CourseModel_Id")] CreateSudentViewModel createSudentViewModel)
        {
            var course = context.Courses.FirstOrDefault(d => d.Id == createSudentViewModel.Id);

            if (ModelState.IsValid)
            {
                UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(context);
                UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);

                Models.Identity.ApplicationUser user = new Models.Identity.ApplicationUser { UserName = createSudentViewModel.Email, Email = createSudentViewModel.Email, FirstName = createSudentViewModel.FirstName, LastName = createSudentViewModel.LastName, };
                var result = userManager.Create(user, createSudentViewModel.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }

                course.Students.Add(user);

                context.SaveChanges();
            }
            return RedirectToAction("_StudentListPartial", new { id = course.Id });
        }
    }
}