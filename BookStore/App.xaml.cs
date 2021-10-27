using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Models.Db;
using BookStore.ViewModels;
using BookStore.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BookStore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Configures config = new Configures("appsettings.json");
            DbContextOptions<StoreContext> options = config.GetOptions("DefaultConnection");
            DbSqlRepository repository = new DbSqlRepository(options, new string[] { "Day", "Week", "Month", "Year" });
            NewViewFactory factory = new NewViewFactory();
            IViewModel viewModel = new MainViewModel(repository, factory);
            var view = new MainView(viewModel);
            view.Show();
        }
    }
}
