using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using LexiconLMSPortal.Models.Classes;
using LexiconLMSPortal.Models.Identity;
using LexiconLMSPortal.Models.ViewModels;

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
    }
}