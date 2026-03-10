using BookStore.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
                
        }
       

        public DbSet<Book> books { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Cart> carts { get; set; }
    }
}
