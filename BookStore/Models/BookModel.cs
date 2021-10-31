using BookStore.Infrastructure;
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
    internal class BookModel
    {
        private DbContextOptions<StoreContext> options;
        private BookView book;
        private string searchAuthorName;
        private string searchSeriesName;
        private string searchGenreName;
        private string searchPublisherName;
        private IEnumerable<AuthorView> searchAuthors;
        private IEnumerable<AuthorView> resultAuthors;
        private IEnumerable<BookSeriesView> searchSeries;
        private IEnumerable<GenreView> searchGenre;
        private IEnumerable<PublisherView> searchPublisher;
        public BookModel(DbContextOptions<StoreContext> options, BookView book = null)
        {
            this.options = options;
            this.book = book ?? new BookView();
            resultAuthors = new List<AuthorView>();
            if (book != null) {
                using (StoreContext db = new StoreContext(options))
                {
                    resultAuthors = (from author in db.Authors
                                     join ba in db.BookAuthors on author.Id equals ba.AuthorId
                                     join books in db.Books on ba.BookId equals books.Id
                                     where books.Id == book.Id
                                     select new AuthorView
                                     {
                                         Id = author.Id,
                                         FirstName = author.FirstName,
                                         MiddleName = author.MiddleName,
                                         LastName = author.LastName
                                     }).ToList();
                }
            }
        }
        public BookView Book { get => book; set => book = value; }
        public string SearchAuthorName { get => searchAuthorName; set => searchAuthorName = value; }
        public string SearchSeriesName { get => searchSeriesName; set => searchSeriesName = value; }
        public string SearchGenreName { get => searchGenreName; set => searchGenreName = value; }
        public string SearchPublisherName { get => searchPublisherName; set => searchPublisherName = value; }
        public string Message { get; set; }
        public IEnumerable<AuthorView> SearchAuthors { get => searchAuthors; set => searchAuthors = value; }
        public IEnumerable<AuthorView> ResultAuthors { get => resultAuthors; set => resultAuthors = value; }
        public IEnumerable<BookSeriesView> SearchSeries { get => searchSeries; set => searchSeries = value; }
        public IEnumerable<GenreView> SearchGenre { get => searchGenre; set => searchGenre = value; }
        public IEnumerable<PublisherView> SearchPublisher { get => searchPublisher; set => searchPublisher = value; }

        public event EventHandler<EventArgs> MessageChanged;
        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        private async Task OnMessageChanged(EventArgs e) => await MessageChanged?.InvokeAsync(this, e);
        private async Task OnPropertyChanged(PropertyChangedEventArgs e) => await PropertyChanged?.InvokeAsync(this, e);

        public async Task SearchAuthorsInStore()
        {
            using (StoreContext db = new StoreContext(options))
            {
                searchAuthors = await (from author in db.Authors
                                       where searchAuthorName == null || EF.Functions.Like(author.FullName, $"%{searchAuthorName}%")
                                       select new AuthorView
                                       {
                                           Id = author.Id,
                                           FirstName = author.FirstName,
                                           MiddleName = author.MiddleName,
                                           LastName = author.LastName
                                       }).ToListAsync();
                await OnPropertyChanged(new PropertyChangedEventArgs(nameof(SearchAuthors)));
            }
        }
        public async Task SearchSeriesInStore()
        {
            using (StoreContext db = new StoreContext(options))
            {
                searchSeries = await (from series in db.BookSerieses
                                      where searchSeriesName == null || EF.Functions.Like(series.Name, $"%{searchSeriesName}%")
                                      select new BookSeriesView
                                      {
                                          Id = series.Id,
                                          Name = series.Name
                                      }).ToListAsync();
                await OnPropertyChanged(new PropertyChangedEventArgs(nameof(SearchSeries)));
            }
        }
        public async Task SearchGenreInStore()
        {
            using (StoreContext db = new StoreContext(options))
            {
                searchGenre = await (from genre in db.Genres
                                      where searchGenreName == null || EF.Functions.Like(genre.Name, $"%{searchGenreName}%")
                                      select new GenreView
                                      {
                                          Id = genre.Id,
                                          Name = genre.Name
                                      }).ToListAsync();
                await OnPropertyChanged(new PropertyChangedEventArgs(nameof(SearchGenre)));
            }
        }
        public async Task SearchPublisherInStore()
        {
            using (StoreContext db = new StoreContext(options))
            {
                searchPublisher = await (from publisher in db.Publishers
                                     where searchPublisherName == null || EF.Functions.Like(publisher.Name, $"%{searchPublisherName}%")
                                     select new PublisherView
                                     {
                                         Id = publisher.Id,
                                         Name = publisher.Name
                                     }).ToListAsync();
                await OnPropertyChanged(new PropertyChangedEventArgs(nameof(SearchPublisher)));
            }
        }
        public async Task AddAuthor(AuthorView author)
        {
            resultAuthors = resultAuthors.Append(author);
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(ResultAuthors)));
        }
        public async Task DeleteAuthor(AuthorView author)
        {
            var list = resultAuthors.ToList();
            list.Remove(author);
            resultAuthors = list;
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(ResultAuthors)));
        }
        public async Task SetSeries(BookSeriesView series)
        {
            book.Series = series.Name;
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(Book)));
        }
        public async Task SetGenre(GenreView genre)
        {
            book.Genre = genre.Name;
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(Book)));
        }
        public async Task SetPublisher(PublisherView publisher)
        {
            book.Publisher = publisher.Name;
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(Book)));
        }
        public async Task AddBook()
        {
            try
            {
                int position = 0;
                int.TryParse(book.SeriesPosition, out position);
                using (StoreContext db = new StoreContext(options))
                {
                    Book newBook = new Book()
                    {
                        Genre = db.Genres.Where(g => g.Name == book.Genre).FirstOrDefault(),
                        Name = book.Name,
                        Pages = book.Pages,
                        Publisher = db.Publishers.Where(p => p.Name == book.Publisher).FirstOrDefault(),
                        YearOfPublished = book.YearOfPublished
                    };
                    await db.Books.AddAsync(newBook);
                    await db.BookAuthors.AddRangeAsync(resultAuthors.Select(a => new BookAuthor
                    {
                        Book = newBook,
                        AuthorId = a.Id
                    }));
                    if (!string.IsNullOrEmpty(book.Series))
                    {
                        await db.BookSeriesBooks.AddRangeAsync(new BookSeriesBook
                        {
                            Book = newBook,
                            Position = position,
                            BookSeries = db.BookSerieses.Where(bs => bs.Name == book.Series).FirstOrDefault()
                        });
                    }
                    await db.SaveChangesAsync();
                }
                Message = "Book created";
                await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
            }
        }
        public async Task EditBook()
        {
            try
            {
                using (StoreContext db = new StoreContext(options))
                {
                    Book dbBook = await db.Books.FindAsync(book.Id);
                    if (dbBook is null)
                    {
                        Message = "Not found book";
                        await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                        return;
                    }
                    dbBook.Genre = db.Genres.Where(g => g.Name == book.Genre).FirstOrDefault();
                    dbBook.Name = book.Name;
                    dbBook.Pages = book.Pages;
                    dbBook.Publisher = db.Publishers.Where(p => p.Name == book.Publisher).FirstOrDefault();
                    dbBook.YearOfPublished = book.YearOfPublished;
                    db.Entry<Book>(dbBook).State = EntityState.Modified;

                    db.BookAuthors.RemoveRange(db.BookAuthors.Where(ba => ba.BookId == book.Id));
                    db.BookSeriesBooks.RemoveRange(db.BookSeriesBooks.Where(bsb => bsb.BookId == book.Id));

                    await db.BookAuthors.AddRangeAsync(resultAuthors.Select(a => new BookAuthor
                    {
                        BookId = book.Id,
                        AuthorId = a.Id
                    }));
                    if (!string.IsNullOrEmpty(book.Series))
                    {
                        await db.BookSeriesBooks.AddRangeAsync(new BookSeriesBook
                        {
                            BookId = book.Id,
                            Position = int.Parse(book.SeriesPosition),
                            BookSeries = db.BookSerieses.Where(bs => bs.Name == book.Series).FirstOrDefault()
                        });
                    }
                    await db.SaveChangesAsync();
                }
                Message = "Book edited";
                await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
            }
        }
    }
}
