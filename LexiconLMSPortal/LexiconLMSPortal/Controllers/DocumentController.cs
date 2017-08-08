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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace LexiconLMSPortal.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class DocumentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Document
        [OverrideAuthorization]
        public ActionResult GetFile(int id)
        {
            var fileToRetrieve = db.Documents.Find(id);
            return File(fileToRetrieve.Contents, "txt");
        }

        public ActionResult DocumentCourseList(int id)
        {
            var course = db.Courses.FirstOrDefault(c => c.Id == id);

            DocumentCourseViewModel dcvm = new DocumentCourseViewModel
            {
                CourseId = id,
                Documents = course.Documents
            };

            return PartialView("DocumentCoursePartial", dcvm);
        }

        public ActionResult DocumentModuleList(int id)
        {
            var module = db.Modules.FirstOrDefault(c => c.Id == id);

            DocumentModuleViewModel dcvm = new DocumentModuleViewModel
            {
                ModuleId = id,
                Documents = module.Documents
            };

            return PartialView("DocumentModulePartial", dcvm);
        }

        [HttpGet]
        public ActionResult AddDocumentCourse(int id)
        {
            CreateDocumentViewModel cdvm = new CreateDocumentViewModel { CourseId = id, DocumentType = CreateDocumentType.Course};
            ViewBag.Target = "#DocumentCoursePartial";
            return PartialView("CreateDocumentPartial", cdvm);
        }

        [HttpGet]
        public ActionResult AddDocumentModule(int id)
        {
            CreateDocumentViewModel cdvm = new CreateDocumentViewModel { ModuleId = id, DocumentType = CreateDocumentType.Module };
            ViewBag.Target = "#DocumentModulePartial";
            return PartialView("CreateDocumentPartial", cdvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDocument(CreateDocumentViewModel cdvm, HttpPostedFileBase upload)
        {
            int id = -1;

            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    UserStore<ApplicationUser> us = new UserStore<ApplicationUser>(db);
                    UserManager<ApplicationUser> um = new UserManager<ApplicationUser>(us);

                    var document = new DocumentModels
                    {
                        Name = cdvm.Name,
                        Description = cdvm.Description,
                        Owner = um.FindByName(User.Identity.Name),
                        TimeStamp = DateTime.Now
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        document.Contents = reader.ReadBytes(upload.ContentLength);
                    }
                    switch(cdvm.DocumentType)
                    {
                        case CreateDocumentType.Course:
                            db.Courses.FirstOrDefault(c => c.Id == cdvm.CourseId).Documents.Add(document);
                            id = cdvm.CourseId;
                            db.SaveChanges();
                            return RedirectToAction("DocumentCourseList", new { id = id });

                        case CreateDocumentType.Module:
                            db.Modules.FirstOrDefault(c => c.Id == cdvm.ModuleId).Documents.Add(document);
                            id = db.Modules.FirstOrDefault(m => m.Id == cdvm.ModuleId).Courses.FirstOrDefault().Id;
                            db.SaveChanges();
                            return RedirectToAction("DocumentModuleList", new { id = id });
                    }
                }
            }

            return RedirectToAction("Course", "Teacher", new { id=id});
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
