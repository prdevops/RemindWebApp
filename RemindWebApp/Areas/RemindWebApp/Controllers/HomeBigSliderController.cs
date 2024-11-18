using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using RemindWebApp.DAL;
using RemindWebApp.DeleteImg;
using RemindWebApp.Extension_GenerateImage;
using RemindWebApp.Models;

namespace RemindWebApp.Areas.RemindWebApp.Controllers
{
    [Area("RemindWebApp")]
    public class HomeBigSliderController : Controller
    {
        private RemindDatabase _remindb;
        private IHostingEnvironment _env;
        public HomeBigSliderController(RemindDatabase remindDatabase, IHostingEnvironment hostingEnvironment)
        {
            _remindb = remindDatabase;
            _env = hostingEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<HomeSliderBig> slider = _remindb.HomeSliderBigs;
            return View(slider);
        }


        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null) return NotFound();
            HomeSliderBig slider = await _remindb.HomeSliderBigs.FirstOrDefaultAsync(x => x.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(HomeSliderBig homeslider)
        {

            if (ModelState["Name"].ValidationState == ModelValidationState.Invalid ||
                ModelState["SubTitle"].ValidationState == ModelValidationState.Invalid ||
                ModelState["Link"].ValidationState == ModelValidationState.Invalid)
            {
                return RedirectToAction(nameof(Index));
            }

            if (homeslider.Photo != null)
            {
                if (!homeslider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "You can chose only image format");
                    return View();
                }

                if (!homeslider.Photo.CheckSize(2))
                {
                    ModelState.AddModelError("Photo", "You can chose only small 2 MB");
                    return View();
                }


                string createdImage = await homeslider.Photo.CopyImage(_env.WebRootPath, "team");
                #region
                HomeSliderBig newhome = new HomeSliderBig()
                {
                    Title = homeslider.Title,
                    SubTitle = homeslider.SubTitle,
                    Link = homeslider.Link

                };
                #endregion


                newhome.ImgPath = createdImage;

               
                await _remindb.HomeSliderBigs.AddAsync(newhome);


            }

            await _remindb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            HomeSliderBig slider = await _remindb.HomeSliderBigs.FirstOrDefaultAsync(x => x.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {

            if (id == null) return NotFound();
            HomeSliderBig _dbslider = await _remindb.HomeSliderBigs.FirstOrDefaultAsync(x => x.Id == id);
            if (_dbslider == null) return NotFound();
            _remindb.HomeSliderBigs.Remove(_dbslider);
            await _remindb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            HomeSliderBig _dbslider = await _remindb.HomeSliderBigs.FirstOrDefaultAsync(x => x.Id == id);
            if (_dbslider == null) return NotFound();
            return View(_dbslider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, HomeSliderBig homeSlider)
        {
            if (id == null) return NotFound();
            HomeSliderBig _dbslider = await _remindb.HomeSliderBigs.FirstOrDefaultAsync(x => x.Id == id);
            if (_dbslider == null) return NotFound();

            if (ModelState["Title"].ValidationState == ModelValidationState.Invalid ||
                ModelState["SubTitle"].ValidationState == ModelValidationState.Invalid ||
                ModelState["Link"].ValidationState == ModelValidationState.Invalid)
            {
                return RedirectToAction(nameof(Index));
            }




            if (homeSlider.ChangePhoto != null)
            {
                if (!homeSlider.ChangePhoto.IsImage())
                {
                    ModelState.AddModelError("ChangePhoto", "You can chose only image format");
                    return View();
                }

                if (!homeSlider.ChangePhoto.CheckSize(2))
                {
                    ModelState.AddModelError("ChangePhoto", "You can chose only small 2 MB");
                    return View();
                }


                string updateimage = await homeSlider.ChangePhoto.CopyImage(_env.WebRootPath, "homefirst");
                homeSlider.ImgPath = updateimage;
                DeleteImage.DeleteFromFolder(_env.WebRootPath, _dbslider.ImgPath);
            

            }

            _dbslider.Title = homeSlider.Title;
            _dbslider.SubTitle = homeSlider.SubTitle;
            _dbslider.Link = homeSlider.Link;
            _dbslider.ImgPath = homeSlider.ImgPath;




            await _remindb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}