using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.Models
{
    public class NewUser:IdentityUser
    {
     
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool EmaileConfirm { get; set; }
        public virtual ICollection<Like> Likes  { get; set; }
        public virtual ICollection<Order> Orders{ get; set; }

    }
}
