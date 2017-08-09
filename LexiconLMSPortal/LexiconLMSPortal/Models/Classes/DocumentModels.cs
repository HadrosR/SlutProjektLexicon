using LexiconLMSPortal.Models.Classes;
using LexiconLMSPortal.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LexiconLMSPortal.Models.Classes
{
    public class DocumentModels
    {
        public int Id { get; set; }      
        public string Name { get; set; }
        public DateTime TimeStamp { get; set; }
        public byte[] Contents { get; set; }
        public string ContentType { get; set; }

        public virtual ApplicationUser Owner { get; set; }
        public virtual CourseModels Course { get; set; }
        public virtual ModuleModels Module { get; set; }
        public virtual ActivityModels Activity { get; set; }
    }
}