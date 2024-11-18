using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using RemindWebApp.DAL;
using RemindWebApp.Models;

namespace RemindWebApp.Areas.RemindWebApp.Controllers
{
    [Area("RemindWebApp")]
    public class AboutContentController : Controller
    {
        private RemindDatabase _remindb;
        private IHostingEnvironment _env;
        public AboutContentController(RemindDatabase remindDatabase, IHostingEnvironment hostingEnvironment)
        {
            _remindb = remindDatabase;
            _env = hostingEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<AboutContent> content = _remindb.AboutContents;
            return View(content);
        }
        public async Task<IActionResult>  Detail(int? id)
        {

            if (id == null) return NotFound();
            AboutContent content = await _remindb.AboutContents.FirstOrDefaultAsync(x => x.Id == id);
            if (content == null) return NotFound();
            return View(content);

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            AboutContent content = await _remindb.AboutContents.FirstOrDefaultAsync(x => x.Id == id);
            if (content == null) return NotFound();
            return View(content);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, AboutContent aboutup)
        {
            if (id == null) return NotFound();
            AboutContent _content = await _remindb.AboutContents.FirstOrDefaultAsync(x => x.Id == id);
            if (_content == null) return NotFound();
 

            if (ModelState["Slogan"].ValidationState == ModelValidationState.Invalid ||
                ModelState["DescriptionFirst"].ValidationState == ModelValidationState.Invalid ||
                ModelState["DescriptionSecond"].ValidationState == ModelValidationState.Invalid ||
                ModelState["DescriptionThird"].ValidationState == ModelValidationState.Invalid)
            {
                return RedirectToAction(nameof(Index));
            }

            _content.Slogan = aboutup.Slogan;
            _content.DescriptionFirst = aboutup.DescriptionFirst;
            _content.DescriptionSecond = aboutup.DescriptionSecond;
            _content.DescriptionThird = aboutup.DescriptionThird;
            
            await _remindb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}