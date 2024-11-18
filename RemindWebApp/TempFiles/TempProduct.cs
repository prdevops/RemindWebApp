using RemindWebApp.DAL;
using RemindWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.TempFiles
{
    public class TempProduct
    {
        private RemindDatabase _remindb;

        public TempProduct(RemindDatabase remind)
        {
            _remindb = remind;
        }
        public List<Product> GetProducts(List<int> Ids)
        {
            return _remindb.Products.Where(product => Ids.Contains(product.Id)).ToList();
        }
    }
}
