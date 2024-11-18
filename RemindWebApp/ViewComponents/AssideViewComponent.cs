using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindWebApp.DAL;
using RemindWebApp.Models;
using RemindWebApp.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.ViewComponents
{
    public class AssideViewComponent : ViewComponent
    {
        private RemindDatabase _remindbd;
        public AssideViewComponent(RemindDatabase remindDatabase)
        {
            _remindbd = remindDatabase;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
         

            var basketProductCookie = Request.Cookies["InCard"];

            if (basketProductCookie == null)
            {
                basketProductCookie = 0.ToString();
            }

            var productIds = basketProductCookie;
            var addingproductIds = basketProductCookie.Split('-').Select(x => int.Parse(x)).ToList();

            var products = _remindbd.Products.Include(m => m.Images).Where(pr => addingproductIds.Contains(pr.Id))
            .Include(x => x.CategoryMarka)
            .Include(o => o.CategoryMarka.Category).Where(c => c.CategoryMarka.CategoryId == c.CategoryMarka.Category.Id)
            .Include(a => a.CategoryMarka.Marka).Where(l => l.CategoryMarka.MarkaId == l.CategoryMarka.Marka.Id);

          
            return View(await Task.FromResult(products));
        }


    }
}
