using BookStore.Infrastructure;
using BookStore.Models.Db;
using BookStore.Models.Presenters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    internal class BookInStoreModel
    {
        private DbContextOptions<StoreContext> options;
        private BookInStoreView bookInStore;
        private string searchBook;
        private IEnumerable<BookView> books;
        private int bookId;
        public BookInStoreModel(DbContextOptions<StoreContext> options, BookInStoreView bookInStore = null)
        {
            this.options = options;
            this.bookInStore = bookInStore ?? new BookInStoreView();
        }
        public BookInStoreView BookInStore { get => bookInStore; }
        public string SearchBookName { get => searchBook; set => searchBook = value; }
        public string Message { get; set; }
        public IEnumerable<BookView> Books { get => books; set => books = value; }

        public event EventHandler<EventArgs> MessageChanged;
        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        private async Task OnMessageChanged(EventArgs e) => await MessageChanged?.InvokeAsync(this, e);
        private async Task OnPropertyChanged(PropertyChangedEventArgs e) => await PropertyChanged?.InvokeAsync(this, e);

        public async Task SearchBook()
        {
            using (StoreContext db = new StoreContext(options))
            {
                books = await (from book in db.Books
                               join genre in db.Genres on book.GenreId equals genre.Id
                               join publisher in db.Publishers on book.PublisherId equals publisher.Id
                               join bsb in db.BookSeriesBooks on book.Id equals bsb.BookId into subBsbTable //LEFT OUTHER JOIN BookSeries
                               from subBsb in subBsbTable.DefaultIfEmpty()
                               join bs in db.BookSerieses on subBsb.BookSeriesId equals bs.Id into subBsTable //LEFT OUTHER JOIN BookSeries
                               from subBs in subBsTable.DefaultIfEmpty()
                               where searchBook == null || EF.Functions.Like(book.Name, $"%{searchBook}%")
                               select new BookView
                               {
                                   Id = book.Id,
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
                                   Series = (subBsb == null || subBs == null ? "" : subBs.Name),
                                   SeriesPosition = (subBsb == null || subBs == null || subBsb.Position == null ? "" : subBsb.Position.ToString())
                               }).ToListAsync();
                await OnPropertyChanged(new PropertyChangedEventArgs(nameof(Books)));
            }
        }
        public async Task SetBook(BookView book)
        {
            this.bookInStore.Book = book.Name;
            bookId = book.Id;
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(BookInStore)));
        }
        public async Task AddBookInStore()
        {
            decimal costPrice;
            decimal price;
            if (decimal.TryParse(bookInStore.CostPrice, out costPrice) &&
                decimal.TryParse(bookInStore.Price, out price))
            {
                using (StoreContext db = new StoreContext(options))
                {
                    db.BookInStores.Add(new BookInStore()
                    {
                        BookId = bookId,
                        CostPrice = costPrice,
                        Price = price,
                        Amount = bookInStore.Amount,
                        DateAdded = DateTime.Now
                    });
                    await db.SaveChangesAsync();
                }
                Message = "Book in store created";
            }
            else
            {
                Message = "Wrong format of entering prices";
            }
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
        public async Task EditBookInStore()
        {
            decimal costPrice;
            decimal price;
            if (decimal.TryParse(bookInStore.CostPrice, out costPrice) &&
                decimal.TryParse(bookInStore.Price, out price))
            {
                using (StoreContext db = new StoreContext(options))
                {
                    BookInStore dbBookInStore = await db.BookInStores.FindAsync(bookInStore.Id);
                    if (dbBookInStore is null)
                    {
                        Message = "Not found book in store";
                        await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                        return;
                    }
                    dbBookInStore.BookId = bookInStore.BookId;
                    dbBookInStore.CostPrice = costPrice;
                    dbBookInStore.Price = price;
                    dbBookInStore.Amount = bookInStore.Amount;
                    db.Entry<BookInStore>(dbBookInStore).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                Message = "Book in store edited";
            }
            else
            {
                Message = "Wrong format of entering prices";
            }
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
    }
}
