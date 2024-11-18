using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RemindWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.DAL
{
    public class RemindDatabase :IdentityDbContext<NewUser>
    {
        public RemindDatabase(DbContextOptions<RemindDatabase> options) :base(options)
        {

        }
         public DbSet<Category> Categories { get; set; }
         public DbSet<Marka> Markas { get; set; }
         public DbSet<CategoryMarka> CategoryMarkas { get; set; }
         public DbSet<Product> Products { get; set; }
         public DbSet<Image> Images { get; set; }
         public DbSet<Like> Likes { get; set; }

         public DbSet<Order> Orders { get; set; }
         public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet <AboutContent> AboutContents { get; set; }
        public DbSet <Team> Teams { get; set; }
        public DbSet <AboutSlider> AboutSliders { get; set; }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Advertisment> Advertisments { get; set; }
        public DbSet<BlogNew> BlogNews { get; set; }
        public DbSet<HomeSliderBig> HomeSliderBigs { get; set; }

    }
}
