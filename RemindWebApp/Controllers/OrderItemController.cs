using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindWebApp.DAL;
using RemindWebApp.ViewModels;

namespace RemindWebApp.Controllers
{
    public class OrderItemController : Controller
    {
        private RemindDatabase _remindb;
        
        public OrderItemController(RemindDatabase remindDatabase)
        {
            _remindb = remindDatabase;
        }
     


        public IActionResult Payment()
        {
            OrderItemViewModel model = new OrderItemViewModel();
            var basketProductCookie = Request.Cookies["InCard"];
            if (basketProductCookie != null)
            {
                var productIds = basketProductCookie;
                var addingproductIds = basketProductCookie.Split('-').Select(x => int.Parse(x)).ToList();

                model.ProductsCheckout = _remindb.Products.Include(m => m.Images)
                  .Include(x => x.CategoryMarka)
                  .Include(o => o.CategoryMarka.Category)
                  .Include(a => a.CategoryMarka.Marka)
                  .Where(pr => addingproductIds.Contains(pr.Id)).ToList();
                model.ProductsCheckoutId = addingproductIds;
            }
            return View(model);
        }
    }
}