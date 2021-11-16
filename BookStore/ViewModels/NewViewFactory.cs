using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Models.Db;
using BookStore.Models.Presenters;
using BookStore.Views;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ViewModels
{
    internal class NewViewFactory: INewViewFactory<StoreContext>
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
        public bool? CreateAccountView(DbContextOptions<StoreContext> options, BookStore.Models.Presenters.AccountView account = null)
        {
            AccountModel model = new AccountModel(options, account);
            AccountViewModel modelView = new AccountViewModel(model, account is null ? false : true);
            BookStore.Views.AccountView view = new BookStore.Views.AccountView()
            {
                DataContext = modelView
            };
            return view.ShowDialog();
        }
        public bool? CreateStockView(DbContextOptions<StoreContext> options, BookStore.Models.Presenters.StockView stock = null)
        {
            StockModel model = new StockModel(options, stock);
            StockViewModel modelView = new StockViewModel(model, stock is null ? false : true);
            BookStore.Views.StockView view = new BookStore.Views.StockView()
            {
                DataContext = modelView
            };
            return view.ShowDialog();
        }
        public bool? CreateBookView(DbContextOptions<StoreContext> options, BookStore.Models.Presenters.BookView book = null)
        {
            BookModel model = new BookModel(options, book);
            BookViewModel modelView = new BookViewModel(model, book is null ? false : true);
            BookStore.Views.BookView view = new BookStore.Views.BookView()
            {
                DataContext = modelView
            };
            return view.ShowDialog();
        }
        public bool? CreateAuthorView(DbContextOptions<StoreContext> options, BookStore.Models.Presenters.AuthorView author = null)
        {
            AuthorModel model = new AuthorModel(options, author);
            AuthorViewModel modelView = new AuthorViewModel(model, author is null ? false : true);
            BookStore.Views.AuthorView view = new BookStore.Views.AuthorView()
            {
                DataContext = modelView
            };
            return view.ShowDialog();
        }
    }
}
