using RemindWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.ViewModels
{
    public class AboutViewModel
    {
        public IEnumerable<AboutContent> AboutContents { get; set; }
        public IEnumerable<AboutSlider> AboutSliders { get; set; }
        public IEnumerable<Team> Teams { get; set; }
    }
}
