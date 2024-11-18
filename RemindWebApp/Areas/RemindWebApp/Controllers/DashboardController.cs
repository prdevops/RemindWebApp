    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RemindWebApp.Areas.RemindWebApp.Controllers
{
    [Area("RemindWebApp")]
    public class DashboardController : Controller
    {
        [Area("RemindWebApp")]

        public IActionResult Index()
        {
            return View();
        }
    }
}