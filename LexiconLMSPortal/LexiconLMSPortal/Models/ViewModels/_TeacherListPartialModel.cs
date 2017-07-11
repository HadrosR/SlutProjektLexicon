﻿using LexiconLMSPortal.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LexiconLMSPortal.Models.ViewModels
{
    public class _TeacherListPartialModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Display(Name ="Teachers:")]
        public string FullName { get { return FirstName + " " + LastName; } }
    }
}