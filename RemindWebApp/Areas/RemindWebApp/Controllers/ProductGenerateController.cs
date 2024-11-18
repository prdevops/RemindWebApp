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
using RemindWebApp.ViewModels;

namespace RemindWebApp.Areas.RemindWebApp.Controllers
{
    [Area("RemindWebApp")]
    public class ProductGenerateController : Controller
    {
        private RemindDatabase _remindb;

        private IHostingEnvironment _env;
        private Product product;


        public ProductGenerateController(RemindDatabase remindDatabase, IHostingEnvironment hostingEnvironment)
        {
            _remindb = remindDatabase;
            _env = hostingEnvironment;


        }


        public IActionResult Index()
        {
            ViewBag.Category = _remindb.Categories;
            ViewBag.Marka = _remindb.Markas;

            #region ProductControl LAZIMLIDI
            //ProductControl productControl = new ProductControl()
            //{

            //    CategoryMarkas = _remindb.CategoryMarkas.Include(p => p.Products)
            //     .Include(m => m.Category).Where(x => x.CategoryId == x.Category.Id)
            //     .Include(d =>d.Marka).Where(l =>l.MarkaId == l.Marka.Id),

            //    Categories = _remindb.Categories
            //    .Include(x => x.CategoryMarkas)
            //    .Include("CategoryMarkas.Products")//   strinqovoy Include
            //    .Include("CategoryMarkas.Products.Images"),
            //    Markas = _remindb.Markas.Include(y => y.CategoryMarkas),

            //    Products = _remindb.Products
            //    .Include(i => i.Images)
            //    .Include(l => l.Likes)
            //    .Include(j => j.CategoryMarka).Where(k =>k.CategoryMarkaId == k.CategoryMarka.Id)
            //    .Include(c => c.CategoryMarka.Category)
            //    .Include(d => d.CategoryMarka.Marka)
            //    .Include(od => od.OrderDetails),
            //     Images = _remindb.Images.Include(i => i.Product).Where(u =>u.ProductId == u.Product.Id),
            //     Likes = _remindb.Likes.Include(i =>i.NewUser).Where(e =>e.NewUserId == e.NewUser.Id)
            //    .Include(o =>o.Product).Where(t =>t.ProductId == t.Product.Id),

            //    Orders = _remindb.Orders.Include(odet => odet.OrderDetails),
            //    OrderDetails = _remindb.OrderDetails.Include(p =>p.Product).Where(p =>p.ProductId == p.Product.Id)
            //    .Include(o =>o.Order).Where(u =>u.OrderId == u.Order.Id),

            //    NewUsers = _remindb.Users.Include(like => like.Likes).Include(order => order.Orders)
            //};
            #endregion
            IEnumerable<Product> products = _remindb.Products
            .Include(i => i.Images)
            .Include(l => l.Likes)
            .Include(j => j.CategoryMarka).Where(k => k.CategoryMarkaId == k.CategoryMarka.Id)
            .Include(c => c.CategoryMarka.Category).Where(k => k.CategoryMarka.CategoryId == k.CategoryMarka.Category.Id)
            .Include(d => d.CategoryMarka.Marka).Where(k => k.CategoryMarka.MarkaId == k.CategoryMarka.Marka.Id)
            .Include(od => od.OrderDetails);



            return View(products);
        }


        public IActionResult Create()
        {
            ViewBag.Category = _remindb.Categories;
            ViewBag.Marka = _remindb.Markas;

            return View();
        }

