using RemindWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.ViewModels
{
    public class BlogNewsViewModel
    {
        public IEnumerable<BlogNew> BlogNewses { get; set; }
        public IEnumerable<Advertisment> Advertisments { get; set; }
    }
}
