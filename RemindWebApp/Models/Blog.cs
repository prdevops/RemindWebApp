using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.Models
{
    [Area("RemindWebApp")]
    public class Blog
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required"), StringLength(300, ErrorMessage = "Length can't be more than 300")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Required"), StringLength(300, ErrorMessage = "Length can't be more than 300")]
        public string Link { get; set; }

        public DateTime CreatedDate { get; set; }
        public string HeaderBig { get; set; }
        public string HeaderSmall { get; set; }

        [Required(ErrorMessage = "Required"), StringLength(300, ErrorMessage = "Length can't be more than 300")]

        public string ImagePath { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "Required")]
        public IFormFile Photo { get; set; }

        [NotMapped]
        public IFormFile ChangePhoto { get; set; }

    }
}
