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
    internal class StockModel
    {
        private DbContextOptions<StoreContext> options;
        private StockView stock;
        private string searchBookInStore;
        private IEnumerable<BookInStoreView> bookInStores;
        private DateTime startDate;
        private DateTime endDate;
        public StockModel(DbContextOptions<StoreContext> options, StockView stock = null)
        {
            this.options = options;
            if (stock == null)
            {
                this.stock = new StockView();
                startDate = DateTime.Now;
                endDate = DateTime.Now.AddMonths(1);
            }
            else
            {
                this.stock = stock;
                startDate = DateTime.Parse(stock.DateStart);
                endDate = DateTime.Parse(stock.DateEnd);
            }
        }
        public StockView Stock { get => stock; set => stock = value; }
        public string SearchBookName { get => searchBookInStore; set => searchBookInStore = value; }
        public string Message { get; set; }
        public IEnumerable<BookInStoreView> BookInStores { get => bookInStores; set => bookInStores = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }

        public event EventHandler<EventArgs> MessageChanged;
        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        private async Task OnMessageChanged(EventArgs e) => await MessageChanged?.InvokeAsync(this, e);
        private async Task OnPropertyChanged(PropertyChangedEventArgs e) => await PropertyChanged?.InvokeAsync(this, e);

        public async Task SearchBookInStore()
        {
            using (StoreContext db = new StoreContext(options))
            {
                bookInStores = await (from bookInStore in db.BookInStores
                                      join book in db.Books on bookInStore.BookId equals book.Id
                                      where searchBookInStore == null || EF.Functions.Like(book.Name, $"%{searchBookInStore}%")
                                      select new BookInStoreView
                                      {
                                          Id = bookInStore.Id,
                                          Book = book.Name,
                                          CostPrice = bookInStore.CostPrice.ToString("#0.00"),
                                          Price = bookInStore.Price.ToString("#0.00"),
                                          Amount = bookInStore.Amount,
                                          DateAdded = bookInStore.DateAdded
                                      }).ToListAsync();
                await OnPropertyChanged(new PropertyChangedEventArgs(nameof(BookInStores)));
            }
        }
        public async Task SetBookInStore(BookInStoreView bookInStore)
        {
            stock.BookInStore = bookInStore.Book;
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(Stock)));
        }
        public async Task AddStock()
        {
            decimal discount;
            if (decimal.TryParse(Stock.Discount, out discount))
            {
                using (StoreContext db = new StoreContext(options))
                {
                    Stock dbStock = await (from stock in db.Stocks
                                           join bookInStore in db.BookInStores on stock.BookInStoreId equals bookInStore.Id
                                           join book in db.Books on bookInStore.BookId equals book.Id
                                           where book.Name == Stock.BookInStore
                                           select stock)
                                           .FirstOrDefaultAsync();
                    if (dbStock is not null)
                    {
                        Message = "Stock already exists";
                        await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                        return;
                    }
                    db.Stocks.Add(new Stock()
                    {
                        Id = stock.Id,
                        BookInStoreId = (from bookInStore in db.BookInStores
                                         join book in db.Books on bookInStore.BookId equals book.Id
                                         where book.Name == Stock.BookInStore
                                         select bookInStore.Id).FirstOrDefault(),
                        Discount = discount,
                        DateStart = startDate,
                        DateEnd = endDate
                    });
                    await db.SaveChangesAsync();
                }
                Message = "Stock created";
            }
            else
            {
                Message = "Wrong format of entering discount";
            }
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
        public async Task EditStock()
        {
            decimal discount;
            if (decimal.TryParse(Stock.Discount, out discount))
            {
                using (StoreContext db = new StoreContext(options))
                {
                    Stock dbStock = await db.Stocks.FindAsync(stock.Id);
                    if (dbStock is null)
                    {
                        Message = "Not found stock";
                        await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                        return;
                    }
                    dbStock.BookInStoreId = (from bookInStore in db.BookInStores
                                             join book in db.Books on bookInStore.BookId equals book.Id
                                             where book.Name == Stock.BookInStore
                                             select bookInStore.Id).FirstOrDefault();
                    dbStock.Discount = discount;
                    dbStock.DateStart = startDate;
                    dbStock.DateEnd = endDate;
                    db.Entry<Stock>(dbStock).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                Message = "Stock edited";
            }
            else
            {
                Message = "Wrong format of entering discount";
            }
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
    }
}
