using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookStore.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal partial class MainView : Window
    {
        public MainView(IViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            /*Configures config = new Configures("appsettings.json");
            DbContextOptions<StoreContext> options = config.GetOptions("DefaultConnection");
            using (StoreContext db = new StoreContext(options))
            {

            }*/

        }
    }
    
}
