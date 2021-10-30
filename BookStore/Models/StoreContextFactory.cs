using BookStore.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class StoreContextFactory : IDesignTimeDbContextFactory<StoreContext>
    {
        public StoreContextFactory() { }
        public StoreContext CreateDbContext(string[] args)
        {
            Configures config = new Configures("appsettings.json");
            return new StoreContext(config.GetOptions("DefaultConnection"));
        }
    }
}