        #region
        [HttpPost]
        public async Task<IActionResult> Create([Bind(include: "")]ProductDashoardViewModel createdashVM)
        {
            try
            {
                if (ModelState["Name"].ValidationState == ModelValidationState.Invalid ||
                  ModelState["Title"].ValidationState == ModelValidationState.Invalid ||
                  ModelState["Description"].ValidationState == ModelValidationState.Invalid ||
                  ModelState["Count"].ValidationState == ModelValidationState.Invalid ||
                  ModelState["Price"].ValidationState == ModelValidationState.Invalid ||
                  ModelState["DiscountProduct"].ValidationState == ModelValidationState.Invalid ||
                  ModelState["ProductDedline"].ValidationState == ModelValidationState.Invalid ||
                  ModelState["PhotoList"].ValidationState == ModelValidationState.Invalid)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (createdashVM.PhotoList.Count != 0)
                {
                    if (createdashVM.DiscountProduct == 0)
                    {
                        createdashVM.ProductDedline = null;
                    };
                    
                    #region   
                    product = new Product()
                    {
                        Name = createdashVM.Name,
                        Title = createdashVM.Title,
                        Description = createdashVM.Description,
                        Price = createdashVM.Price,
                        Count = createdashVM.Count,
                        DiscountProduct = createdashVM.DiscountProduct,
                        ProductDedline = createdashVM.ProductDedline,
                        CreatedDate = DateTime.Now,
                        MostView = createdashVM.MostView,
                        SellerCount = 0,
                        Active = null
                    };
                    #endregion
                    if (!_remindb.CategoryMarkas.Any(cm => cm.CategoryId == createdashVM.CategoryId && cm.MarkaId == createdashVM.MarkaId))
                    {
                        CategoryMarka categoryMarka = new CategoryMarka()
                        {
                            CategoryId = createdashVM.CategoryId,
                            MarkaId = createdashVM.MarkaId,
                        };
                        await _remindb.CategoryMarkas.AddAsync(categoryMarka);
                        await _remindb.SaveChangesAsync();

                        product.CategoryMarkaId = categoryMarka.Id;


                    }
                    else
                    {
                        product.CategoryMarkaId = _remindb.CategoryMarkas.FirstOrDefault(cm => cm.CategoryId == createdashVM.CategoryId && cm.MarkaId == createdashVM.MarkaId).Id;

                    };
                    _remindb.Products.Add(product);
                    _remindb.SaveChanges();

                    foreach (var generateimage in createdashVM.PhotoList)
                    {
                        if (!generateimage.IsImage())
                        {
                            ModelState.AddModelError("Photo", "You can chose only image format");
                            return View();
                        }
                        if (!generateimage.CheckSize(1))
                        {
                            ModelState.AddModelError("Photo", "You can chose only small 1 MB");
                            return View();
                        }
                        string createdImage = await generateimage.CopyImage(_env.WebRootPath, "product");
                        Image image = new Image()
                        {
                            PathImage = createdImage,
                            ProductId = product.Id,
                            Active = null,
                        };
                        await _remindb.Images.AddAsync(image);
                        await _remindb.SaveChangesAsync();

                    };




                }
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));

            }

