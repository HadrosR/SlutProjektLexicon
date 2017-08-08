using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LexiconLMSPortal.Models.ViewModels
{
    public enum CreateDocumentType
    {
        Course,
        Module,
        Activity
    }

    public class CreateDocumentViewModel
    {
        // Course, Module or Activity Id
        [Key]
        public int Id { get; set; }
        
        public CreateDocumentType DocumentType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

}