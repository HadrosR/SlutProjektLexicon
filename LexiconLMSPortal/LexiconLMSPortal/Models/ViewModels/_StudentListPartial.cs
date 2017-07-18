using LexiconLMSPortal.Models.Classes;
using LexiconLMSPortal.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LexiconLMSPortal.Models.ViewModels
{
    public class _StudentListPartial
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Display(Name ="Email")]
        public string EMail { get; set; }
        [Display(Name ="Course")]
        public CourseModels CourseId { get; set; }
        [Display(Name = "Students")]
        public string FullName { get { return FirstName + " " + LastName; } }

        //public virtual ICollection<ApplicationUser> Course { get; set; }
    } 
}