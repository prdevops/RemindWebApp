using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.Models
{
    public class CategoryMarka
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int MarkaId { get; set; }
        public virtual Marka Marka { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
