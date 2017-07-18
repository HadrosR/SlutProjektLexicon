using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LexiconLMSPortal.Models.Classes;

namespace LexiconLMSPortal.Models.ViewModels
{
    public class ScheduleViewModel
    {
        public List<ActivityModels> Monday { get; set; }
        public List<ActivityModels> Tuesday { get; set; }
        public List<ActivityModels> Wednesday { get; set; }
        public List<ActivityModels> Thursday { get; set; }
        public List<ActivityModels> Friday { get; set; }

        public ScheduleViewModel()
        {
            Monday = new List<ActivityModels>();
            Tuesday = new List<ActivityModels>();
            Wednesday = new List<ActivityModels>();
            Thursday = new List<ActivityModels>();
            Friday = new List<ActivityModels>();
        }
    }
}