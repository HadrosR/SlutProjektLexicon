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

namespace LexiconLMSPortal.Controllers
{
    public class DocumentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Document
        public ActionResult GetFile(int id)
        {
            var fileToRetrieve = db.Documents.Find(id);
            return File(fileToRetrieve.Contents, "txt");
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