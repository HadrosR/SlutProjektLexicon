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
            return View(db.Modules.ToList());
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModuleModels moduleModels = db.Modules.Find(id);
            if (moduleModels == null)
            {
                return HttpNotFound();
            }
            return View(moduleModels);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate")] ModuleModels moduleModels)
        {
            if (ModelState.IsValid)
            {
                db.Modules.Add(moduleModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(moduleModels);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModuleModels moduleModels = db.Modules.Find(id);
            if (moduleModels == null)
            {
                return HttpNotFound();
            }
            return View(moduleModels);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate")] ModuleModels moduleModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(moduleModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(moduleModels);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModuleModels moduleModels = db.Modules.Find(id);
            if (moduleModels == null)
            {
                return HttpNotFound();
            }
            return View(moduleModels);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModuleModels moduleModels = db.Modules.Find(id);
            db.Modules.Remove(moduleModels);
            db.SaveChanges();
            return RedirectToAction("Index");
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
