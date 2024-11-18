using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RemindWebApp.Models
{
    public class Marka
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required"), StringLength(300, ErrorMessage = "Length can't be more than 300")]
        public string Name { get; set; }
        public string Active { get; set; }
        public virtual ICollection<CategoryMarka> CategoryMarkas { get; set; }
    
    }
}