            return RedirectToAction("Index", "ProductGenerate");

        }

        #endregion

        //Include -un DUZGUN VARIANTI 
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) { return NotFound(); }
            Product product = await _remindb.Products.Include(a => a.CategoryMarka).Where(y => y.CategoryMarkaId == y.CategoryMarka.Id)
                .Include(e => e.CategoryMarka.Category).Where(e => e.CategoryMarka.CategoryId == e.CategoryMarka.Category.Id)
                .Include(d => d.CategoryMarka.Marka).Where(r => r.CategoryMarka.MarkaId == r.CategoryMarka.Marka.Id)
                .Include(i => i.Images)
                .Include(h => h.Likes).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) { return NotFound(); }
            return View(product);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }
            Product product = await _remindb.Products.Include(a => a.CategoryMarka).Where(y => y.CategoryMarkaId == y.CategoryMarka.Id)
                .Include(e => e.CategoryMarka.Category).Where(e => e.CategoryMarka.CategoryId == e.CategoryMarka.Category.Id)
                .Include(d => d.CategoryMarka.Marka).Where(r => r.CategoryMarka.MarkaId == r.CategoryMarka.Marka.Id)
                .Include(i => i.Images)
                .Include(h => h.Likes).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) { return NotFound(); }
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) { return NotFound(); }
            Product product = await _remindb.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) { return NotFound(); }

            var image = _remindb.Images.Where(x => x.ProductId == id);

            foreach (var item in image)
            {
                DeleteImage.DeleteFromFolder(_env.WebRootPath, item.PathImage);
                _remindb.Images.Remove(item);
            }



            _remindb.Products.Remove(product);
            await _remindb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Category = _remindb.Categories;
            ViewBag.Marka = _remindb.Markas;
            if (id == null) { return NotFound(); }
            Product product = await _remindb.Products.Include(a => a.CategoryMarka).Where(y => y.CategoryMarkaId == y.CategoryMarka.Id)
             .Include(e => e.CategoryMarka.Category).Where(e => e.CategoryMarka.CategoryId == e.CategoryMarka.Category.Id)
             .Include(d => d.CategoryMarka.Marka).Where(r => r.CategoryMarka.MarkaId == r.CategoryMarka.Marka.Id)
             .Include(i => i.Images)
             .Include(h => h.Likes).FirstOrDefaultAsync(p => p.Id == id);



            if (product == null) { return NotFound(); }

            PoductUpdateViewModel pr = new PoductUpdateViewModel()
            {
                Name = product.Name,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Count = product.Count,
                DiscountProduct = product.DiscountProduct,
                ProductDedline = product.ProductDedline,
                CreatedDate = product.CreatedDate,
                MostView = product.MostView,
                Active = null,
                Images = product.Images,

            };
            return View(pr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind(include: "")] int id, PoductUpdateViewModel updateProductViewModel)
        {

            Product _dbproduct = await _remindb.Products.FirstOrDefaultAsync(p => p.Id == id);


            if (_dbproduct == null) { return NotFound(); }


            try
            {


                if (ModelState["Name"].ValidationState == ModelValidationState.Invalid ||
                  ModelState["Title"].ValidationState == ModelValidationState.Invalid ||
                  ModelState["Description"].ValidationState == ModelValidationState.Invalid ||
                  ModelState["Count"].ValidationState == ModelValidationState.Invalid ||
                  ModelState["Price"].ValidationState == ModelValidationState.Invalid ||
                  //ModelState["DiscountProduct"].ValidationState == ModelValidationState.Invalid ||
                  //ModelState["ProductDedline"].ValidationState == ModelValidationState.Invalid ||
                  ModelState["MostView"].ValidationState == ModelValidationState.Invalid)
                {
                    return RedirectToAction(nameof(Index));
                }



                if (updateProductViewModel.DiscountProduct == 0)
                {
                    updateProductViewModel.ProductDedline = null;
                };

                #region

                _dbproduct.Name = updateProductViewModel.Name;
                _dbproduct.Title = updateProductViewModel.Title;
                _dbproduct.Description = updateProductViewModel.Description;
                _dbproduct.Price = updateProductViewModel.Price;
                _dbproduct.Count = updateProductViewModel.Count;
                _dbproduct.DiscountProduct = updateProductViewModel.DiscountProduct;
                _dbproduct.ProductDedline = updateProductViewModel.ProductDedline;
                _dbproduct.MostView = updateProductViewModel.MostView;
                _dbproduct.Active = null;



                #endregion
                if (!_remindb.CategoryMarkas.Any(d => d.CategoryId == updateProductViewModel.CategoryId && d.MarkaId == updateProductViewModel.MarkaId))
                {
                    CategoryMarka categoryMarka = new CategoryMarka()
                    {
                        CategoryId = updateProductViewModel.CategoryId,
                        MarkaId = updateProductViewModel.MarkaId,
                    };
                    await _remindb.CategoryMarkas.AddAsync(categoryMarka);
                    await _remindb.SaveChangesAsync();

                    updateProductViewModel.CategoryMarkaId = categoryMarka.Id;
                    _dbproduct.CategoryMarkaId = updateProductViewModel.CategoryMarkaId;


                }
                else
                {
                    updateProductViewModel.CategoryMarkaId = _remindb.CategoryMarkas.FirstOrDefault(cm => cm.CategoryId == updateProductViewModel.CategoryId && cm.MarkaId == updateProductViewModel.MarkaId).Id;
                    _dbproduct.CategoryMarkaId = updateProductViewModel.CategoryMarkaId;
                };

                if (updateProductViewModel.ChangePhotoList != null) {
                    foreach (var generateimage in updateProductViewModel.ChangePhotoList)
                    {
                        if (!generateimage.IsImage())
                        {
                            ModelState.AddModelError("Photo", "You can chose only image format");
                            return View();
                        }
                        if (!generateimage.CheckSize(2))
                        {
                            ModelState.AddModelError("Photo", "You can chose only small 2 MB");
                            return View();
                        }
                        string createdImage = await generateimage.CopyImage(_env.WebRootPath, "product");
                        Image image = new Image()
                        {
                            PathImage = createdImage,
                            ProductId = _dbproduct.Id,// imageler id product saxlanaq;lll
                            Active = null,
                        };
                        await _remindb.Images.AddAsync(image);
                        await _remindb.SaveChangesAsync();


                    };
                }
           
                
                  

                await _remindb.SaveChangesAsync();



            }
            catch (Exception ex)
            {
                await _remindb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }


            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> DelImage(int id)
        {



            Image delete = await _remindb.Images.FirstOrDefaultAsync(x => x.Id == id);
            if (delete == null) { return NotFound(); }

            DeleteImage.DeleteFromFolder(_env.WebRootPath, delete.PathImage);
            _remindb.Images.Remove(delete);
            await _remindb.SaveChangesAsync();
            return Json(id);
        }
    }
}
