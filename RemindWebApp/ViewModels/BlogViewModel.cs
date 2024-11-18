using RemindWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.ViewModels
{
    public class BlogViewModel
    {
        public Blog BlogSingleHeader { get; set; }
        public IEnumerable<Blog> BlogsHeader { get; set; }
        public IEnumerable<Blog> BlogsCenter { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<Advertisment> Advertisments  { get; set; }

    }
}
