using LexiconLMSPortal.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LexiconLMSPortal.Models.ViewModels
{
    public class DocumentModuleViewModel
    {
        public int ModuleId { get; set; }

        public ICollection<DocumentModels> Documents { get; set; }
    }
}