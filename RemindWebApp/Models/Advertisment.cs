using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.Models
{
    public class Advertisment
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Required")]
        public IFormFile Photo { get; set; }

        [NotMapped]
        public IFormFile ChangePhoto { get; set; }
    }
}
