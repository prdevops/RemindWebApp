using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int Count { get; set; }
        public double Price { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
  

    }
}
