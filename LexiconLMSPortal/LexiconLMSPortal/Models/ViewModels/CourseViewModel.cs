using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LexiconLMSPortal.Models.Classes;
using System.ComponentModel.DataAnnotations;

namespace LexiconLMSPortal.Models.ViewModels
{
    public class CourseViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name ="Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name ="End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }     
    }
}