using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RemindWebApp.DAL;
using RemindWebApp.ViewModels;

namespace RemindWebApp.Controllers
{
 
    public class AboutController : Controller
    {

        private RemindDatabase _remindb;
        public AboutController(RemindDatabase remindDatabase)
        {
            _remindb = remindDatabase;
        }
    

        public IActionResult Index()
        {
            AboutViewModel about = new AboutViewModel()
            {
                AboutContents = _remindb.AboutContents,
                AboutSliders = _remindb.AboutSliders,
                Teams = _remindb.Teams

            };
            return View(about);
          
        }
    }
}