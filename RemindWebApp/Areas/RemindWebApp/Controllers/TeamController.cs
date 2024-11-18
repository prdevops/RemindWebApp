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
    public class TeamController : Controller
    {
        private RemindDatabase _remindb;
        private IHostingEnvironment _env;
        public TeamController(RemindDatabase remindDatabase, IHostingEnvironment hostingEnvironment)
        {
            _remindb = remindDatabase;
            _env = hostingEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Team> products = _remindb.Teams;


            return View(products);
        }
        public async Task<IActionResult>  Detail(int? id)
        {
            if (id == null) return NotFound();
            Team team = await _remindb.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (team == null) return NotFound();
            return View(team);
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Team createTeam)
        {

            if (ModelState["Name"].ValidationState == ModelValidationState.Invalid ||
                ModelState["Job"].ValidationState == ModelValidationState.Invalid ||
                ModelState["About"].ValidationState == ModelValidationState.Invalid)
            {
                return RedirectToAction(nameof(Index));
            }

            if (createTeam.Photo != null)
            {
                if (!createTeam.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "You can chose only image format");
                    return View();
                }

                if (!createTeam.Photo.CheckSize(2))
                {
                    ModelState.AddModelError("Photo", "You can chose only small 2 MB");
                    return View();
                }


                string createdImage = await createTeam.Photo.CopyImage(_env.WebRootPath, "team");
                #region
                Team newteam = new Team()
                {
                    Name = createTeam.Name,
                Job = createTeam.Job,
                About = createTeam.About

                };
                #endregion


                newteam.ImagePath = createdImage;

                // return Content($"{newproduct.Image}");
                await _remindb.Teams.AddAsync(newteam);


            }

            await _remindb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Team team = await _remindb.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (team == null) return NotFound();
            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {

            if (id == null) return NotFound();
            Team team = await _remindb.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (team == null) return NotFound();
           

            _remindb.Teams.Remove(team);
            await _remindb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            Team team = await _remindb.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (team == null) return NotFound();
            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, Team upteanm)
        {
            if (id == null) return NotFound();
            Team teamdb = await _remindb.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (teamdb == null) return NotFound();


            if (ModelState["Name"].ValidationState == ModelValidationState.Invalid ||
                ModelState["Job"].ValidationState == ModelValidationState.Invalid ||
                ModelState["About"].ValidationState == ModelValidationState.Invalid)
            {
                return RedirectToAction(nameof(Index));
            }




            if (upteanm.ChangePhoto != null)
            {
                if (!upteanm.ChangePhoto.IsImage())
                {
                    ModelState.AddModelError("ChangePhoto", "You can chose only image format");
                    return View();
                }

                if (!upteanm.ChangePhoto.CheckSize(2))
                {
                    ModelState.AddModelError("ChangePhoto", "You can chose only small 2 MB");
                    return View();
                }
              

                string updateimage = await upteanm.ChangePhoto.CopyImage(_env.WebRootPath, "team");
                upteanm.ImagePath = updateimage;
                DeleteImage.DeleteFromFolder(_env.WebRootPath, teamdb.ImagePath);
              


            }

            teamdb.Name = upteanm.Name;
            teamdb.Job = upteanm.Job;
            teamdb.About = upteanm.About;
            teamdb.ImagePath = upteanm.ImagePath;



            await _remindb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}