
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
            if (teacher == null)
            {
                return RedirectToAction("_TeacherListPartial");
            }

            var dbteacher = context.Users.Find(teacher.Id);

            if (dbteacher == null)
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

                if (teacher == null)
                {
                    return HttpNotFound();
                }

                etvm = new EditTeacherViewModel
                {
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Email = teacher.Email,
                    Id = id
                };
            }

            return PartialView(etvm);
        }

        //POST: EditTeacher
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult EditTeacher(EditTeacherViewModel edited)
        {
            if (!ModelState.IsValid)
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

            ApplicationUser teacher = userManager.FindById(edited.Id);

            if(teacher == null)
            {
                return RedirectToAction("_TeacherListPartial");
            }

            teacher.FirstName = edited.FirstName;
            teacher.LastName = edited.LastName;
            teacher.Email = edited.Email;
            teacher.UserName = edited.Email;

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
            if (edited.Password != null)
            {
                if (edited.Password == edited.ConfirmPassword)
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
        /// <summary>
        /// CourseList to list all the courses on index page
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Index page for teachers with partialviews that displays courses and teacher list 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="Teacher")]
        public ActionResult Index()
        {
            return View();
        }
        //Get: Course Create
        /// <summary>
        /// Get function for CreateCourse returns a PartialView PartialView
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="Teacher")]
        public ActionResult CreateCourse()
        {
            return PartialView();
        }
        //Post: Course Create
        /// <summary>
        /// Post function for CreateCourse. Creates a new course and returns the new CourseListView 
        /// with all the courses
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Teacher")]
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
                    EndDate = course.EndDate,
                    Modules = new List<ModuleModels>(),
                    Students = new List<ApplicationUser>()

                };
                context.Courses.Add(coursetemp);
                context.SaveChanges();
                return RedirectToAction("CourseListView");
            }

            return View("CourseListView");
        }
        //GET: Course Edit
        [Authorize(Roles = "Teacher")]
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
            if (course == null)
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
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseModels course = context.Courses.Find(id);
            if (course == null)
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

        public ActionResult _StudentListPartial(int id)
        {

            List<_StudentListPartial> sl = new List<_StudentListPartial>();
            var students = context.Courses.FirstOrDefault(t => t.Id == id).Students;

            foreach (var s in students)
            {
                sl.Add(new _StudentListPartial
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Id = s.Id

                });
            }
            return View(sl);
        }

        public ActionResult FullStudentList()
        {
            List<_StudentListPartial> sl = new List<_StudentListPartial>();
            
            // Checks the database for all users with the role of "Student"
            var students = context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(context.Roles.FirstOrDefault(z => z.Name == "Student").Id)).ToList();

            foreach (var s in students)
            {
                sl.Add(new _StudentListPartial
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    EMail = s.Email,
                    CourseId = s.CourseId,
                    Id = s.Id
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

        //POST: CreateStudent
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

        //GET For Editing of student
        [HttpGet]
        public ActionResult EditStudent(string ID)
        {
            EditStudentViewModel esvm = new EditStudentViewModel();

            if (ID != null)
            {
                UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(context);
                UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);

                var student = userManager.FindById(ID);

                if (student == null)
                {
                    return HttpNotFound();
                }

                esvm = new EditStudentViewModel
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    Id = ID
                };
            }
            return PartialView("_EditStudentPartial", esvm);
        }

        //Post: For Edit of student
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent(EditStudentViewModel editStudentViewModel)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("_StudentListPartial");
            }

            if (editStudentViewModel == null)
            {
                return HttpNotFound();
            }

            UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(context);
            UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);

            ApplicationUser student = userManager.FindById(editStudentViewModel.Id);

            if(student == null)
            {
                return RedirectToAction("_StudentListPartial");
            }

            student.FirstName = editStudentViewModel.FirstName;
            student.LastName = editStudentViewModel.LastName;
            student.Email = editStudentViewModel.Email;
            student.UserName = editStudentViewModel.Email;

            var result = userManager.Update(student);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return RedirectToAction("_StudentListPartial", new { id = student.CourseId.Id });
        }

        //GET: DeleteStudent
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteStudent(string id)
        {
            _StudentListPartial vm = new _StudentListPartial();

            if (id != null)
            {
                UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(context);
                UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);

                var student = userManager.FindById(id);

                if (student == null)
                {
                    return HttpNotFound();
                }

                vm = new _StudentListPartial
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Id = student.Id
                };
            }

            return PartialView("_DeleteStudentPartial",vm);
        }

        //POST: DeleteTeacher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStudent(_StudentListPartial student)
        {
            if (student == null)
            {
                return RedirectToAction("_StudentListPartial"); // Will generate yellow screen of death probably
            }

            var dbstudent = context.Users.Find(student.Id);
            int cid = dbstudent.CourseId.Id;

            if (dbstudent == null)
            {
                RedirectToAction("_StudentListPartial", new { id = cid });
            }

            
            context.Entry(dbstudent).State = EntityState.Deleted;

            context.SaveChanges();

            return RedirectToAction("_StudentListPartial", new { id = cid });

        }

        public ActionResult TeacherCourseModulesPartial(int id)
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
                    Id = m.Id,
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
            return PartialView("TeacherCourseModulesPartial", vm);
        }

        [HttpGet]
        //this action result returns the partial containing the modal
        public ActionResult CreateModule(int id)
        {
            var course = context.Courses.FirstOrDefault(c => c.Id == id);
            var lastModule = course.Modules.OrderByDescending(m => m.EndDate).FirstOrDefault();
            CreateModuleViewModel cmvm = new CreateModuleViewModel()
            {
                CourseId = id,
                StartDate = lastModule == null ? course.StartDate : lastModule.EndDate,
                EndDate = lastModule == null ? course.StartDate : lastModule.EndDate,
            };

            return PartialView("CreateModulePartial", cmvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModule(CreateModuleViewModel createModuleViewModel)
        {
            var course = context.Courses.FirstOrDefault(d => d.Id == createModuleViewModel.CourseId);
            if (ModelState.IsValid)
            {
                course.Modules.Add(new ModuleModels
                {
                    Name = createModuleViewModel.Name,
                    Description = createModuleViewModel.Description,
                    StartDate = createModuleViewModel.StartDate,
                    EndDate = createModuleViewModel.EndDate,
                    Activities = new List<ActivityModels>(),
                });

                context.SaveChanges();
            }
            return RedirectToAction("TeacherCourseModulesPartial", new { id = course.Id });
        }

        //GET: DeleteStudent
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteModule(int? id)
        {
            ModuleViewModel vm = new ModuleViewModel();
            if (id != null)
            {
                var module = context.Modules.FirstOrDefault(m => m.Id == id);

                vm = new ModuleViewModel()
                {
                    Id = module.Id,
                    Name = module.Name,
                    Description = module.Description,
                    StartDate = module.StartDate,
                    EndDate = module.EndDate,
                };
            }

            return PartialView("DeleteModulePartial", vm);
        }

        //POST: DeleteTeacher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteModule(ModuleViewModel module)
        {
            if (module == null)
            {
                return RedirectToAction("TeacherCourseModulesPartial"); // Will generate yellow screen of death
            }

            var dbmodule = context.Modules.Find(module.Id);

            int cid = dbmodule.Courses.FirstOrDefault().Id;

            if (dbmodule == null)
            {
                RedirectToAction("TeacherCourseModulesPartial", new { id = cid });
            }


            context.Entry(dbmodule).State = EntityState.Deleted;

            context.SaveChanges();

            return RedirectToAction("TeacherCourseModulesPartial", new { id = cid });

        }

        //GET: Course Edit
        [Authorize(Roles = "Teacher")]
        public ActionResult EditModule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var module = context.Modules.Find(id);

            EditModuleViewModel vm = new EditModuleViewModel
            {
                Id = module.Id,
                Description = module.Description,
                Name = module.Name,
                StartDate = module.StartDate,
                EndDate = module.EndDate

            };

            return PartialView("EditModulePartial", vm);
        }
        //Post: Course Edit
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult EditModule(EditModuleViewModel modulevm)
        {
            var module = context.Modules.FirstOrDefault(m => m.Id == modulevm.Id);
            var course = module.Courses.FirstOrDefault();

            if (ModelState.IsValid)
            {
                module.Name = modulevm.Name;
                module.Description = modulevm.Description;
                module.StartDate = modulevm.StartDate;
                module.EndDate = modulevm.EndDate;

                context.Entry(module).State = EntityState.Modified;
                context.SaveChanges();
            }

            return RedirectToAction("TeacherCourseModulesPartial", new { id = course.Id });
        }

    }
}