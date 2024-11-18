using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindWebApp.DAL;
using RemindWebApp.Models;
using RemindWebApp.ViewModels;

namespace RemindWebApp.Controllers
{
    
    public class ProductController : Controller
    {
        private const byte Take = 12;

        private RemindDatabase _reminddb;
        public int skip { get; set; }

        public ProductController(RemindDatabase remindbatabase)
        {
            _reminddb = remindbatabase;
        }

        public IActionResult Index(int? page)
        {
            if (page == null)
            {
                skip = 1;
            }
            else
            {
                skip = (page.Value - 1) * Take;

            }

            decimal count = _reminddb.Products.Count();
            ViewBag.PageCount = Math.Ceiling(count / Take);


            RemindViewModel remindproduct = new RemindViewModel()
            {
                CategoryMarkas = _reminddb.CategoryMarkas.Include(p => p.Products),
                Categories = _reminddb.Categories.Include(x => x.CategoryMarkas),
                Markas = _reminddb.Markas.Include(y => y.CategoryMarkas),

                Products = _reminddb.Products.Skip(skip).Take(Take).Include(i => i.Images).Include(l => l.Likes).Include(c => c.CategoryMarka.Category).Include(d => d.CategoryMarka.Marka).Include(od => od.OrderDetails),

                Images = _reminddb.Images,
                Likes = _reminddb.Likes,

                Orders = _reminddb.Orders.Include(odet => odet.OrderDetails),
                OrderDetails = _reminddb.OrderDetails,
                NewUsers = _reminddb.Users.Include(like => like.Likes).Include(order => order.Orders)
            };
            return View(remindproduct);
        }
        public async Task<IActionResult> Markas(int categoryId)
        {
            try
            {
                var markas = await _reminddb.CategoryMarkas.Where(m => m.CategoryId == categoryId).Select(m => new MarkaViewModel
                {
                    Id = m.Marka.Id,
                    Name = m.Marka.Name
                }).Distinct().ToListAsync();


                return Json(new { data = markas });
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<IActionResult> Search(int markaId, int categoryId)
        {
            try
            {
                var products = await _reminddb.Products.Include(m => m.Images).Where(m => m.CategoryMarka.CategoryId == categoryId && m.CategoryMarka.MarkaId == markaId).OrderByDescending(m => m.Id).Take(Take).ToListAsync();
                return PartialView("SearchResult", products);
            }
            catch (Exception)
            {
                throw;
            }
        }


    
        public async Task<IActionResult> SingleProduct(int? id, int? catid)
        {
            DetailProductViewModel detailView = new DetailProductViewModel
            {
                Products = _reminddb.Products.Include(i => i.Images).Include(cm => cm.CategoryMarka).Where(p => p.CategoryMarkaId == p.CategoryMarka.Id)
                .Include(d => d.CategoryMarka.Category).Where(c => c.CategoryMarka.CategoryId == c.CategoryMarka.Category.Id)
                .Include(a => a.CategoryMarka.Marka).Where(b => b.CategoryMarka.MarkaId == b.CategoryMarka.Marka.Id).Where(e => e.CategoryMarka.Category.Id == catid),

                Markas = _reminddb.Markas.Include(cm => cm.CategoryMarkas),
                CategoryMarkas = _reminddb.CategoryMarkas.Include(cm => cm.Products),
                DetailProduct = await _reminddb.Products.Include(i => i.Images).Include(c => c.CategoryMarka).Include(o => o.CategoryMarka.Category).Include(u => u.CategoryMarka.Marka).FirstOrDefaultAsync(c => c.Id == id),
                DetailCategory = await _reminddb.Categories.Include(a => a.CategoryMarkas).FirstOrDefaultAsync(c => c.Id == catid)


            };
            return View(detailView);

        }
    }
}