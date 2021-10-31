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
        public void CreateReserveBookView(DbContextOptions<StoreContext> options, BookViewShow book, Account account = null)
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
        public bool? CreateStockView(DbContextOptions<StoreContext> options, BookStore.Models.StockView stock = null)
        {
            StockModel model = new StockModel(options, stock);
            StockViewModel modelView = new StockViewModel(model, stock is null ? false : true);
            BookStore.Views.StockView view = new BookStore.Views.StockView()
            {
                DataContext = modelView
            };
            return view.ShowDialog();
        }
        public bool? CreateBookView(DbContextOptions<StoreContext> options, BookStore.Models.BookView book = null)
        {
            BookModel model = new BookModel(options, book);
            BookViewModel modelView = new BookViewModel(model, book is null ? false : true);
            BookStore.Views.BookView view = new BookStore.Views.BookView()
            {
                DataContext = modelView
            };
            return view.ShowDialog();
        }
    }
}
