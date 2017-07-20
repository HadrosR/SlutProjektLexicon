using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LexiconLMSPortal.Models.Classes;

namespace LexiconLMSPortal.Models.ViewModels
{
    public class ScheduleViewModel
    {
        public List<ScheduleActivityViewModel> Monday { get; set; }
        public List<ScheduleActivityViewModel> Tuesday { get; set; }
        public List<ScheduleActivityViewModel> Wednesday { get; set; }
        public List<ScheduleActivityViewModel> Thursday { get; set; }
        public List<ScheduleActivityViewModel> Friday { get; set; }

        public ScheduleViewModel()
        {
            Monday = new List<ScheduleActivityViewModel>();
            Tuesday = new List<ScheduleActivityViewModel>();
            Wednesday = new List<ScheduleActivityViewModel>();
            Thursday = new List<ScheduleActivityViewModel>();
            Friday = new List<ScheduleActivityViewModel>();
        }
    }
}