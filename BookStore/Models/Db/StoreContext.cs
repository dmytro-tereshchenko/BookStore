using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.Db
{
    public class StoreContext:DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookSeries> BookSerieses { get; set; }
        public DbSet<BookSeriesBook> BookSeriesBooks { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
            /*Database.EnsureDeleted();
            Database.EnsureCreated();
            Database.Migrate();*/
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthor>().HasKey(u => new { u.BookId, u.AuthorId });
            modelBuilder.Entity<BookSeriesBook>().HasKey(u => new { u.BookId, u.BookSeriesId });
            modelBuilder.Entity<Book>(entity => entity
                        .HasCheckConstraint("CK_Book_Pages", "[Pages] >=0"));
            modelBuilder.Entity<Book>(entity => entity
                        .HasCheckConstraint("CK_Book_YearOfPublished", "[YearOfPublished] >=0 AND [YearOfPublished] <= YEAR(GETDATE())"));

        }
    }
}
