using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindWebApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.ViewComponents
{
   

    public class RemindHeaderViewComponent :ViewComponent
    {
        private RemindDatabase _remindb;

        public RemindHeaderViewComponent(RemindDatabase remindDatabase)
        {
            _remindb = remindDatabase;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var product = _remindb.Products
                .Include(x => x.CategoryMarka)
                .Include(o => o.CategoryMarka.Category).Where(c => c.CategoryMarka.CategoryId == c.CategoryMarka.Category.Id)
                .Include(a => a.CategoryMarka.Marka).Where(l => l.CategoryMarka.MarkaId == l.CategoryMarka.Marka.Id);
               
            return View(await Task.FromResult(product));
        }

        

    }
}
