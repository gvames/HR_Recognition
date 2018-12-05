using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchContext : IdentityDbContext<StoreUser>
    {


        public DutchContext(DbContextOptions<DutchContext> dutchContextOprions) : base(dutchContextOprions)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        //nu este necesar sa fie creata o proprietate <DbSet>  pentru <OrderItem>
        //<OrderItem> va fi accesat prin intermediul Orders folosind relatia parinte-copil

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Title);
                

            modelBuilder.Entity<Order>()
                .HasData(new Order()
                {
                    Id =1,
                    OrderDate = DateTime.Now,
                    OrderNumber = "12345"
                });

          
               
        }
    }
}
