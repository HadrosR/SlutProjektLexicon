using LexiconLMSPortal.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LexiconLMSPortal.Models.ViewModels
{
    public class DocumentCourseViewModel
    {
        public int CourseId { get; set; }

        public ICollection<DocumentModels> Documents { get; set; }
    }
}