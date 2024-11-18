using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.Models
{
    public class Image
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required"), StringLength(255, ErrorMessage = "Length can't be more than 255")]
        public string PathImage { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Required")]
        public IFormFile Photo { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string Active { get; set; }


    }
}
