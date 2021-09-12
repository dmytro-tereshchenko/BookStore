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
            /*string connectionString = GetConnectionString("appsettings.json");
            var optionsBuilder = new DbContextOptionsBuilder<StoreContext>();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(connectionString);*/
            Configures config = new Configures("appsettings.json");

            /*return new StoreContext(optionsBuilder.Options);*/
            return new StoreContext(config.GetOptions("DefaultConnection"));
        }
        /*private string GetConnectionString(string jsonFile)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile(jsonFile);
            IConfigurationRoot config = builder.Build();
            return config.GetConnectionString("DefaultConnection");
        }*/
    }
}
