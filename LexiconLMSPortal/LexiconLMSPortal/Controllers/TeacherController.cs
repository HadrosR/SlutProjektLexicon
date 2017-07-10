using LexiconLMSPortal.Models.Identity;
using LexiconLMSPortal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LexiconLMSPortal.Controllers
{
    public class TeacherController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        // GET: Teacher/Courses/id
        public ActionResult Courses(int id)
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
                Modules = new List<ModulesViewModel>()
            };

            // Add viewmodels for every module
            foreach(var m in course.Modules)
            {
                vm.Modules.Add(new ModulesViewModel
                {
                    Name = m.Name,
                    Description = m.Description,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate

                    /* Activities here */
                });
            }

            return View("Course", vm);
        }
    }
}