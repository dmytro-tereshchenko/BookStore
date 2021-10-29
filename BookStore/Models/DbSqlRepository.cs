﻿using BookStore.Infrastructure;
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
        private RadioButtonRepository period;
        private Account currentUser;
        private TypeResultView currentResultView;
        private string loginField;
        private string bookSearch;
        private string authorSearch;
        private string genreSearch;
        private IEnumerable<BookView> resultBooks;
        private IEnumerable<SimpleEntityView> resultSimpleEntities;
        private IEnumerable<BookReservedView> resultBooksReserved;
        private IEnumerable<BookSoldView> resultBooksSold;
        public DbSqlRepository(DbContextOptions<StoreContext> options, string[] keysRadioButton)
        {
            this.options = options;
            period = new RadioButtonRepository(keysRadioButton);
            currentUser = null;
            Task loadBd = new Task(() => { using (StoreContext db = new StoreContext(options)) { db.Accounts.ToList(); } }); //Load accounts to cashe 
            loadBd.Start();
        }
        public DbContextOptions<StoreContext> DbOptions { get => options; }
        public Account CurrentUser { get => currentUser; }
        public RadioButtonRepository Period { get => period; }
        public string LoginField { get => loginField; set => loginField = value; }
        public string BookSearch { get => bookSearch; set => bookSearch = value; }
        public string AuthorSearch { get => authorSearch; set => authorSearch = value; }
        public string GenreSearch { get => genreSearch; set => genreSearch = value; }
        public string Message { get; set; }
        public string TableName
        {
            get => currentResultView switch
            {
                TypeResultView.AllBooksView => "All books",
                TypeResultView.NewBooksView => "New books",
                TypeResultView.BestSellingBooksView => "Best selling books",
                TypeResultView.MostPopularAuthorsView => "Most popular authors",
                TypeResultView.MostPopularGenresView => "Most popular genres",
                TypeResultView.ReservedBooksView => "Reserved books",
                TypeResultView.SoldBooksView => "Sold books",
                TypeResultView.ResultSearchView => "Search results",
                _ => throw new NotImplementedException()
            };
        }
        public IEnumerable<BookView> ResultBooks { get => resultBooks; set => resultBooks = value; }
        public IEnumerable<SimpleEntityView> ResultSimpleEntities { get => resultSimpleEntities; set => resultSimpleEntities = value; }
        public IEnumerable<BookReservedView> ResultBooksReserved { get => resultBooksReserved; set => resultBooksReserved = value; }
        public IEnumerable<BookSoldView> ResultBooksSold { get => resultBooksSold; set => resultBooksSold = value; }

        public event EventHandler<EventArgs> CurrentUserChanged;
        public event EventHandler<EventArgs> ResultBooksViewChanged;
        public event EventHandler<EventArgs> ResultSimpleEntitiesViewChanged;
        public event EventHandler<EventArgs> ResultBooksReservedViewChanged;
        public event EventHandler<EventArgs> ResultBooksSoldViewChanged;
        public event EventHandler<EventArgs> MessageChanged;

        private async void OnCurrentUserChanged(EventArgs e) => await CurrentUserChanged?.InvokeAsync(this, e);
        private async void OnResultBooksViewChanged(EventArgs e) => await ResultBooksViewChanged?.InvokeAsync(this, e);
        private async void OnResultSimpleEntitiesViewChanged(EventArgs e) => await ResultSimpleEntitiesViewChanged?.InvokeAsync(this, e);
        private async void OnResultBooksReservedViewChanged(EventArgs e) => await ResultBooksReservedViewChanged?.InvokeAsync(this, e);
        private async void OnResultBooksSoldViewChanged(EventArgs e) => await ResultBooksSoldViewChanged?.InvokeAsync(this, e);
        private async void OnMessageChanged(EventArgs e) => await MessageChanged?.InvokeAsync(this, e);

        public async Task UserLogIn(string password)
        {
            using (StoreContext db = new StoreContext(options))
            {
                currentUser = await db.Accounts.Where(ac => ac.Login == loginField && ac.Password == password).FirstOrDefaultAsync();
            }
            if (currentUser != null)
            {
                OnCurrentUserChanged(new PropertyChangedEventArgs(nameof(CurrentUser)));
            }
            else
            {
                Message = "Wrong login or password";
                OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
            }
        }
        public async Task SearchBook()
        {
            using (StoreContext db = new StoreContext(options))
            {
                var query = await (from bookInStore in db.BookInStores
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
                                                                select reserve).Count()) > 0 &&
                                        (genreSearch == null || EF.Functions.Like(genre.Name, $"%{genreSearch}%")) &&
                                        (bookSearch == null || EF.Functions.Like(book.Name, $"%{bookSearch}%"))
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
                                   })
                              .ToListAsync();
                resultBooks = query.Where((authorFilter) => authorSearch == null || authorFilter.Authors.Contains(authorSearch));
            }
            currentResultView = TypeResultView.ResultSearchView;
            ClearViews();
            OnResultBooksViewChanged(new PropertyChangedEventArgs(nameof(ResultBooks)));
        }
        public Task UserLogout()
        {
            currentUser = null;
            OnCurrentUserChanged(new PropertyChangedEventArgs(nameof(CurrentUser)));
            return Task.CompletedTask;
        }
        public async Task PeriodChanged()
        {
            switch (currentResultView)
            {
                case TypeResultView.NewBooksView:
                    await NewBooksView();
                    break;
                case TypeResultView.BestSellingBooksView:
                    await BestSellingBooksView();
                    break;
                case TypeResultView.MostPopularAuthorsView:
                    await MostPopularAuthorsView();
                    break;
                case TypeResultView.MostPopularGenresView:
                    await MostPopularGenresView();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public async Task AllBooksView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultBooks = await (from bookInStore in db.BookInStores
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
                                     }).ToListAsync();
            }
            currentResultView = TypeResultView.AllBooksView;
            ClearViews();
            OnResultBooksViewChanged(new PropertyChangedEventArgs(nameof(ResultBooks)));
        }
        public async Task NewBooksView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultBooks = await (from bookInStore in db.BookInStores
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
                                     where bookInStore.DateAdded > DateTime.Now.AddDays(-GetDays())
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
                                     }).ToListAsync();
            }
            currentResultView = TypeResultView.NewBooksView;
            ClearViews();
            OnResultBooksViewChanged(new PropertyChangedEventArgs(nameof(ResultBooks)));
        }
        public async Task BestSellingBooksView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                var query = await (from bst in db.BookSolds
                                   join bookInStore in db.BookInStores on bst.BookInStoreId equals bookInStore.Id
                                   join book in db.Books on bookInStore.BookId equals book.Id
                                   join genre in db.Genres on book.GenreId equals genre.Id
                                   join publisher in db.Publishers on book.PublisherId equals publisher.Id
                                   join bookSold in db.BookSolds on bookInStore.Id equals bookSold.BookInStoreId
                                   join bsb in db.BookSeriesBooks on book.Id equals bsb.BookId into subBsbTable //LEFT OUTHER JOIN BookSeries
                                   from subBsb in subBsbTable.DefaultIfEmpty()
                                   join bs in db.BookSerieses on subBsb.BookSeriesId equals bs.Id into subBsTable //LEFT OUTHER JOIN BookSeries
                                   from subBs in subBsTable.DefaultIfEmpty()
                                   where bookSold.DateSold > DateTime.Now.AddDays(-GetDays())
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
                                   }).ToListAsync();
                resultBooks = query.GroupBy(g => new
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
                }).Select(grp => new
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
                .Take(5).Select(r => new BookView
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
                });
            }
            currentResultView = TypeResultView.BestSellingBooksView;
            ClearViews();
            OnResultBooksViewChanged(new PropertyChangedEventArgs(nameof(ResultBooks)));
        }
        public async Task MostPopularAuthorsView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultSimpleEntities = await (from bookSold in db.BookSolds
                                              join bookInStore in db.BookInStores on bookSold.BookInStoreId equals bookInStore.Id
                                              join book in db.Books on bookInStore.BookId equals book.Id
                                              join bookAuthor in db.BookAuthors on book.Id equals bookAuthor.BookId
                                              join author in db.Authors on bookAuthor.AuthorId equals author.Id
                                              where bookSold.DateSold > DateTime.Now.AddDays(-GetDays())
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
                               .ToListAsync();
            }
            currentResultView = TypeResultView.MostPopularAuthorsView;
            ClearViews();
            OnResultSimpleEntitiesViewChanged(new PropertyChangedEventArgs(nameof(ResultSimpleEntities)));
        }
        public async Task MostPopularGenresView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultSimpleEntities = await (from bookSold in db.BookSolds
                                        join bookInStore in db.BookInStores on bookSold.BookInStoreId equals bookInStore.Id
                                        join book in db.Books on bookInStore.BookId equals book.Id
                                        join genre in db.Genres on book.GenreId equals genre.Id
                                        where bookSold.DateSold > DateTime.Now.AddDays(-GetDays())
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
                               .ToListAsync();
            }
            currentResultView = TypeResultView.MostPopularGenresView;
            ClearViews();
            OnResultSimpleEntitiesViewChanged(new PropertyChangedEventArgs(nameof(ResultSimpleEntities)));
        }
        public async Task ReservedBooksView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultBooksReserved = await (from reservedBook in db.BookReserves
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
                                       .ToListAsync();
            }
            currentResultView = TypeResultView.ReservedBooksView;
            ClearViews();
            OnResultBooksReservedViewChanged(new PropertyChangedEventArgs(nameof(ResultBooksReserved)));
        }
        public async Task SoldBooksView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultBooksSold = await (from soldBook in db.BookSolds
                                         join bookInStore in db.BookInStores on soldBook.BookInStoreId equals bookInStore.Id
                                         join book in db.Books on bookInStore.BookId equals book.Id
                                         join account in db.Accounts on soldBook.AccountId equals account.Id into subTable //LEFT OUTHER JOIN Accounts
                                         from resultAcc in subTable.DefaultIfEmpty()
                                         select new BookSoldView
                                         {
                                             Id = soldBook.Id,
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
                                             CostPrice = (Math.Round(bookInStore.CostPrice * 100) / 100).ToString("#0.00"),
                                             SoldPrice = (Math.Round(soldBook.SoldPrice * 100) / 100).ToString("#0.00"),
                                             AccountLogin = (resultAcc == null ? "unknown" : resultAcc.Login),
                                             DateSold = soldBook.DateSold.ToShortDateString()
                                         })
                                   .ToListAsync();
            }
            currentResultView = TypeResultView.SoldBooksView;
            ClearViews();
            OnResultBooksSoldViewChanged(new PropertyChangedEventArgs(nameof(ResultBooksSold)));
        }
        public async Task BuyBook(BookView buyBook)
        {
            using (StoreContext db = new StoreContext(options))
            {
                BookInStore book = await db.BookInStores.FindAsync(buyBook.Id);
                if (book is null || book.Amount < 1)
                {
                    Message = "Not found book or not free to sale";
                    OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                book.Amount -= 1;
                db.BookSolds.Add(new BookSold()
                {
                    AccountId = currentUser?.Id ?? null,
                    BookInStoreId = buyBook.Id,
                    DateSold = DateTime.Now,
                    SoldPrice = decimal.Parse(buyBook.Price)
                });
                await db.SaveChangesAsync();
            }
            Message = "Book bought";
            OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
        private void ClearViews()
        {
            if (currentResultView != TypeResultView.AllBooksView &&
            currentResultView != TypeResultView.NewBooksView &&
            currentResultView != TypeResultView.BestSellingBooksView &&
            currentResultView != TypeResultView.ResultSearchView)
            {
                resultBooks = null;
            }
            if (currentResultView != TypeResultView.MostPopularAuthorsView &&
            currentResultView != TypeResultView.MostPopularGenresView)
            {
                resultSimpleEntities = null;
            }
            if (currentResultView != TypeResultView.ReservedBooksView)
            {
                resultBooksReserved = null;
            }
            if (currentResultView != TypeResultView.SoldBooksView)
            {
                resultBooksSold = null;
            }
        }
        private int GetDays() 
        {
            if(period["Day"]) return 1;
            else if (period["Week"]) return 7;
            else if (period["Month"]) return 31;
            else if (period["Year"]) return 365;
            throw new NotImplementedException();
        }
    }
}
