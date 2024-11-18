using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime SaleDayToUser { get; set; }
        [ForeignKey("NewUser")]
        public string NewUserId { get; set; }
        public virtual NewUser NewUser { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        

    }
}
