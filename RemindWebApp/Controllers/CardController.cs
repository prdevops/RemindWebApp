using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindWebApp.DAL;
using RemindWebApp.Models;
using RemindWebApp.TempFiles;
using RemindWebApp.ViewModels;

namespace RemindWebApp.Controllers
{

    public class CardController : Controller
    {
        private RemindDatabase _remindb;


        public CardController(RemindDatabase remindDatabase)
        {
            _remindb = remindDatabase;
        }

        public IActionResult Checkout()
        {
            ChekoutViewModel model = new ChekoutViewModel();

            var basketProductCookie = Request.Cookies["InCard"];
            if (basketProductCookie != null)
            {
                var productIds = basketProductCookie;
                var addingproductIds = basketProductCookie.Split('-').Select(x => int.Parse(x)).ToList();
                model.ProductsCheckout = _remindb.Products.Include(m => m.Images).Where(pr => addingproductIds.Contains(pr.Id)).ToList();
                model.ProductsCheckoutId = addingproductIds;

            }

            return View(model);
        }

        public IActionResult AssideCard(int id)
        {

            var basketProductCookie = Request.Cookies["InCard"];

            var productIds = basketProductCookie;
            var addingproductIds = basketProductCookie.Split('-').Select(x => int.Parse(x)).ToList();

            var products = _remindb.Products.Include(m => m.Images)
           .Include(x => x.CategoryMarka)
           .Include(o => o.CategoryMarka.Category)
           .Include(a => a.CategoryMarka.Marka)
               .Where(pr => addingproductIds.Contains(pr.Id));

            return PartialView("_PartialAssideCard", products);
        }

        public IActionResult DyncAsside(int id)
        {

            var basketProductCookie = Request.Cookies["InCard"];

            var productIds = basketProductCookie;
            var addingproductIds = basketProductCookie.Split('-').Select(x => int.Parse(x)).ToList();

            var products = _remindb.Products.Include(m => m.Images)
               .Include(x => x.CategoryMarka)
               .Include(o => o.CategoryMarka.Category)
               .Include(a => a.CategoryMarka.Marka)
               .Where(pr => addingproductIds.Contains(pr.Id)).ToList();


            return PartialView("_PartialCard", products);

        }

        public IActionResult AssideTotal(int id)
        {

            var basketProductCookie = Request.Cookies["InCard"];

            var productIds = basketProductCookie;
            var addingproductIds = basketProductCookie.Split('-').Select(x => int.Parse(x)).ToList();

            var products = _remindb.Products.Include(m => m.Images)
               .Include(x => x.CategoryMarka)
               .Include(o => o.CategoryMarka.Category)
               .Include(a => a.CategoryMarka.Marka)
               .Where(pr => addingproductIds.Contains(pr.Id)).ToList();


            return PartialView("_PartialCalculateAsside", products);

        }


    }

}
