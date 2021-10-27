using BookStore.Models;
using BookStore.Models.Db;
using BookStore.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    internal class NewViewFactory
    {
        public void CreateReserveBookView(DbContextOptions<StoreContext> options, BookView book, Account account = null)
        {
            ReserveBookModel model = new ReserveBookModel(options, book, account);
            ReserveBookModelView modelView = new ReserveBookModelView(model);
            ReserveBookView view = new ReserveBookView()
            {
                DataContext = modelView
            };
            view.Show();
        }
    }
}
