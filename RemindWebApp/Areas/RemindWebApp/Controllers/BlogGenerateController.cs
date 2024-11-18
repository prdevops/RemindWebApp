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
    public class BlogGenerateController : Controller
    {
        
        private RemindDatabase _remindb;
        private IHostingEnvironment _env;

        public BlogGenerateController(RemindDatabase remindDatabase, IHostingEnvironment hostingEnvironment)
        {
            _remindb = remindDatabase;
            _env = hostingEnvironment;
        }

        public IActionResult Index()
        {

            return View(_remindb.Blogs);

        }
        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null) return NotFound();
            Blog blog = await _remindb.Blogs
                .FirstOrDefaultAsync(x => x.Id == id);

            if (blog == null) return NotFound();
            return View(blog);
        }
        public IActionResult Create()
        {

   
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Blog blog)
        {

            if( ModelState["Title"].ValidationState == ModelValidationState.Invalid ||
                ModelState["Photo"].ValidationState == ModelValidationState.Invalid)
            {
                return RedirectToAction(nameof(Index));
            }

            if (blog.Photo != null)
            {
                if (!blog.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "You can chose only image format");
                    return View();
                }

                if (!blog.Photo.CheckSize(2))
                {
                    ModelState.AddModelError("Photo", "You can chose only small 2 MB");
                    return View();
                }

               
                string createdImage = await blog.Photo.CopyImage(_env.WebRootPath, "blog");
                #region
                Blog newblog = new Blog()
                {
                    Title = blog.Title,
                    Link = blog.Link,
                    HeaderBig = blog.HeaderBig,
                    HeaderSmall = blog.HeaderSmall,
                    CreatedDate = DateTime.Now
                    
                };
                #endregion


                newblog.ImagePath = createdImage;

                // return Content($"{newproduct.Image}");
                await _remindb.Blogs.AddAsync(newblog);


            }

            await _remindb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Blog blogdel = await _remindb.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (blogdel == null) return NotFound();
            return View(blogdel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {

            if (id == null) return NotFound();
            Blog blogdel = await _remindb.Blogs.FirstOrDefaultAsync(o =>o.Id == id);
            if (blogdel == null) return NotFound();

            _remindb.Blogs.Remove(blogdel);
            await _remindb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            Blog blogUpdate = await _remindb.Blogs.FirstOrDefaultAsync(o => o.Id == id);
            if (blogUpdate == null) return NotFound();

            return View(blogUpdate);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, Blog updateblog)
        {
            if (id == null) return NotFound();
            Blog _dbblog = await _remindb.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (_dbblog == null) return NotFound();


            if (ModelState["Title"].ValidationState == ModelValidationState.Invalid ||
                           ModelState["Photo"].ValidationState == ModelValidationState.Invalid)
            {
                return RedirectToAction(nameof(Index));
            }




            if (updateblog.ChangePhoto != null)
            {
                if (!updateblog.ChangePhoto.IsImage())
                {
                    ModelState.AddModelError("ChangePhoto", "You can chose only image format");
                    return View();
                }

                if (!updateblog.ChangePhoto.CheckSize(2))
                {
                    ModelState.AddModelError("ChangePhoto", "You can chose only small 2 MB");
                    return View();
                }

               
                string updateimage = await updateblog.ChangePhoto.CopyImage(_env.WebRootPath, "blog");
                updateblog.ImagePath = updateimage;
               DeleteImage.DeleteFromFolder(_env.WebRootPath, _dbblog.ImagePath);
                


            }

            _dbblog.Title = updateblog.Title;
            _dbblog.Link = updateblog.Link;
            _dbblog.HeaderBig = updateblog.HeaderBig;
            _dbblog.HeaderSmall = updateblog.HeaderSmall;
            _dbblog.ImagePath = updateblog.ImagePath;
            

            await _remindb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }







    }
}