using BookStore.Infrastructure;
using BookStore.Models.Db;
using BookStore.Models.Presenters;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BookStore.Models
{
    internal class ReserveBookModel
    {
        private DbContextOptions<StoreContext> options;
        private BookViewShow book;
        private Account account;
        private string description;
        public ReserveBookModel(DbContextOptions<StoreContext> options, BookViewShow book, Account account = null)
        {
            this.options = options;
            this.book = book;
            this.account = account;
        }
        public BookViewShow Book { get => book; }
        public string Description { get => description; set => description = value; }
        public string Message { get; set; }

        public event EventHandler<EventArgs> MessageChanged;

        private async Task OnMessageChanged(EventArgs e) => await MessageChanged?.InvokeAsync(this, e);

        public async Task AddReservedBook()
        {
            using (StoreContext db = new StoreContext(options))
            {
                BookInStore dbBook = await db.BookInStores.FindAsync(book.Id);
                if (dbBook is null || dbBook.Amount < 1) 
                {
                    Message = "Not found book or not free to sale";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return; 
                }
                db.BookReserves.Add(new BookReserve()
                {
                    BookInStoreId = book.Id,
                    Description = description,
                    AccountId = account?.Id ?? null,
                    DateReserve = DateTime.Now
                });
                await db.SaveChangesAsync();
            }
            Message = "Book reserved";
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
    }
}
