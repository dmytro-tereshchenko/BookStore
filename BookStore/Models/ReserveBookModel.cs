using BookStore.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    internal class ReserveBookModel
    {
        private DbContextOptions<StoreContext> options;
        private BookView book;
        private Account account;
        private string description;
        public ReserveBookModel(DbContextOptions<StoreContext> options, BookView book, Account account = null)
        {
            this.options = options;
            this.book = book;
            this.account = account;
        }
        public BookView Book { get => book; }
        public string Description { get => description; set => description = value; }
        public void AddReservedBook()
        {
            using (StoreContext db = new StoreContext(options))
            {
                BookInStore dbBook = db.BookInStores.Find(book.Id);
                if (dbBook is null || dbBook.Amount < 1) return;
                db.BookReserves.Add(new BookReserve()
                {
                    BookInStoreId = book.Id,
                    Description = description,
                    AccountId = account?.Id ?? null,
                    DateReserve = DateTime.Now
                });
                db.SaveChanges();
            }
        }
    }
}
