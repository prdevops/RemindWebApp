using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RemindWebApp.DAL;
using RemindWebApp.ViewModels;

namespace RemindWebApp.Controllers
{
    
    public class BlogNewsController : Controller
    {
        private RemindDatabase _remind;
        public BlogNewsController(RemindDatabase remindDatabase)
        { 
            _remind = remindDatabase;
        }

        public IActionResult Index()
        {
            ViewBag.BlogNewsCount = _remind.BlogNews.Count();
            BlogNewsViewModel blogNews = new BlogNewsViewModel()
            {
             
                Advertisments = _remind.Advertisments,
                BlogNewses = _remind.BlogNews.Take(4)
               
            };
            return View(blogNews);
        }

        public IActionResult LoadMore(int skip)
        {
            BlogNewsViewModel blogNews = new BlogNewsViewModel()
            {
                Advertisments = _remind.Advertisments,
                BlogNewses = _remind.BlogNews.Skip(skip).Take(4)

            };

            return PartialView("_PartialBlogNews", blogNews);
            #region
            //return Json(_remind.BlogNews.Select(x => new 
            //{
            //    x.ImagePath

            //}));
            #endregion

        }

    }
}