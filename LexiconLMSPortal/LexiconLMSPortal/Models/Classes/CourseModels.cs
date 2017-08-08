using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LexiconLMSPortal.Models.Classes
{
    public class CourseModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<Identity.ApplicationUser> Students { get; set; }
        public virtual ICollection<ModuleModels> Modules { get; set; }
        public virtual ICollection<DocumentModels> Documents { get; set; }
    }
}