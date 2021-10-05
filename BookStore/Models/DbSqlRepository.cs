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
                               join bsb in db.BookSeriesBooks on book.Id equals bsb.BookId into subBsbTable
                               from subBsb in subBsbTable.DefaultIfEmpty()
                               join bs in db.BookSerieses on subBsb.BookSeriesId equals bs.Id into subBsTable
                               from subBs in subBsTable.DefaultIfEmpty()
                               join stock in (from s in db.Stocks
                                              where s.DateStart <= DateTime.Now && s.DateEnd >= DateTime.Now
                                              select s)
                                              on bookInStore.Id equals stock.BookInStoreId into subResult
                               from subStock in subResult.DefaultIfEmpty()
                               select new BookView
                               {
                                   Id = bookInStore.Id,
                                   Name = book.Name,
                                   Authors = DbSqlRepository.GetAuthorsByBookId(options, book.Id),
                                   Pages = book.Pages,
                                   YearOfPublished = book.YearOfPublished,
                                   Publisher = publisher.Name,
                                   Genre = genre.Name,
                                   Series = (subBsb == null || subBs == null ? "" : $"{subBs.Name} ({subBsb.Position} book)"),
                                   Price = (Math.Round(bookInStore.Price * (subStock == null ? 1 : (decimal)(100 - subStock.Discount) / 100) * 100) / 100).ToString("#0.00")
                               }).ToList();
            }
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
