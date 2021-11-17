using BookStore.Infrastructure;
using BookStore.Interfaces;
using BookStore.Models.Db;
using BookStore.Models.Presenters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    internal class DbSqlRepository: IDbRepository<StoreContext>
    {
        private DbContextOptions<StoreContext> options;
        private RadioButtonRepository period;
        private Account currentUser;
        private TypeResultView currentResultView;
        private string loginField;
        private string bookSearch;
        private string authorSearch;
        private string genreSearch;
        private IEnumerable<BookViewShow> resultBooks;
        private IEnumerable<SimpleEntityView> resultSimpleEntities;
        private IEnumerable<BookReservedView> resultBooksReserved;
        private IEnumerable<BookSoldView> resultBooksSold;
        private IEnumerable<AccountView> resultManageAccounts;
        private IEnumerable<AuthorView> resultManageAuthors;
        private IEnumerable<GenreView> resultManageGenres;
        private IEnumerable<PublisherView> resultManagePublishers;
        private IEnumerable<BookView> resultManageBooks;
        private IEnumerable<BookInStoreView> resultManageBooksInStore;
        private IEnumerable<StockView> resultManageStocks;
        private IEnumerable<BookSeriesView> resultManageBookSeries;
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
        public TypeResultView TypeResultView { get => currentResultView; }
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
                TypeResultView.ResultManageAccountsView => "Accounts",
                TypeResultView.ResultManageAuthorsView => "Authors",
                TypeResultView.ResultManageGenresView => "Genres",
                TypeResultView.ResultManagePublishersView => "Publishers",
                TypeResultView.ResultManageBooksView => "Books",
                TypeResultView.ResultManageBooksInStoreView => "Books in Store",
                TypeResultView.ResultManageStocksView => "Stocks",
                TypeResultView.ResultManageBookSeriesView => "Series",
                _ => throw new NotImplementedException()
            };
        }
        public IEnumerable<BookViewShow> ResultBooksView { get => resultBooks; set => resultBooks = value; }
        public IEnumerable<SimpleEntityView> ResultSimpleEnitiesView { get => resultSimpleEntities; set => resultSimpleEntities = value; }
        public IEnumerable<BookReservedView> ResultReservedBooksView { get => resultBooksReserved; set => resultBooksReserved = value; }
        public IEnumerable<BookSoldView> ResultSoldBooksView { get => resultBooksSold; set => resultBooksSold = value; }
        public IEnumerable<AccountView> ResultManageAccountsView { get => resultManageAccounts; set => resultManageAccounts = value; }
        public IEnumerable<AuthorView> ResultManageAuthorsView { get => resultManageAuthors; set => resultManageAuthors = value; }
        public IEnumerable<GenreView> ResultManageGenresView { get => resultManageGenres; set => resultManageGenres = value; }
        public IEnumerable<PublisherView> ResultManagePublishersView { get => resultManagePublishers; set => resultManagePublishers = value; }
        public IEnumerable<BookView> ResultManageBooksView { get => resultManageBooks; set => resultManageBooks = value; }
        public IEnumerable<BookInStoreView> ResultManageBooksInStoreView { get => resultManageBooksInStore; set => resultManageBooksInStore = value; }
        public IEnumerable<StockView> ResultManageStocksView { get => resultManageStocks; set => resultManageStocks = value; }
        public IEnumerable<BookSeriesView> ResultManageBookSeriesView { get => resultManageBookSeries; set => resultManageBookSeries = value; }

        public event EventHandler<EventArgs> CurrentUserChanged;
        public event EventHandler<EventArgs> MessageChanged;
        public event EventHandler<PropertyChangedEventArgs> ResultViewChanged;

        private async Task OnCurrentUserChanged(EventArgs e) => await CurrentUserChanged?.InvokeAsync(this, e);
        private async Task OnMessageChanged(EventArgs e) => await MessageChanged?.InvokeAsync(this, e);
        private async Task OnResultViewChanged(PropertyChangedEventArgs e) => await ResultViewChanged?.InvokeAsync(this, e);

        public async Task UserLogIn(string password)
        {
            using (StoreContext db = new StoreContext(options))
            {
                currentUser = await db.Accounts.Where(ac => ac.Login == loginField && ac.Password == password).FirstOrDefaultAsync();
            }
            if (currentUser != null)
            {
                await OnCurrentUserChanged(new PropertyChangedEventArgs(nameof(CurrentUser)));
            }
            else
            {
                Message = "Wrong login or password";
                await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
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
                                   select new BookViewShow
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
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultBooksView)));
        }
        public async Task UserLogout()
        {
            currentUser = null;
            currentResultView = TypeResultView.AllBooksView;
            ClearViews();
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultBooksView)));
            await OnCurrentUserChanged(new PropertyChangedEventArgs(nameof(CurrentUser)));
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
                                     select new BookViewShow
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
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultBooksView)));
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
                                     select new BookViewShow
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
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultBooksView)));
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
                                   select new BookViewShow
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
                .Take(5).Select(r => new BookViewShow
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
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultBooksView)));
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
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultSimpleEnitiesView)));
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
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultSimpleEnitiesView)));
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
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultReservedBooksView)));
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
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultSoldBooksView)));
        }
        public async Task ManageAccountsView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultManageAccounts = await (from account in db.Accounts
                                              select new AccountView
                                              {
                                                  Id = account.Id,
                                                  Login = account.Login,
                                                  Password = new String('*', account.Password.Length),
                                                  IsAdmin = account.Admin
                                              })
                                   .ToListAsync();
            }
            currentResultView = TypeResultView.ResultManageAccountsView;
            ClearViews();
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultManageAccountsView)));
        }
        public async Task ManageAuthorsView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultManageAuthors = await (from author in db.Authors
                                             select new AuthorView
                                             {
                                                 Id = author.Id,
                                                 FirstName = author.FirstName,
                                                 MiddleName = author.MiddleName,
                                                 LastName = author.LastName
                                             })
                                   .ToListAsync();
            }
            currentResultView = TypeResultView.ResultManageAuthorsView;
            ClearViews();
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultManageAuthorsView)));
        }
        public async Task ManageGenresView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultManageGenres = await (from genre in db.Genres
                                            select new GenreView
                                            {
                                                Id = genre.Id,
                                                Name = genre.Name
                                            })
                                   .ToListAsync();
            }
            currentResultView = TypeResultView.ResultManageGenresView;
            ClearViews();
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultManageGenresView)));
        }
        public async Task ManagePublishersView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultManagePublishers = await (from publisher in db.Publishers
                                                select new PublisherView
                                                {
                                                    Id = publisher.Id,
                                                    Name = publisher.Name
                                                })
                                   .ToListAsync();
            }
            currentResultView = TypeResultView.ResultManagePublishersView;
            ClearViews();
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultManagePublishersView)));
        }
        public async Task ManageBooksView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultManageBooks = await (from book in db.Books
                                           join genre in db.Genres on book.GenreId equals genre.Id
                                           join publisher in db.Publishers on book.PublisherId equals publisher.Id
                                           join bsb in db.BookSeriesBooks on book.Id equals bsb.BookId into subBsbTable //LEFT OUTHER JOIN BookSeries
                                           from subBsb in subBsbTable.DefaultIfEmpty()
                                           join bs in db.BookSerieses on subBsb.BookSeriesId equals bs.Id into subBsTable //LEFT OUTHER JOIN BookSeries
                                           from subBs in subBsTable.DefaultIfEmpty()
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
                                               PublisherId = publisher.Id,
                                               Publisher = publisher.Name,
                                               GenreId = genre.Id,
                                               Genre = genre.Name,
                                               SeriesId = (subBsb == null || subBs == null ? 0 : subBs.Id),
                                               Series = (subBsb == null || subBs == null ? "" : subBs.Name),
                                               SeriesPosition = (subBsb == null || subBs == null||subBsb.Position == null ? "" : subBsb.Position.ToString())
                                           }).ToListAsync();
            }
            currentResultView = TypeResultView.ResultManageBooksView;
            ClearViews();
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultManageBooksView)));
        }
        public async Task ManageBooksInStoreView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultManageBooksInStore = await (from bookInStore in db.BookInStores
                                                  join book in db.Books on bookInStore.BookId equals book.Id
                                                  select new BookInStoreView
                                                  {
                                                      Id = bookInStore.Id,
                                                      BookId = book.Id,
                                                      Book = book.Name,
                                                      CostPrice = bookInStore.CostPrice.ToString("#0.00"),
                                                      Price = bookInStore.Price.ToString("#0.00"),
                                                      Amount = bookInStore.Amount,
                                                      DateAdded = bookInStore.DateAdded.ToShortDateString()
                                                  }).ToListAsync();
            }
            currentResultView = TypeResultView.ResultManageBooksInStoreView;
            ClearViews();
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultManageBooksInStoreView)));
        }
        public async Task ManageStocksView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultManageStocks = await (from bookInStore in db.BookInStores
                                            join book in db.Books on bookInStore.BookId equals book.Id
                                            join stock in db.Stocks on bookInStore.Id equals stock.BookInStoreId
                                            select new StockView
                                            {
                                                Id = stock.Id,
                                                BookInStoreId = bookInStore.Id,
                                                BookInStore = book.Name,
                                                Discount = stock.Discount.ToString("#0.00"),
                                                DateStart = stock.DateStart.ToShortDateString(),
                                                DateEnd = stock.DateEnd.ToShortDateString()
                                            }).ToListAsync();
            }
            currentResultView = TypeResultView.ResultManageStocksView;
            ClearViews();
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultManageStocksView)));
        }
        public async Task ManageBookSeriesView()
        {
            using (StoreContext db = new StoreContext(options))
            {
                resultManageBookSeries = await (from bookSeries in db.BookSerieses
                                                select new BookSeriesView
                                                {
                                                    Id = bookSeries.Id,
                                                    Name = bookSeries.Name
                                                }).ToListAsync();
            }
            currentResultView = TypeResultView.ResultManageBookSeriesView;
            ClearViews();
            await OnResultViewChanged(new PropertyChangedEventArgs(nameof(ResultManageBookSeriesView)));
        }
        public async Task BuyBook(BookViewShow buyBook)
        {
            using (StoreContext db = new StoreContext(options))
            {
                BookInStore book = await db.BookInStores.FindAsync(buyBook.Id);
                if (book is null || book.Amount < 1)
                {
                    Message = "Not found book or not free to sale";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
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
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
        public async Task DeleteAccount(AccountView account)
        {
            using (StoreContext db = new StoreContext(options))
            {
                Account accountInDb = await db.Accounts.FindAsync(account.Id);
                if (accountInDb is null)
                {
                    Message = "Not found account";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                db.Accounts.Remove(accountInDb);
                await db.SaveChangesAsync();
            }
            Message = "Account removed";
            await ManageAccountsView();
        }
        public async Task DeleteAuthor(AuthorView author)
        {
            using (StoreContext db = new StoreContext(options))
            {
                Author authorInDb = await db.Authors.FindAsync(author.Id);
                if (authorInDb is null)
                {
                    Message = "Not found author";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                db.BookAuthors.RemoveRange(db.BookAuthors.Where(ba => ba.AuthorId == authorInDb.Id));
                db.Authors.Remove(authorInDb);
                await db.SaveChangesAsync();
            }
            Message = "Author removed";
            await ManageAuthorsView();
        }
        public async Task DeleteGenre(GenreView genre)
        {
            using (StoreContext db = new StoreContext(options))
            {
                Genre genreInDb = await db.Genres.FindAsync(genre.Id);
                if (genreInDb is null)
                {
                    Message = "Not found genre";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                db.Genres.Remove(genreInDb);
                await db.SaveChangesAsync();
            }
            Message = "Genre removed";
            await ManageGenresView();
        }
        public async Task DeletePublisher(PublisherView publisher)
        {
            using (StoreContext db = new StoreContext(options))
            {
                Publisher publisherInDb = await db.Publishers.FindAsync(publisher.Id);
                if (publisherInDb is null)
                {
                    Message = "Not found publisher";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                db.Publishers.Remove(publisherInDb);
                await db.SaveChangesAsync();
            }
            Message = "Publisher removed";
            await ManagePublishersView();
        }
        public async Task DeleteBook(BookView book)
        {
            using (StoreContext db = new StoreContext(options))
            {
                Book bookInDb = await db.Books.FindAsync(book.Id);
                if (bookInDb is null)
                {
                    Message = "Not found book";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                db.BookSeriesBooks.RemoveRange(db.BookSeriesBooks.Where(bsb => bsb.BookId == bookInDb.Id));
                db.BookAuthors.RemoveRange(db.BookAuthors.Where(ba => ba.BookId == bookInDb.Id));
                db.Books.Remove(bookInDb);
                await db.SaveChangesAsync();
            }
            Message = "Book removed";
            await ManageBooksView();
        }
        public async Task DeleteBookInStore(BookInStoreView bookInStore)
        {
            using (StoreContext db = new StoreContext(options))
            {
                BookInStore bookInStoreInDb = await db.BookInStores.FindAsync(bookInStore.Id);
                if (bookInStoreInDb is null)
                {
                    Message = "Not found book in store";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                db.BookInStores.Remove(bookInStoreInDb);
                await db.SaveChangesAsync();
            }
            Message = "Book in store removed";
            await ManageBooksInStoreView();
        }
        public async Task DeleteStock(StockView stock)
        {
            using (StoreContext db = new StoreContext(options))
            {
                Stock stockInDb = await db.Stocks.FindAsync(stock.Id);
                if (stockInDb is null)
                {
                    Message = "Not found stock";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                db.Stocks.Remove(stockInDb);
                await db.SaveChangesAsync();
            }
            Message = "Stock removed";
            await ManageStocksView();
        }
        public async Task DeleteBookSeries(BookSeriesView bookSeries)
        {
            using (StoreContext db = new StoreContext(options))
            {
                BookSeries stockInDb = await db.BookSerieses.FindAsync(bookSeries.Id);
                if (stockInDb is null)
                {
                    Message = "Not found stock";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                db.BookSeriesBooks.RemoveRange(db.BookSeriesBooks.Where(bsb => bsb.BookSeriesId == stockInDb.Id));
                db.BookSerieses.Remove(stockInDb);
                await db.SaveChangesAsync();
            }
            Message = "Book series removed";
            await ManageBookSeriesView();
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
            if (currentResultView != TypeResultView.ResultManageAccountsView)
            {
                resultManageAccounts = null;
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
