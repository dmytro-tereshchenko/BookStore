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
        public DbSet<BookInStore> BookInStores { get; set; }
        public DbSet<BookReserve> BookReserves { get; set; }
        public DbSet<BookSeries> BookSerieses { get; set; }
        public DbSet<BookSeriesBook> BookSeriesBooks { get; set; }
        public DbSet<BookSold> BookSolds { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
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
                        .HasCheckConstraint("CK_Book_YearOfPublished", "[YearOfPublished] >= 0 AND [YearOfPublished] <= YEAR(GETDATE())"));
            modelBuilder.Entity<BookInStore>(entity => entity
                       .HasCheckConstraint("CK_BookInStore_CostPrice", "[CostPrice] >= 0"));
            modelBuilder.Entity<BookInStore>(entity => entity
                       .HasCheckConstraint("CK_BookInStore_Price", "[Price] >= 0"));
            modelBuilder.Entity<BookInStore>(entity => entity
                       .HasCheckConstraint("CK_BookInStore_Amount", "[Amount] >= 0"));
            modelBuilder.Entity<Stock>(entity => entity
                        .HasCheckConstraint("CK_Stock_Discount", "[Discount] > 0 AND [Discount] <= 100.0"));
            modelBuilder.Entity<BookReserve>().Property(u => u.DateReserve).HasDefaultValueSql<DateTime>("getdate()");
            modelBuilder.Entity<BookSold>().Property(u => u.DateSold).HasDefaultValueSql<DateTime>("getdate()");
            modelBuilder.Entity<BookInStore>().Property(u => u.DateAdded).HasDefaultValueSql<DateTime>("getdate()");
            modelBuilder.Entity<Account>().Property(u => u.Admin).HasDefaultValue<bool>(false);

            //////////////////////////////////////////////////////////////////////////////

            //Add data to database for testing app.

            //////////////////////////////////////////////////////////////////////////////

            Account[] accounts = new Account[]
            {
                new Account(){ Id=1, Login="admin", Password="admin", Admin=true},
                new Account(){ Id=2, Login="seller1", Password="seller1", Admin=false}
            };
            Author[] authors = new Author[]
            {
                new Author(){ Id=1, FirstName="Marcel", LastName="Proust"},
                new Author(){ Id=2, FirstName="James", LastName="Joyce"},
                new Author(){ Id=3, FirstName="Miguel", LastName="de Cervantes"},
                new Author(){ Id=4, FirstName="Gabriel", MiddleName="García", LastName="Márquez"},
                new Author(){ Id=5, FirstName="Francis", MiddleName="Scott", LastName="Fitzgerald"},
                new Author(){ Id=6, FirstName="John", MiddleName="Ronald Reuel", LastName="Tolkien"}
            };
            Genre[] genres = new Genre[]
            {
                new Genre(){ Id=1, Name="Modernist" },
                new Genre(){ Id=2, Name="Novel" },
                new Genre(){ Id=3, Name="Magic realism" },
                new Genre(){ Id=4, Name="Tragedy" },
                new Genre(){ Id=5, Name="Fantasy" }
            };
            Publisher[] publishers = new Publisher[]
            {
                new Publisher(){ Id=1, Name="Grasset and Gallimard"},
                new Publisher(){ Id=2, Name="Shakespeare and Company"},
                new Publisher(){ Id=3, Name="Francisco de Robles"},
                new Publisher(){ Id=4, Name="Editorial"},
                new Publisher(){ Id=5, Name="Charles Scribner's Sons"},
                new Publisher(){ Id=6, Name="George Allen & Unwin"}
            };
            Book[] books = new Book[]
            {
                new Book(){ Id=1,
                    GenreId=1,
                    PublisherId=1,
                    Name="In Search of Lost Time",
                    Pages=4215,
                    YearOfPublished=1922 },
                new Book(){ Id=2,
                    GenreId=1,
                    PublisherId=2,
                    Name="Ulysses",
                    Pages=730,
                    YearOfPublished=1922 },
                new Book(){ Id=3,
                    GenreId=2,
                    PublisherId=3,
                    Name="Don Quixote",
                    Pages=845,
                    YearOfPublished=1620 },
                new Book(){ Id=4,
                    GenreId=3,
                    PublisherId=4,
                    Name="One Hundred Years of Solitude",
                    Pages=624,
                    YearOfPublished=1970 },
                new Book(){ Id=5,
                    GenreId=4,
                    PublisherId=5,
                    Name="The Great Gatsby",
                    Pages=749,
                    YearOfPublished=1925 },
                new Book(){ Id=6,
                    GenreId=5,
                    PublisherId=6,
                    Name="The Fellowship of the Ring",
                    Pages=423,
                    YearOfPublished=1954 },
                new Book(){ Id=7,
                    GenreId=5,
                    PublisherId=6,
                    Name="The Two Towers",
                    Pages=352,
                    YearOfPublished=1954 },
                new Book(){ Id=8,
                    GenreId=5,
                    PublisherId=6,
                    Name="The Return of the King",
                    Pages=416,
                    YearOfPublished=1955 }

            };
            BookAuthor[] bookAuthors = new BookAuthor[]
            {
                new BookAuthor(){ AuthorId=1, BookId=1},
                new BookAuthor(){ AuthorId=2, BookId=2},
                new BookAuthor(){ AuthorId=3, BookId=3},
                new BookAuthor(){ AuthorId=4, BookId=4},
                new BookAuthor(){ AuthorId=5, BookId=5},
                new BookAuthor(){ AuthorId=6, BookId=6},
                new BookAuthor(){ AuthorId=6, BookId=7},
                new BookAuthor(){ AuthorId=6, BookId=8}
            };
            BookInStore[] bookInStores = new BookInStore[]
            {
                new BookInStore(){
                    Id=1,
                    BookId=1,
                    Amount=5,
                    CostPrice=254.2m,
                    Price=350m,
                    DateAdded=DateTime.Now.AddYears(-3)
                },
                new BookInStore(){
                    Id=2,
                    BookId=2,
                    Amount=12,
                    CostPrice=324.7m,
                    Price=400m,
                    DateAdded=DateTime.Now.AddMonths(-9)
                },
                new BookInStore(){
                    Id=3,
                    BookId=3,
                    Amount=24,
                    CostPrice=128.6m,
                    Price=200m,
                    DateAdded=DateTime.Now.AddDays(-20)
                },
                new BookInStore(){
                    Id=4,
                    BookId=4,
                    Amount=7,
                    CostPrice=742.5m,
                    Price=1000m,
                    DateAdded=DateTime.Now.AddDays(-4)
                },
                new BookInStore(){
                    Id=5,
                    BookId=5,
                    Amount=11,
                    CostPrice=418.1m,
                    Price=600m,
                    DateAdded=DateTime.Now
                },
                new BookInStore(){
                    Id=6,
                    BookId=6,
                    Amount=14,
                    CostPrice=251m,
                    Price=325m,
                    DateAdded=DateTime.Now
                },
                new BookInStore(){
                    Id=7,
                    BookId=7,
                    Amount=10,
                    CostPrice=284m,
                    Price=392m,
                    DateAdded=DateTime.Now.AddDays(-8)
                },
                new BookInStore(){
                    Id=8,
                    BookId=8,
                    Amount=15,
                    CostPrice=327.6m,
                    Price=450m,
                    DateAdded=DateTime.Now.AddDays(-8)
                }
            };
            BookReserve[] bookReserves = new BookReserve[]
            {
                new BookReserve()
                {
                    Id=1,
                    AccountId=2,
                    BookInStoreId=1,
                    Description ="Book for client with telephone 0984512574",
                    DateReserve=DateTime.Now.AddDays(-2)},
                new BookReserve()
                {
                    Id=2,
                    AccountId=2,
                    BookInStoreId=3,
                    DateReserve=DateTime.Now.AddDays(-1)}
            };
            BookSold[] bookSolds = new BookSold[]
            {
                new BookSold()
                {
                    Id=1,
                    AccountId=2,
                    BookInStoreId=2,
                    SoldPrice=400m,
                    DateSold=DateTime.Now.AddMonths(-3)
                },
                new BookSold()
                {
                    Id=2,
                    AccountId=2,
                    BookInStoreId=4,
                    SoldPrice=1000m,
                    DateSold=DateTime.Now.AddDays(-12)
                },
                new BookSold()
                {
                    Id=3,
                    AccountId=2,
                    BookInStoreId=5,
                    SoldPrice=600m,
                    DateSold=DateTime.Now.AddDays(-5)
                },
                new BookSold()
                {
                    Id=4,
                    AccountId=2,
                    BookInStoreId=4,
                    SoldPrice=1000m,
                    DateSold=DateTime.Now.AddDays(-2)
                }
            };
            Stock[] stocks = new Stock[]
            {
                new Stock()
                {
                    Id=1,
                    BookInStoreId=3,
                    Discount=15,
                    DateStart=DateTime.Now.AddDays(-5),
                    DateEnd=DateTime.Now.AddDays(20)
                }
            };
            BookSeries[] bookSeries = new BookSeries[]
            {
                new BookSeries(){ Id=1, Name="The Lord of the Rings" }
            };
            BookSeriesBook[] bookSeriesBooks = new BookSeriesBook[]
            {
                new BookSeriesBook(){ BookSeriesId=1, BookId=6, Position=1 },
                new BookSeriesBook(){ BookSeriesId=1, BookId=7, Position=2 },
                new BookSeriesBook(){ BookSeriesId=1, BookId=8, Position=3 }
            };

            modelBuilder.Entity<Account>().HasData(accounts);
            modelBuilder.Entity<Author>().HasData(authors);
            modelBuilder.Entity<Genre>().HasData(genres);
            modelBuilder.Entity<Publisher>().HasData(publishers);
            modelBuilder.Entity<Book>().HasData(books);
            modelBuilder.Entity<BookAuthor>().HasData(bookAuthors);
            modelBuilder.Entity<BookInStore>().HasData(bookInStores);
            modelBuilder.Entity<BookReserve>().HasData(bookReserves);
            modelBuilder.Entity<BookSold>().HasData(bookSolds);
            modelBuilder.Entity<Stock>().HasData(stocks);
            modelBuilder.Entity<BookSeries>().HasData(bookSeries);
            modelBuilder.Entity<BookSeriesBook>().HasData(bookSeriesBooks);
        }
    }
}
