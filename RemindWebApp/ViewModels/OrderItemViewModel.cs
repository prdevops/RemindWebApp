using RemindWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.ViewModels
{
    public class OrderItemViewModel
    {
       
        public List<Product> ProductsCheckout { get; set; }
        public List<int> ProductsCheckoutId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Tel { get; set; }
        [Required]
        public string Address { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
      



    }
}
