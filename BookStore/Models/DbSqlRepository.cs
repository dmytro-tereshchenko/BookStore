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
        private List<SimpleEntityView> resultSimpleEntities;
        private List<BookReservedView> resultBooksReserved;
        public DbSqlRepository(DbContextOptions<StoreContext> options)
        {
            this.options = options;
            currentUser = null;
            Task loadBd = new Task(() => { using (StoreContext db = new StoreContext(options)) { db.Accounts.ToList(); } }); //Load accounts to cashe 
            loadBd.Start();
        }
        public Account CurrentUser { get => currentUser; }
        public string LoginField { get => loginField; set => loginField = value; }
        public string TableName { get => tableName; set => tableName = value; }
        public List<BookView> ResultBooks { get => resultBooks; set => resultBooks = value; }
        public List<SimpleEntityView> ResultSimpleEntities { get => resultSimpleEntities; set => resultSimpleEntities = value; }
        public List<BookReservedView> ResultBooksReserved { get => resultBooksReserved; set => resultBooksReserved = value; }

        public event EventHandler<EventArgs> CurrentUserChanged;
        public event EventHandler<EventArgs> ResultBooksViewChanged;
        public event EventHandler<EventArgs> ResultSimpleEntitiesViewChanged;
        public event EventHandler<EventArgs> ResultBooksReservedViewChanged;

        private void OnCurrentUserChanged(EventArgs e) => CurrentUserChanged?.Invoke(this, e);
        private void OnResultBooksViewChanged(EventArgs e) => ResultBooksViewChanged?.Invoke(this, e);
        private void OnResultSimpleEntitiesViewChanged(EventArgs e) => ResultSimpleEntitiesViewChanged?.Invoke(this, e);
        private void OnResultBooksReservedViewChanged(EventArgs e) => ResultBooksReservedViewChanged?.Invoke(this, e);
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
            resultSimpleEntities = null;
            resultBooksReserved = null;
            TableName = "All books";
            OnResultBooksViewChanged(new PropertyChangedEventArgs(nameof(ResultBooks)));
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
            resultSimpleEntities = null;
            resultBooksReserved = null;
            TableName = "New books";
            OnResultBooksViewChanged(new PropertyChangedEventArgs(nameof(ResultBooks)));
        }
        public void BestSellingBooksView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultBooks = (from bst in db.BookSolds
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
                               .GroupBy(g => new
                               {
                                   g.Id,
                                   g.Name,
                                   g.Authors,
                                   g.Pages,
                                   g.YearOfPublished,
                                   g.Publisher,
                                   g.Genre,
                                   g.Series,
                                   g.Price
                               })
                               .Select(grp => new
                               {
                                   grp.Key.Id,
                                   grp.Key.Name,
                                   grp.Key.Authors,
                                   grp.Key.Pages,
                                   grp.Key.YearOfPublished,
                                   grp.Key.Publisher,
                                   grp.Key.Genre,
                                   grp.Key.Series,
                                   grp.Key.Price,
                                   BooksCount = grp.Count()
                               })
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
            resultSimpleEntities = null;
            resultBooksReserved = null;
            TableName = "Best selling books";
            OnResultBooksViewChanged(new PropertyChangedEventArgs(nameof(ResultBooks)));
        }
        public void MostPopularAuthorsView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultSimpleEntities = (from bst in db.BookSolds
                                        join bookInStore in db.BookInStores on bst.BookInStoreId equals bookInStore.Id
                                        join book in db.Books on bookInStore.BookId equals book.Id
                                        join bookAuthor in db.BookAuthors on book.Id equals bookAuthor.BookId
                                        join author in db.Authors on bookAuthor.AuthorId equals author.Id
                                        group author by new { author.Id, author.FirstName, author.MiddleName, author.LastName } into grp
                                        select new
                                        {
                                            grp.Key.Id,
                                            grp.Key.FirstName,
                                            grp.Key.MiddleName,
                                            grp.Key.LastName,
                                            CountBookSold = grp.Count()
                                        })
                               .OrderByDescending(g => g.CountBookSold)
                               .Take(5)
                               .Select(gr => new SimpleEntityView
                               {
                                   Id = gr.Id,
                                   Name = $"{gr.FirstName} {(gr.MiddleName == null ? "" : gr.MiddleName + " ")}{gr.LastName}"
                               })
                               .ToList();
            }
            resultBooksReserved = null;
            resultBooks = null;
            TableName = "Most popular authors";
            OnResultSimpleEntitiesViewChanged(new PropertyChangedEventArgs(nameof(ResultSimpleEntities)));
        }
        public void MostPopularGenresView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultSimpleEntities = (from bst in db.BookSolds
                                        join bookInStore in db.BookInStores on bst.BookInStoreId equals bookInStore.Id
                                        join book in db.Books on bookInStore.BookId equals book.Id
                                        join genre in db.Genres on book.GenreId equals genre.Id
                                        group genre by new { genre.Id, genre.Name } into grp
                                        select new
                                        {
                                            grp.Key.Id,
                                            grp.Key.Name,
                                            CountBookSold = grp.Count()
                                        })
                               .OrderByDescending(g => g.CountBookSold)
                               .Take(5)
                               .Select(gr => new SimpleEntityView
                               {
                                   Id = gr.Id,
                                   Name = gr.Name
                               })
                               .ToList();
            }
            resultBooksReserved = null;
            resultBooks = null;
            TableName = "Most popular genres";
            OnResultSimpleEntitiesViewChanged(new PropertyChangedEventArgs(nameof(ResultSimpleEntities)));
        }
        public void ReservedBooksView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultBooksReserved = (from reservedBook in db.BookReserves
                                       join bookInStore in db.BookInStores on reservedBook.BookInStoreId equals bookInStore.Id
                                       join book in db.Books on bookInStore.BookId equals book.Id
                                       join account in db.Accounts on reservedBook.AccountId equals account.Id into subTable //LEFT OUTHER JOIN Accounts
                                       from resultAcc in subTable.DefaultIfEmpty()
                                       join stock in (from s in db.Stocks //LEFT OUTHER JOIN Discounts
                                                      where s.DateStart <= DateTime.Now && s.DateEnd >= DateTime.Now
                                                      select s)
                                              on bookInStore.Id equals stock.BookInStoreId into subResult
                                       from subStock in subResult.DefaultIfEmpty()
                                       select new BookReservedView
                                       {
                                           Id = reservedBook.Id,
                                           BookName = book.Name,
                                           Authors = String.Join(", ", db.Authors.Join(db.BookAuthors,
                                                a => a.Id,
                                                ba => ba.AuthorId,
                                                (a, ba) => new
                                                {
                                                    AuthorId = a.Id,
                                                    AuthorName = a.FullName,
                                                    BookId = ba.BookId
                                                }).Where(c => c.BookId == book.Id).Select(d => d.AuthorName)),
                                           Description = reservedBook.Description,
                                           Price = (Math.Round(bookInStore.Price * (subStock == null ? 1 : (decimal)(100 - subStock.Discount) / 100) * 100) / 100).ToString("#0.00"),
                                           AccountLogin = (resultAcc == null ? "unknown" : resultAcc.Login),
                                           DateReserve = reservedBook.DateReserve.ToShortDateString()
                                       })
                                       .ToList();
            }
            resultSimpleEntities = null;
            resultBooks = null;
            TableName = "Reserved books";
            OnResultBooksReservedViewChanged(new PropertyChangedEventArgs(nameof(ResultBooksReserved)));
        }
        /*public static string GetAuthorsByBookId(DbContextOptions<StoreContext> options, int bookId)
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
        }*/
    }
}
