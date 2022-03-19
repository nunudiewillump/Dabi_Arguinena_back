
using Dabi_Arguinena_back.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dabi_Arguinena_back.Context
{
    public class BlogContext : DbContext
    {
        public DbSet<Products> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }


        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            modelbuilder.Entity<Products>().HasData(
                new Products { Id = 1, Name = "Rascador de espalda", Description = "mano magica para rascarse la espalda", CategoryId = 1, Price = 40}
                


                );
            modelbuilder.Entity<Category>().HasData(
               new Category { Id = 1, CategoryName = "Espalda", Description = "Articulos para la espalda"}



               );
        }
    }
}
