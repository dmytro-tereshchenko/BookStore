using BookStore.Infrastructure;
using BookStore.Models.Db;
using BookStore.Models.Presenters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BookStore.Interfaces
{
    internal interface IDbRepository<TContext> where TContext: DbContext
    {
        
        DbContextOptions<TContext> DbOptions { get; }
        Account CurrentUser { get; }
        RadioButtonRepository Period { get; }
        string LoginField { get; set; }
        string BookSearch { get; set; }
        string AuthorSearch { get; set; }
        string GenreSearch { get; set; }
        string Message { get; set; }
        TypeResultView TypeResultView { get; }
        string TableName { get; }
        IEnumerable<BookViewShow> ResultBooksView { get; set; }
        IEnumerable<SimpleEntityView> ResultSimpleEnitiesView { get; set; }
        IEnumerable<BookReservedView> ResultReservedBooksView { get; set; }
        IEnumerable<BookSoldView> ResultSoldBooksView { get; set; }
        IEnumerable<AccountView> ResultManageAccountsView { get; set; }
        IEnumerable<AuthorView> ResultManageAuthorsView { get; set; }
        IEnumerable<GenreView> ResultManageGenresView { get; set; }
        IEnumerable<PublisherView> ResultManagePublishersView { get; set; }
        IEnumerable<BookView> ResultManageBooksView { get; set; }
        IEnumerable<BookInStoreView> ResultManageBooksInStoreView { get; set; }
        IEnumerable<StockView> ResultManageStocksView { get; set; }
        IEnumerable<BookSeriesView> ResultManageBookSeriesView { get; set; }

        event EventHandler<EventArgs> CurrentUserChanged;
        event EventHandler<EventArgs> MessageChanged;
        event EventHandler<PropertyChangedEventArgs> ResultViewChanged;

        Task UserLogIn(string password);
        Task SearchBook();
        Task UserLogout();
        Task PeriodChanged();
        Task AllBooksView();
        Task NewBooksView();
        Task BestSellingBooksView();
        Task MostPopularAuthorsView();
        Task MostPopularGenresView();
        Task ReservedBooksView();
        Task SoldBooksView();
        Task ManageAccountsView();
        Task ManageAuthorsView();
        Task ManageGenresView();
        Task ManagePublishersView();
        Task ManageBooksView();
        Task ManageBooksInStoreView();
        Task ManageStocksView();
        Task ManageBookSeriesView();
        Task BuyBook(BookViewShow buyBook);
        Task DeleteAccount(AccountView account);
        Task DeleteAuthor(AuthorView author);
        Task DeleteGenre(GenreView genre);
        Task DeletePublisher(PublisherView publisher);
        Task DeleteBook(BookView book);
        Task DeleteBookInStore(BookInStoreView bookInStore);
        Task DeleteStock(StockView stock);
        Task DeleteBookSeries(BookSeriesView bookSeries);
    }
}
