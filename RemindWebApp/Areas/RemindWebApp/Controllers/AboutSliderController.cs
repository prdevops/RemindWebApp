using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindWebApp.DAL;
using RemindWebApp.DeleteImg;
using RemindWebApp.Extension_GenerateImage;
using RemindWebApp.Models;

namespace RemindWebApp.Areas.RemindWebApp.Controllers
{
    [Area("RemindWebApp")]
    public class AboutSliderController : Controller
    {
        private RemindDatabase _remindb;
        private IHostingEnvironment _env;
        public AboutSliderController(RemindDatabase remindDatabase, IHostingEnvironment hostingEnvironment)
        {
            _remindb = remindDatabase;
            _env = hostingEnvironment;

        }
        public IActionResult Index()
        {
            IEnumerable<AboutSlider> aboutSlider = _remindb.AboutSliders;
            return View(aboutSlider);
        }

        public async Task<IActionResult> Detail(int? id) { 
            if (id == null) return NotFound();
            AboutSlider slider = _remindb.AboutSliders.FirstOrDefault(x => x.Id == id); ;
            if (slider == null) return NotFound();
            return View(slider);
         
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(AboutSlider slider)
        {
            if (slider.Photo != null)
            {
                if (!slider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "You can chose only image format");
                    return View();
                }

                if (!slider.Photo.CheckSize(2))
                {
                    ModelState.AddModelError("Photo", "You can chose only small 2 MB");
                    return View();
                }


                string createdImage = await slider.Photo.CopyImage(_env.WebRootPath, "team");
                #region
                AboutSlider newslider = new AboutSlider()
                {
                    ImagePath = createdImage

                };
                #endregion


            
                await _remindb.AboutSliders.AddAsync(newslider);


            }

            await _remindb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            AboutSlider slider = _remindb.AboutSliders.FirstOrDefault(a => a.Id == id); ;
            if (slider == null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {

            if (id == null) return NotFound();
            AboutSlider sliderdb = await _remindb.AboutSliders.FirstOrDefaultAsync(x => x.Id == id); 
            if (sliderdb == null) return NotFound();
            

            _remindb.AboutSliders.Remove(sliderdb);
            await _remindb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            AboutSlider slider = await _remindb.AboutSliders.FirstOrDefaultAsync(x => x.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, AboutSlider upslider)
        {
            if (id == null) return NotFound();
            AboutSlider _sliderdb = await _remindb.AboutSliders.FirstOrDefaultAsync(x => x.Id == id);
            if (upslider == null) return NotFound();


            if (upslider.ChangePhoto != null)
            {
                if (!upslider.ChangePhoto.IsImage())
                {
                    ModelState.AddModelError("ChangePhoto", "You can chose only image format");
                    return View();
                }

                if (!upslider.ChangePhoto.CheckSize(2))
                {
                    ModelState.AddModelError("ChangePhoto", "You can chose only small 2 MB");
                    return View();
                }


                string updateimage = await upslider.ChangePhoto.CopyImage(_env.WebRootPath, "team");
                upslider.ImagePath = updateimage;
                DeleteImage.DeleteFromFolder(_env.WebRootPath, _sliderdb.ImagePath);


            }

            _sliderdb.ImagePath = upslider.ImagePath;




            await _remindb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}