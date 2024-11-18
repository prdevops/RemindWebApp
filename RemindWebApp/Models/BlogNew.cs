using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.Models
{
    public class BlogNew
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required"), StringLength(300, ErrorMessage = "Length can't be more than 300")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Required"), StringLength(1000, ErrorMessage = "Length can't be more than 1000")]
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public string By { get; set; }

        [Required(ErrorMessage = "Required"), StringLength(300, ErrorMessage = "Length can't be more than 300")]
        public string ImagePath { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "Required")]
        public IFormFile Photo { get; set; }

        [NotMapped]
        public IFormFile ChangePhoto { get; set; }
    }
}
