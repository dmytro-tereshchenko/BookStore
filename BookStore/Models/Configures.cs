using BookStore.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Configures
    {
        private IConfigurationRoot config;
        public Configures(string jsonFile)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile(jsonFile);
            config = builder.Build();
        }
        public DbContextOptions<StoreContext> GetOptions(string nameConnectionString)
        {
            string connectionString = config.GetConnectionString(nameConnectionString);
            var optionsBuilder = new DbContextOptionsBuilder<StoreContext>();
            optionsBuilder.EnableSensitiveDataLogging();
            return optionsBuilder.UseSqlServer(connectionString).Options;
        }
        public string GetStringParameter(string key) => config[key];
    }
}
