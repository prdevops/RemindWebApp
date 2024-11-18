using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.Models
{
    public class HomeSliderBig
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required"), StringLength(300, ErrorMessage = "Length can't be more than 300")]
        public string ImgPath { get; set; }
        [Required(ErrorMessage = "Required"), StringLength(300, ErrorMessage = "Length can't be more than 300")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Required"), StringLength(300, ErrorMessage = "Length can't be more than 300")]
        public string SubTitle { get; set; }
        [Required(ErrorMessage = "Required"), StringLength(300, ErrorMessage = "Length can't be more than 300")]
        public string Link { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Required")]
        public IFormFile Photo { get; set; }

        [NotMapped]
        public IFormFile ChangePhoto { get; set; }

    }
}
