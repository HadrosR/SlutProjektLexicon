﻿using System;
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
            return File(fileToRetrieve.Contents, fileToRetrieve.ContentType, fileToRetrieve.Name);
        }

        [OverrideAuthorization]
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

        [OverrideAuthorization]
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

        [OverrideAuthorization]
        public ActionResult DocumentActivityList(int id)
        {
            var activity = db.Activities.FirstOrDefault(c => c.Id == id);

            DocumentActivityViewModel dcvm = new DocumentActivityViewModel
            {
                ActivityId = id,
                Documents = activity.Documents
            };

            return PartialView("DocumentActivityPartial", dcvm);
        }

        [HttpGet]
        public ActionResult AddDocumentCourse(int id)
        {
            CreateDocumentViewModel cdvm = new CreateDocumentViewModel { Id = id, DocumentType = CreateDocumentType.Course};
            ViewBag.Target = "Course";
            return PartialView("CreateDocumentPartial", cdvm);
        }

        [HttpGet]
        public ActionResult AddDocumentModule(int id)
        {
            CreateDocumentViewModel cdvm = new CreateDocumentViewModel { Id = id, DocumentType = CreateDocumentType.Module };
            ViewBag.Target = "Module";
            return PartialView("CreateDocumentPartial", cdvm);
        }

        [HttpGet]
        public ActionResult AddDocumentActivity(int id)
        {
            CreateDocumentViewModel cdvm = new CreateDocumentViewModel { Id = id, DocumentType = CreateDocumentType.Activity };
            ViewBag.Target = "Activity";
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
                        Name = upload.FileName,
                        TimeStamp = DateTime.Now,
                        ContentType = upload.ContentType,
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        document.Contents = reader.ReadBytes(upload.ContentLength);
                    }
                    switch(cdvm.DocumentType)
                    {
                        case CreateDocumentType.Course:
                            db.Courses.FirstOrDefault(c => c.Id == cdvm.Id).Documents.Add(document);
                            db.SaveChanges();
                            return RedirectToAction("DocumentCourseList", new { id = cdvm.Id });

                        case CreateDocumentType.Module:
                            db.Modules.FirstOrDefault(c => c.Id == cdvm.Id).Documents.Add(document);
                            db.SaveChanges();
                            return RedirectToAction("DocumentModuleList", new { id = cdvm.Id });

                        case CreateDocumentType.Activity:
                            db.Activities.FirstOrDefault(c => c.Id == cdvm.Id).Documents.Add(document);
                            db.SaveChanges();
                            return RedirectToAction("DocumentActivityList", new { id = cdvm.Id });
                    }
                }
            }

            return RedirectToAction("Course", "Teacher", new { id=id});
        }

        [NonAction]
        public DeleteDocumentViewModel FindDocumentDelete(int id, CreateDocumentType dt)
        {
            var document = db.Documents.Find(id);

            int parentId = -1;
            switch (dt)
            {
                case CreateDocumentType.Course:
                    parentId = document.Course.Id;
                    break;
                case CreateDocumentType.Module:
                    parentId = document.Module.Id;
                    break;

                case CreateDocumentType.Activity:
                    parentId = document.Activity.Id;
                    break;
            }

            DeleteDocumentViewModel ddvm = new DeleteDocumentViewModel
            {
                Id = id,
                Name = document.Name,
                DocumentType = dt,
                ParentId = parentId
            };

            return ddvm;
        }

        public ActionResult RemoveDocumentCourse(int id)
        {
            DeleteDocumentViewModel ddvm = FindDocumentDelete(id, CreateDocumentType.Course);
            ViewBag.Target = "Course";
            return PartialView("DeleteDocumentPartial", ddvm);
        }

        public ActionResult RemoveDocumentModule(int id)
        {
            DeleteDocumentViewModel ddvm = FindDocumentDelete(id, CreateDocumentType.Module);
            ViewBag.Target = "Module";
            return PartialView("DeleteDocumentPartial", ddvm);
        }

        public ActionResult RemoveDocumentActivity(int id)
        {
            DeleteDocumentViewModel ddvm = FindDocumentDelete(id, CreateDocumentType.Activity);
            ViewBag.Target = "Activity";
            return PartialView("DeleteDocumentPartial", ddvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveDocument(DeleteDocumentViewModel ddvm)
        {
            var doc = db.Documents.Find(ddvm.Id);

            string actionName = "";

            switch (ddvm.DocumentType)
            {
                case CreateDocumentType.Course:
                    actionName = "DocumentCourseList";
                    break;
                case CreateDocumentType.Module:
                    actionName = "DocumentModuleList";
                    break;

                case CreateDocumentType.Activity:
                    actionName = "DocumentActivityList";
                    break;
            }
            
            db.Entry(doc).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction(actionName, new { id = ddvm.ParentId });
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
