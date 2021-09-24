using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop
{
    public class WsDBContext: DbContext
    {
        public WsDBContext(DbContextOptions<WsDBContext> options) : base(options) 
        {

        }
        public DbSet<ProductModel> Products { get; set; }

        public DbSet<Settings> Settings { get; set; }
    }
}
