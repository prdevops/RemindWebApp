using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindWebApp.DAL;
using RemindWebApp.Models;
using RemindWebApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RemindWebApp.Controllers
{
    
    public class HomeController : Controller
    {
        private RemindDatabase _reminddb;
        private UserManager<NewUser> _userManager;
        public HomeController(RemindDatabase remindDatabase, UserManager<NewUser> userManager)
        {
            _userManager = userManager;
            _reminddb = remindDatabase;
           

        }


        public IActionResult Index()
        {
            var basketProductCookie = Request.Cookies["InCard"];
            ViewBag.Cookie = basketProductCookie;

            Dictionary<Category, IEnumerable<Product>> categoryByProducts = new Dictionary<Category, IEnumerable<Product>>();

            foreach (var category in _reminddb.Categories)
            {
                var categorymarkas = _reminddb.CategoryMarkas.Include(x => x.Category)
                    .Include(x => x.Products)
                    .Include("Products.Images")
                    .Where(x => x.CategoryId == category.Id);
                var products = new List<Product>();
                var categoryy = new Category();
                foreach (var item in categorymarkas)
                {
                    products.AddRange(item.Products);
                    categoryy = item.Category;
                }
                categoryByProducts.Add(categoryy, products);
            }

            //List<Product> ProductToCategory = _reminddb.Products.Include(x => x.CategoryMarka).Include(y => y.CategoryMarka.Marka).Include(i => i.CategoryMarka.Category)
            //  .Include(m => m.Images).Where(pr => (CategoryId == null || pr.CategoryMarka.CategoryId == categoryid) &&
            //  (productid == null || pr.Id == productid)).OrderByDescending(m => m.Id).Take(_take).ToList();


            RemindViewModel remindproduct = new RemindViewModel()
            {
                CategoryByProducts = categoryByProducts,
                CategoryMarkas = _reminddb.CategoryMarkas.Include(p => p.Products).
                 Include(m => m.Category).Where(x => x.CategoryId == x.Category.Id),
                Categories = _reminddb.Categories
                .Include(x => x.CategoryMarkas)
                .Include("CategoryMarkas.Products")
                .Include("CategoryMarkas.Products.Images"),
                Markas = _reminddb.Markas.Include(y => y.CategoryMarkas),

                Products = _reminddb.Products
                .Include(i => i.Images)
                .Include(l => l.Likes)
                .Include(j => j.CategoryMarka)
                .Include(c => c.CategoryMarka.Category)
                .Include(d => d.CategoryMarka.Marka)
                .Include(od => od.OrderDetails),
                Images = _reminddb.Images.Include(i => i.Product),
                Likes = _reminddb.Likes,

                Orders = _reminddb.Orders.Include(odet => odet.OrderDetails),
                OrderDetails = _reminddb.OrderDetails,

                NewUsers = _reminddb.Users.Include(like => like.Likes).Include(order => order.Orders),
                HomeSliderBigs = _reminddb.HomeSliderBigs
               
            };

            return View(remindproduct);


        }



        public IActionResult SearchProduct(string str)
        {
            var model = _reminddb.Products.Include(x => x.CategoryMarka)
                .Include(o => o.CategoryMarka.Category).Where(c => c.CategoryMarka.CategoryId == c.CategoryMarka.Category.Id)
                .Include(a => a.CategoryMarka.Marka).Where(l => l.CategoryMarka.MarkaId == l.CategoryMarka.Marka.Id)
                .Where(k => k.Name.StartsWith(str) || k.CategoryMarka.Category.Name.StartsWith(str));

            return PartialView("_SearchingPartial", model);

        }
        #region Olmadi Yar User User.Id
        //private Task<NewUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        //NewUser user = await GetCurrentUserAsync();
        //var userId = user?.Id;
        //string mail = user?.Email;
        #endregion

        private async Task<NewUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }


        public async Task<IActionResult> AddLike(int id)
        {
            Product product = await _reminddb.Products.FirstOrDefaultAsync(h => h.Id == id);
            // var myuser = await _userManager.FindByNameAsync(User.Identity.Name);
            NewUser user = await GetCurrentUserAsync();
            if (user.Id == null || user == null)
            {
                return RedirectToAction(nameof(Index));
            }


            Like like = new Like()
            {
                LikeCount = 1,
                NewUserId = user.Id,
                ProductId = product.Id

            };

            await _reminddb.Likes.AddAsync(like);
            await _reminddb.SaveChangesAsync();


            return View();
        }

        public async Task<IActionResult> Removelike(int id)
        {
            Product product = await _reminddb.Products.FirstOrDefaultAsync(h => h.Id == id);
            NewUser user = await GetCurrentUserAsync();
            if (user.Id == null || user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            Like like = await _reminddb.Likes.FirstOrDefaultAsync(l => l.ProductId == product.Id && l.NewUserId == user.Id);
            _reminddb.Likes.Remove(like);
            await _reminddb.SaveChangesAsync();
            return View();
        }




        #region 2- 3-ci variant Identity-de User-i tapmaq 
        //public async Task<IActionResult> Getid()//2-ci variant Identity-de User-i tapmaq ucun Sadece Useri globala cixartmaq lazimdir.
        //{
        //    var userId = _userManager.GetUserId(User);
        //    NewUser currentUser = await _userManager.FindByIdAsync(userId);
        //    return View();
        //}
        //---------------------------------------------------------------
        //Ответ Джо будет работать нормально, но обратите внимание, 
        //3-cu variant
        //что есть и более простой метод, который непосредственно
        //принимает экземпляр ClaimsPrincipal и вызывает GetUserId внутренне:

        //var user = await _userManager.GetUserAsync(User);
        #endregion

    }
}
