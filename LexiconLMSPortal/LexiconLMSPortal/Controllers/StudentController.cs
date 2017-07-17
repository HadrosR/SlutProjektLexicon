using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LexiconLMSPortal.Models.Classes;
using LexiconLMSPortal.Models.Identity;
using LexiconLMSPortal.Models.ViewModels;

namespace LexiconLMSPortal.Controllers
{
    public class StudentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult _StudentListPartial(int id)
        {
            List<_StudentListPartial> sl = new List<_StudentListPartial>();
            var students = db.Courses.FirstOrDefault(t => t.Id == id).Students;

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

        // GET: Student
        public ActionResult Index()
        {
            return View("");
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
