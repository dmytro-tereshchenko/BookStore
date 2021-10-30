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
            ReserveBookViewModel modelView = new ReserveBookViewModel(model);
            ReserveBookView view = new ReserveBookView()
            {
                DataContext = modelView
            };
            view.Show();
        }
        public bool? CreateAccountView(DbContextOptions<StoreContext> options, BookStore.Models.AccountView account = null)
        {
            AccountModel model = new AccountModel(options, account);
            AccountViewModel modelView = new AccountViewModel(model, account is null ? false : true);
            BookStore.Views.AccountView view = new BookStore.Views.AccountView()
            {
                DataContext = modelView
            };
            return view.ShowDialog();
        }
    }
}
