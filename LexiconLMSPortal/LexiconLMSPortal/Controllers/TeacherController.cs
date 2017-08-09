
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
    [Authorize(Roles ="Teacher")]
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
            var sortres = aktivCourses.OrderBy(n => n.StartDate);

            return PartialView("CourseListView", sortres);
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
        public ActionResult CreateCourse([Bind(Include = "Id,Name,Description,StartDate,EndDate")] CourseViewModel course)
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
                var newlyAdded = context.Courses.Add(coursetemp);
                context.SaveChanges();
                return RedirectToAction("Course", new { id = newlyAdded.Id });
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
            CourseViewModel coursetemp = new CourseViewModel
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
            return PartialView("EditCoursePartial",coursetemp);
        }
        //Post: Course Edit
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse([Bind(Include = "Id,Name,Description,StartDate,EndDate")] CourseViewModel course)
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
                return RedirectToAction("CourseListView");
            }
            return View("CourseListView");
        }
        //GET: Course delete
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteCourse(int? id)
        {
            CourseViewModel tempcourse = new CourseViewModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = context.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            tempcourse = new CourseViewModel
            {
                Name = course.Name,
                Description = course.Description,
                Id = course.Id,
                StartDate = course.StartDate,
                EndDate = course.EndDate
            };
            return PartialView("DeleteCoursePartial",tempcourse);
        }
        //Post: Course Delete
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCourse(CourseViewModel course)
        {
            if (course == null)
            {
                return RedirectToAction("DeleteCoursePartial");
            }
            var tempcourse = context.Courses.Find(course.Id);
            if (tempcourse==null)
            {
                return RedirectToAction("DeleteCoursePartial");
            }
            context.Courses.Remove(tempcourse);
            context.SaveChanges();
            return RedirectToAction("CourseListView");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Teacher/Course/id
        [Authorize(Roles = "Teacher")]
        public ActionResult Course(int? id)
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
                EndDate = course.EndDate,
                Documents = course.Documents
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
            _StudentListPartialContainer container = new _StudentListPartialContainer();

            List<_StudentListPartial> sl = new List<_StudentListPartial>();

            var course = context.Courses.FirstOrDefault(t => t.Id == id);
            var students = course.Students;

            foreach (var s in students)
            {
                sl.Add(new _StudentListPartial
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Id = s.Id

                });
            }
            container.Students = sl;
            container.CourseId = course.Id;

            return View(container);
        }

        public ActionResult FullStudentList(string sortOrder)
        {
            return View();
        }

        public ActionResult _FullStudentListPartial(string sortOrder,string search)
        {
            List<_StudentListPartial> sl = new List<_StudentListPartial>();
            //Creates variables that saves the users input of a sortorder
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "name" : "";
            ViewBag.EmailSort = sortOrder == "email" ? "email_des" : "email";
            ViewBag.CourseSort = sortOrder == "Course" ? "Course_des" : "Course";
            // Checks the database for all users with the role of "Student"
            var students = context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(context.Roles.FirstOrDefault(z => z.Name == "Student").Id)).ToList();
            //Finds the order the user wants in the list
            var sortres = sl.OrderBy(n => n.FirstName);
            foreach (var s in students)
            {
                switch (search)
                {
                    case null:
                        sl.Add(new _StudentListPartial
                        {
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            EMail = s.Email,
                            CourseId = s.CourseId,
                            Id = s.Id
                        });
                        break;
                    default:
                        if (/*s.Email.Contains(search) || s.CourseId.Name.Contains(search) || */s.FirstName.ToUpper().Contains(search.ToUpper()) || s.LastName.ToUpper().Contains(search.ToUpper()))
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
                        break;
                }
            }

            switch (sortOrder)
            {
                case "name":
                    sortres = sl.OrderByDescending(n => n.FirstName);
                    break;
                case "email":
                    sortres = sl.OrderBy(e => e.EMail);
                    break;
                case "email_des":
                    sortres = sl.OrderByDescending(e => e.EMail);
                    break;
                case "Course":
                    sortres = sl.OrderBy(c => c.CourseId.Name);
                    break;
                case "Course_des":
                    sortres = sl.OrderByDescending(c => c.CourseId.Name);
                    break;
                default:
                    sortres = sl.OrderBy(n => n.FirstName);
                    break;
            }
            

            return View(sortres);
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

                Models.Identity.ApplicationUser user = new Models.Identity.ApplicationUser { UserName = createSudentViewModel.Email, Email = createSudentViewModel.Email, FirstName = createSudentViewModel.FirstName, LastName = createSudentViewModel.LastName};
                var result = userManager.Create(user, createSudentViewModel.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
                userManager.AddToRole(user.Id, "Student");
                course.Students.Add(user);
                TempData["Message"] = "Succsessfully added a student";
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

        //GET For Editing of student
        [HttpGet]
        public ActionResult EditStudentFullList(string ID)
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
            return PartialView("_EditStudentPartialFullList", esvm);
        }

        //Post: For Edit of student full list : UNTESTED
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudentFullList(EditStudentViewModel editStudentViewModel)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("_FullStudentListPartial");
            }

            if (editStudentViewModel == null)
            {
                return HttpNotFound();
            }

            UserStore<Models.Identity.ApplicationUser> userStore = new UserStore<Models.Identity.ApplicationUser>(context);
            UserManager<Models.Identity.ApplicationUser> userManager = new UserManager<Models.Identity.ApplicationUser>(userStore);

            ApplicationUser student = userManager.FindById(editStudentViewModel.Id);

            if (student == null)
            {
                return RedirectToAction("_FullStudentListPartial");
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

            return RedirectToAction("_FullStudentListPartial");
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

        //GET: DeleteStudentFullList
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteStudentFullList(string id)
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
                    EMail = student.Email,
                    CourseId = student.CourseId,
                    Id = student.Id
                };
            }

            return PartialView("_DeleteStudentPartialFullList", vm);
        }

        //POST: DeleteTeacherFullList : UNTESTED
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStudentFullList(_StudentListPartial student)
        {
            if (student == null)
            {
                return RedirectToAction("_FullStudentListPartial"); // Will generate yellow screen of death probably
            }

            var dbstudent = context.Users.Find(student.Id);
            int cid = dbstudent.CourseId.Id;

            if (dbstudent == null)
            {
                RedirectToAction("_FullStudentListPartial", new { id = cid });
            }

            context.Entry(dbstudent).State = EntityState.Deleted;

            context.SaveChanges();

            return RedirectToAction("_FullStudentListPartial", new { id = cid });

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
                    Documents = m.Documents.ToList(),
                });

            }
            return PartialView("TeacherCourseModulesPartial", vm);
        }

        public ActionResult TeacherCourseActivitiesPartial(int id)
        {
            // Get the specifik module
            var module = context.Modules.FirstOrDefault(n => n.Id == id);

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
            return PartialView("TeacherCourseActivitiesPartial", vm);
        }

        [HttpGet]
        public ActionResult CreateModule(int id)
        {
            // Find appropriate course to add module to 
            var course = context.Courses.FirstOrDefault(c => c.Id == id);

            // Get module with the latest date
            var lastModule = course.Modules.OrderByDescending(m => m.EndDate).FirstOrDefault();

            CreateModuleViewModel cmvm = new CreateModuleViewModel()
            {
                CourseId = id,
                // Use the latest module to determine an example date
                StartDate = lastModule == null ? course.StartDate : lastModule.EndDate,
                EndDate = lastModule == null ? course.StartDate : lastModule.EndDate,
            };

            return PartialView("CreateModulePartial", cmvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModule(CreateModuleViewModel createModuleViewModel)
        {
            // Find the course to add the module to
            var course = context.Courses.FirstOrDefault(d => d.Id == createModuleViewModel.CourseId);

            if (ModelState.IsValid)
            {
                // Add a module to the course using a new model 
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

        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteModule(int? id)
        {
            ModuleViewModel vm = new ModuleViewModel();

            // Check if the id is correct
            if (id != null)
            {
                // Find the module to be deleted
                var module = context.Modules.FirstOrDefault(m => m.Id == id);

                // Return a model of the corresponding module
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteModule(ModuleViewModel module)
        {
            if (module == null)
            {
                return RedirectToAction("TeacherCourseModulesPartial"); // Will generate yellow screen of death
            }

            // Find the module to be deleted
            var dbmodule = context.Modules.Find(module.Id);

            // Store its id for later
            int cid = dbmodule.Courses.FirstOrDefault().Id;

            if (dbmodule == null)
            {
                RedirectToAction("TeacherCourseModulesPartial", new { id = cid });
            }

            // Remove the module
            context.Entry(dbmodule).State = EntityState.Deleted;

            context.SaveChanges();

            return RedirectToAction("TeacherCourseModulesPartial", new { id = cid });

        }

        [Authorize(Roles = "Teacher")]
        public ActionResult EditModule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find the module to be edited
            var module = context.Modules.Find(id);

            // Create a view model to return to the partial view
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

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult EditModule(EditModuleViewModel modulevm)
        {
            // Store module and course
            var module = context.Modules.FirstOrDefault(m => m.Id == modulevm.Id);
            var course = module.Courses.FirstOrDefault();

            if (ModelState.IsValid)
            {
                // Change its attributes to the edited ones
                module.Name = modulevm.Name;
                module.Description = modulevm.Description;
                module.StartDate = modulevm.StartDate;
                module.EndDate = modulevm.EndDate;

                context.Entry(module).State = EntityState.Modified;
                context.SaveChanges();
            }

            return RedirectToAction("TeacherCourseModulesPartial", new { id = course.Id });
        }


        [HttpGet]
        public ActionResult CreateActivity(int id)
        {
            // Find the module to add an activity to
            var module = context.Modules.FirstOrDefault(c => c.Id == id);

            // Create a templated model for the user to edit
            CreateActivityViewModel cmvm = new CreateActivityViewModel()
            {
                ModuleId = id,
                StartDate = module.StartDate,
                EndDate = module.StartDate
            };

            return PartialView("CreateActivityPartial", cmvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateActivity(CreateActivityViewModel createActivityViewModel)
        {
            // Find the module to add the activity to
            var module = context.Modules.FirstOrDefault(d => d.Id == createActivityViewModel.ModuleId);

            if (ModelState.IsValid)
            {
                // Add a new activity model to the module created from the user's input
                module.Activities.Add(new ActivityModels
                {
                    Name = createActivityViewModel.Name,
                    Description = createActivityViewModel.Description,
                    StartDate = createActivityViewModel.StartDate,
                    EndDate = createActivityViewModel.EndDate,
                });

                context.SaveChanges();
            }
            return RedirectToAction("TeacherCourseActivitiesPartial", new { id = module.Id });
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteActivity(int? id)
        {
            ActivityViewModel vm = new ActivityViewModel();

            if (id != null)
            {
                // Find the activity to be deleted
                var activity = context.Activities.FirstOrDefault(a => a.Id == id);

                // Store information for confirmation
                vm = new ActivityViewModel()
                {
                    // To be able to re-render partial view later
                    ModuleId = activity.Modules.FirstOrDefault().Id,
                    Id = activity.Id,
                    Name = activity.Name,
                    Description = activity.Description,
                    StartDate = activity.StartDate,
                    EndDate = activity.EndDate,
                };
            }

            return PartialView("DeleteActivityPartial", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteActivity(ActivityViewModel activity)
        {
            if (activity == null)
            {
                return RedirectToAction("TeacherCourseActivitiesPartial"); // Will generate yellow screen of death
            }

            // Find the activity to be deleted
            var dbactivity = context.Activities.Find(activity.Id);

            // Store its module's id for later
            int mid = dbactivity.Modules.FirstOrDefault().Id;

            if (dbactivity == null)
            {
                return RedirectToAction("TeacherCourseActivitiesPartial", new { id = mid });
            }

            // Delete it
            context.Entry(dbactivity).State = EntityState.Deleted;

            context.SaveChanges();

            return RedirectToAction("TeacherCourseActivitiesPartial", new { id = mid });

        }

        //GET: Course Edit
        [Authorize(Roles = "Teacher")]
        public ActionResult EditActivity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            // Find the activity to be edited
            var activity = context.Activities.Find(id);

            // Store current information to present it to the user for edit
            EditActivityViewModel vm = new EditActivityViewModel
            {
                Id = activity.Id,
                Description = activity.Description,
                Name = activity.Name,
                StartDate = activity.StartDate,
                EndDate = activity.EndDate,
                ModuleId = activity.Modules.FirstOrDefault().Id
                
            };

            return PartialView("EditActivityPartial", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult EditActivity(EditActivityViewModel activityvm)
        {
            // Find activity to be edited
            var activity = context.Activities.FirstOrDefault(a => a.Id == activityvm.Id);

            // Store its module for later
            var module = activity.Modules.FirstOrDefault();

            if (ModelState.IsValid)
            {
                // Edit the activity using user inputted values stored in the view model
                activity.Name = activityvm.Name;
                activity.Description = activityvm.Description;
                activity.StartDate = activityvm.StartDate;
                activity.EndDate = activityvm.EndDate;

                context.Entry(activity).State = EntityState.Modified;
                context.SaveChanges();
            }

            return RedirectToAction("TeacherCourseActivitiesPartial", new { id = module.Id });
        }

    }
}