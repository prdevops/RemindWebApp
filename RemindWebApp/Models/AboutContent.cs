using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.Models
{
    public class AboutContent
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required"), StringLength(300, ErrorMessage = "Length can't be more than 300")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Required"), StringLength(2000, ErrorMessage = "Length can't be more than 2000")]
        public string DescriptionFirst { get; set; }

        [Required(ErrorMessage = "Required"), StringLength(500, ErrorMessage = "Length can't be more than 500")]
        public string DescriptionSecond { get; set; }

        [Required(ErrorMessage = "Required"), StringLength(2000, ErrorMessage = "Length can't be more than 2000")]
        public string DescriptionThird { get; set; }

        [Required(ErrorMessage = "Required"), StringLength(350, ErrorMessage = "Length can't be more than 350")]
        public string Slogan { get; set; }

    }
}
