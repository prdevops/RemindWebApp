using RemindWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.ViewModels
{
    public class DetailProductViewModel
    {
        public Category  DetailCategory { get; set; }
        public Product DetailProduct { get; set; }
        public IEnumerable<Marka> Markas { get; set; }
        public IEnumerable<CategoryMarka> CategoryMarkas { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public IEnumerable<Like> Likes { get; set; }
        public IEnumerable<Order> Orders { get; set; }  
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
        public IEnumerable<NewUser> NewUsers { get; set; }
    }
}
 