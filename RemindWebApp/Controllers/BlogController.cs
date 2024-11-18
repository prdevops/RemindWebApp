using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RemindWebApp.DAL;
using RemindWebApp.ViewModels;

namespace RemindWebApp.Controllers
{
 
    public class BlogController : Controller
    {
        private RemindDatabase _remindb;
        public BlogController(RemindDatabase remindDatabase)
        {
            _remindb = remindDatabase;
        }
        public IActionResult Index()
        {
            ViewBag.BlogCount = _remindb.BlogNews.Count();
            BlogViewModel blogs = new BlogViewModel()
            {
                BlogSingleHeader = _remindb.Blogs.FirstOrDefault(e => e.HeaderBig != null),
                BlogsHeader = _remindb.Blogs.Where(e =>e.HeaderSmall !=null),
                BlogsCenter =_remindb.Blogs.OrderByDescending(i => i.Id).Take(3),
                Blogs = _remindb.Blogs.OrderByDescending(l =>l.Id).Take(4),
                Advertisments = _remindb.Advertisments
            };
            return View(blogs);
        }

        public IActionResult LoadBlog(int skip)
        {


            BlogViewModel blogs = new BlogViewModel()
            {
                BlogSingleHeader = _remindb.Blogs.FirstOrDefault(e => e.HeaderBig != null),
                BlogsHeader = _remindb.Blogs.Where(e => e.HeaderSmall != null),
                BlogsCenter = _remindb.Blogs.OrderByDescending(i => i.Id).Skip(skip).Take(3),
                Blogs = _remindb.Blogs.OrderByDescending(l => l.Id).Take(4),
                Advertisments = _remindb.Advertisments
            };

            return PartialView("_PartialBlog", blogs);
        }
    }
}