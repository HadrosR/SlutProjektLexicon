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
            return PartialView();
        }
        
        //POST: AddTeacher
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult AddTeacher(RegisterViewModel arg)
        
        {
            if (arg == null)
            {
                return HttpNotFound();
            }

            //Add contents of arg to a new post in the database
            UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(context);
            UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);

            ApplicationUser user = new Models.Identity.ApplicationUser { UserName = arg.Email, Email = arg.Email, FirstName = arg.FirstName, LastName = arg.LastName };

            var result = userManager.Create(user, arg.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }

                return RedirectToAction("_TeacherListPartial");
            }

            userManager.AddToRole(user.Id, "Teacher");

            context.SaveChanges();

            return RedirectToAction("_TeacherListPartial");
        }

        //GET: DeleteTeacher
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteTeacher(string id)
        {
            _TeacherListPartialModel vm = new _TeacherListPartialModel();

            if (id != null)
            {
                UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(context);
                UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);

                var teacher = userManager.FindById(id);

                if (teacher == null)
                {
                    return HttpNotFound();
                }

                vm = new _TeacherListPartialModel
                {
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    EMail = teacher.Email,
                    Id = teacher.Id
                };
            }

            return PartialView(vm);
        }

        //POST: DeleteTeacher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTeacher(_TeacherListPartialModel teacher)
        {
            if(teacher == null)
            {
                return RedirectToAction("_TeacherListPartial");
            }

            var dbteacher = context.Users.Find(teacher.Id);

            if(dbteacher == null)
            {
                RedirectToAction("_TeacherListPartial");
            }

            context.Entry(dbteacher).State = EntityState.Deleted;

            context.SaveChanges();

            return RedirectToAction("_TeacherListPartial");
        }

        //GET: EditTeacher
        public ActionResult EditTeacher(string id)
        {
            EditTeacherViewModel etvm = new EditTeacherViewModel();

            if (id != null)
            {
                UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(context);
                UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);

                var teacher = userManager.FindById(id);

                if(teacher == null)
                {
                    return HttpNotFound();
                }

                etvm = new EditTeacherViewModel
                {
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Email = teacher.Email,
                };
            }

            return  PartialView(etvm);
        }

        //POST: EditTeacher
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult EditTeacher(EditTeacherViewModel edited)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("_TeacherListPartial");
            }

            if (edited == null)
            {
                return HttpNotFound();
            }

            //Add contents of arg to a new post in the database
            UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(context);
            UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);

            ApplicationUser teacher = userManager.FindByName(edited.Email);

            teacher.FirstName = edited.FirstName;
            teacher.LastName = edited.LastName;
            teacher.Email = edited.Email;
            
            var result = userManager.Update(teacher);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }

                return RedirectToAction("_TeacherListPartial");
            }

            // Want to change password?
            if(edited.Password != null)
            {
                if(edited.Password == edited.ConfirmPassword )
                {
                    //result = userManager.ChangePassword(teacher.Id, teacher.PasswordHash, edited.Password);

                    PasswordHasher ph = new PasswordHasher();

                    string hashed = ph.HashPassword(edited.Password);

                    var updatedUserPw = context.Users.Find(teacher.Id);

                    updatedUserPw.PasswordHash = hashed;

                    context.Entry(updatedUserPw).State = EntityState.Modified;
                }
            }

            context.SaveChanges();

            return RedirectToAction("_TeacherListPartial");
        
        }
        
        public ActionResult CourseListView()
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

            return PartialView("CourseListView", aktivCourses);
        }

        // GET: Teacher
        [Authorize(Roles ="Teacher")]
        public ActionResult Index()
        {
            return View();
        }
        //Get: Course Create
        [Authorize(Roles ="Teacher")]
        public ActionResult CreateCourse()
        {
            return PartialView();
        }
        //Post: Course Create
        [HttpPost]
        [Authorize(Roles ="Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCourse([Bind(Include = "Id,Name,Description,StartDate,EndDate")] CreateCourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                CourseModels coursetemp = new CourseModels
                {
                    Description = course.Description,
                    Name = course.Name,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate
                
                };
                context.Courses.Add(coursetemp);
                context.SaveChanges();
                return RedirectToAction("CourseListView");
            }

            return View("CourseListView");
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
            CreateCourseViewModel coursetemp = new CreateCourseViewModel
            {
                Id = course.Id,
                Description = course.Description,
                Name = course.Name,
                StartDate = course.StartDate,
                EndDate = course.EndDate

            };
            if (course==null)
            {
                return HttpNotFound();
            }
            return View(coursetemp);
        }
        //Post: Course Edit
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse([Bind(Include = "Id,Name,Description,StartDate,EndDate")] CreateCourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                CourseModels coursetemp = new CourseModels
                {
                    Id = course.Id,
                    Description = course.Description,
                    Name = course.Name,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate

                };
                context.Entry(coursetemp).State = EntityState.Modified;
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
                    Id = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    EMail = t.Email
                });
            }

            //returns the list to the partial view
            return View(tl);
        }
    }
}