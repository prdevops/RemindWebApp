using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int LikeCount { get; set; }
        [ForeignKey("NewUser")]
        public string NewUserId  { get; set; }
        public virtual NewUser NewUser { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
