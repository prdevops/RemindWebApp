using RemindWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.ViewModels
{
    public class ChekoutViewModel
    {
        public List<Product> ProductsCheckout { get; set; }
        public List<int> ProductsCheckoutId { get; set; }
        
    }
}
