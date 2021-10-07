using BookStore.Interfaces;
using BookStore.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    internal class DbSqlRepository
    {
        private DbContextOptions<StoreContext> options;
        private Account currentUser;
        private string loginField;
        private string tableName;
        private List<BookView> resultBooks;
        public DbSqlRepository(DbContextOptions<StoreContext> options)
        {
            this.options = options;
            currentUser = null;
            Task loadBd = new Task(() => { using (StoreContext db = new StoreContext(options)) { db.Accounts.ToList(); } });
            loadBd.Start();
        }
        public Account CurrentUser { get => currentUser; }
        public string LoginField { get => loginField; set => loginField = value; }
        public string TableName { get => tableName; set => tableName = value; }
        public List<BookView> ResultBooks { get => resultBooks; set => resultBooks = value; }

        public event EventHandler<EventArgs> CurrentUserChanged;
        public event EventHandler<EventArgs> ResultViewChanged;

        private void OnCurrentUserChanged(EventArgs e) => CurrentUserChanged?.Invoke(this, e);
        private void OnResultViewChanged(EventArgs e) => ResultViewChanged?.Invoke(this, e);
       /* private void OnCurrentUserChanged(EventArgs e)
        {
            var eventListeners = CurrentUserChanged.GetInvocationList();

            Console.WriteLine("Raising Event");
            for (int index = 0; index < eventListeners.Count(); index++)
            {
                var methodToInvoke = (EventHandler)eventListeners[index];
                methodToInvoke.BeginInvoke(this, e, EndAsyncEventCurrentUserChanged, null);
            }
            //CurrentUserChanged?.Invoke(this, e);
        }*/
        /*private void EndAsyncEventCurrentUserChanged(IAsyncResult iar)
        {
            var ar = (System.Runtime.Remoting.Messaging.AsyncResult)iar;
            var invokedMethod = (EventHandler)ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(iar);
            }
            catch
            {
                // Handle any exceptions that were thrown by the invoked method
                Console.WriteLine("An event listener went kaboom!");
            }
        }*/
        public void UserLogIn(string password)
        {
            using (StoreContext db = new StoreContext(options))
            {
                currentUser = db.Accounts.Where(ac => ac.Login == loginField && ac.Password == password).FirstOrDefault();
            }
            if (currentUser != null)
            {
                OnCurrentUserChanged(new PropertyChangedEventArgs(nameof(CurrentUser)));
            }
        }
        public void UserLogout()
        {
            currentUser = null;
            OnCurrentUserChanged(new PropertyChangedEventArgs(nameof(CurrentUser)));
        }
        public void AllBooksView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultBooks = (from bookInStore in db.BookInStores
                               join book in db.Books on bookInStore.BookId equals book.Id
                               join genre in db.Genres on book.GenreId equals genre.Id
                               join publisher in db.Publishers on book.PublisherId equals publisher.Id
                               join bsb in db.BookSeriesBooks on book.Id equals bsb.BookId into subBsbTable //LEFT OUTHER JOIN BookSeries
                               from subBsb in subBsbTable.DefaultIfEmpty()
                               join bs in db.BookSerieses on subBsb.BookSeriesId equals bs.Id into subBsTable //LEFT OUTHER JOIN BookSeries
                               from subBs in subBsTable.DefaultIfEmpty()
                               join stock in (from s in db.Stocks //LEFT OUTHER JOIN Discounts
                                              where s.DateStart <= DateTime.Now && s.DateEnd >= DateTime.Now
                                              select s)
                                              on bookInStore.Id equals stock.BookInStoreId into subResult
                               from subStock in subResult.DefaultIfEmpty()
                               where bookInStore.Amount - ((from reserve in db.BookReserves //Filter for free books
                                                            where bookInStore.Id == reserve.BookInStoreId
                                                            select reserve).Count()) > 0
                               select new BookView
                               {
                                   Id = bookInStore.Id,
                                   Name = book.Name,
                                   Authors = String.Join(", ", db.Authors.Join(db.BookAuthors,
                                                a => a.Id,
                                                ba => ba.AuthorId,
                                                (a, ba) => new
                                                {
                                                    AuthorId = a.Id,
                                                    AuthorName = a.FullName,
                                                    BookId = ba.BookId
                                                }).Where(c => c.BookId == book.Id).Select(d => d.AuthorName)),
                                   Pages = book.Pages,
                                   YearOfPublished = book.YearOfPublished,
                                   Publisher = publisher.Name,
                                   Genre = genre.Name,
                                   Series = (subBsb == null || subBs == null ? "" : $"{subBs.Name} ({subBsb.Position} book)"),
                                   Price = (Math.Round(bookInStore.Price * (subStock == null ? 1 : (decimal)(100 - subStock.Discount) / 100) * 100) / 100).ToString("#0.00")
                               }).ToList();
            }
            TableName = "All books";
            OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultBooks)));
        }
        public void NewBooksView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultBooks = (from bookInStore in db.BookInStores
                               join book in db.Books on bookInStore.BookId equals book.Id
                               join genre in db.Genres on book.GenreId equals genre.Id
                               join publisher in db.Publishers on book.PublisherId equals publisher.Id
                               join bsb in db.BookSeriesBooks on book.Id equals bsb.BookId into subBsbTable //LEFT OUTHER JOIN BookSeries
                               from subBsb in subBsbTable.DefaultIfEmpty()
                               join bs in db.BookSerieses on subBsb.BookSeriesId equals bs.Id into subBsTable //LEFT OUTHER JOIN BookSeries
                               from subBs in subBsTable.DefaultIfEmpty()
                               join stock in (from s in db.Stocks //LEFT OUTHER JOIN Discounts
                                              where s.DateStart <= DateTime.Now && s.DateEnd >= DateTime.Now
                                              select s)
                                              on bookInStore.Id equals stock.BookInStoreId into subResult
                               from subStock in subResult.DefaultIfEmpty()
                               where bookInStore.Amount - ((from reserve in db.BookReserves //Filter for free books
                                                            where bookInStore.Id == reserve.BookInStoreId
                                                            select reserve).Count()) > 0
                               where bookInStore.DateAdded > DateTime.Now.AddDays(-31)
                               select new BookView
                               {
                                   Id = bookInStore.Id,
                                   Name = book.Name,
                                   Authors = String.Join(", ", db.Authors.Join(db.BookAuthors,
                                                a => a.Id,
                                                ba => ba.AuthorId,
                                                (a, ba) => new
                                                {
                                                    AuthorId = a.Id,
                                                    AuthorName = a.FullName,
                                                    BookId = ba.BookId
                                                }).Where(c => c.BookId == book.Id).Select(d => d.AuthorName)),
                                   Pages = book.Pages,
                                   YearOfPublished = book.YearOfPublished,
                                   Publisher = publisher.Name,
                                   Genre = genre.Name,
                                   Series = (subBsb == null || subBs == null ? "" : $"{subBs.Name} ({subBsb.Position} book)"),
                                   Price = (Math.Round(bookInStore.Price * (subStock == null ? 1 : (decimal)(100 - subStock.Discount) / 100) * 100) / 100).ToString("#0.00")
                               }).ToList();
            }
            TableName = "New books";
            OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultBooks)));
        }
        public void BestSellingBooksView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                /*var bookSoldQuery = db.BookSolds.GroupBy(b => b.BookInStoreId)
                    .Select(b => new
                    {
                        BookInStoreId = b.Key,
                        booksCount = b.Count()
                    })
                    .OrderByDescending(b => b.booksCount)
                    .Take(5);

                resultBooks = (from bookSoldTop in bookSoldQuery*/
                resultBooks = (from bst in db.BookSolds
                                   /*group bst by bst.BookInStoreId into grp
                                   select new
                                   {
                                       BookInStoreId = grp.Key,
                                       booksCount = grp.Count()
                                   } into bookSoldQuery
                                   from bookSoldTop in bookSoldQuery*/
                                   /*orderby bookSoldTop.BooksCount descending*/ /*into bookSoldQuery2*/
                                   /*from bookSoldTop in bookSoldQuery2*/
                               join bookInStore in db.BookInStores on bst.BookInStoreId equals bookInStore.Id
                               join book in db.Books on bookInStore.BookId equals book.Id
                               join genre in db.Genres on book.GenreId equals genre.Id
                               join publisher in db.Publishers on book.PublisherId equals publisher.Id
                               join bookSold in db.BookSolds on bookInStore.Id equals bookSold.BookInStoreId
                               join bsb in db.BookSeriesBooks on book.Id equals bsb.BookId into subBsbTable //LEFT OUTHER JOIN BookSeries
                               from subBsb in subBsbTable.DefaultIfEmpty()
                               join bs in db.BookSerieses on subBsb.BookSeriesId equals bs.Id into subBsTable //LEFT OUTHER JOIN BookSeries
                               from subBs in subBsTable.DefaultIfEmpty()
                               select new BookView
                               {
                                   Id = bookInStore.Id,
                                   Name = book.Name,
                                   Authors = String.Join(", ", db.Authors.Join(db.BookAuthors,
                                                a => a.Id,
                                                ba => ba.AuthorId,
                                                (a, ba) => new
                                                {
                                                    AuthorId = a.Id,
                                                    AuthorName = a.FullName,
                                                    BookId = ba.BookId
                                                }).Where(c => c.BookId == book.Id).Select(d => d.AuthorName)),
                                   Pages = book.Pages,
                                   YearOfPublished = book.YearOfPublished,
                                   Publisher = publisher.Name,
                                   Genre = genre.Name,
                                   Series = (subBsb == null || subBs == null ? "" : $"{subBs.Name} ({subBsb.Position} book)"),
                                   Price = (Math.Round((db.BookSolds.Where(b => b.BookInStoreId == bst.BookInStoreId)
                                                                    .Average(b => b.SoldPrice)
                                                        ) * 100) / 100).ToString("#0.00")
                               }).ToList()
                               .GroupBy(g => new { g.Id, g.Name, g.Authors, g.Pages, g.YearOfPublished, g.Publisher, g.Genre, g.Series, g.Price })
                               .Select(grp => new { grp.Key.Id, grp.Key.Name, grp.Key.Authors, grp.Key.Pages, grp.Key.YearOfPublished, grp.Key.Publisher, grp.Key.Genre, grp.Key.Series, grp.Key.Price, BooksCount = grp.Count() })
                               .OrderByDescending(b => b.BooksCount)
                               .Take(5)
                               .Select(r => new BookView
                               {
                                   Id = r.Id,
                                   Name = r.Name,
                                   Authors = r.Authors,
                                   Pages = r.Pages,
                                   YearOfPublished = r.YearOfPublished,
                                   Publisher = r.Publisher,
                                   Genre = r.Genre,
                                   Series = r.Series,
                                   Price = r.Price
                               })
                               .ToList();
            }
            TableName = "Best selling books";
            OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultBooks)));
        }
        public static string GetAuthorsByBookId(DbContextOptions<StoreContext> options, int bookId)
        {
            using (StoreContext db = new StoreContext(options))
            {
                var query = db.Authors.Join(db.BookAuthors,
                a => a.Id,
                ba => ba.AuthorId,
                (a, ba) => new
                {
                    AuthorId = a.Id,
                    AuthorName = a.FullName,
                    BookId = ba.BookId
                }).Where(c => c.BookId == bookId).Select(d => d.AuthorName);
                return String.Join(", ", query);
            }
        }
    }
}
