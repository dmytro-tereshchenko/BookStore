using BookStore.Infrastructure;
using BookStore.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Presenters
{
    internal class BookSeriesModel
    {
        private DbContextOptions<StoreContext> options;
        private BookSeriesView bookSeries;
        public BookSeriesModel(DbContextOptions<StoreContext> options, BookSeriesView bookSeries = null)
        {
            this.options = options;
            this.bookSeries = bookSeries ?? new BookSeriesView();
        }
        public BookSeriesView BookSeries { get => bookSeries; }
        public string Message { get; set; }

        public event EventHandler<EventArgs> MessageChanged;

        private async Task OnMessageChanged(EventArgs e) => await MessageChanged?.InvokeAsync(this, e);

        public async Task AddBookSeries()
        {
            using (StoreContext db = new StoreContext(options))
            {
                BookSeries dbBookSeries = await db.BookSerieses
                    .Where(a => a.Name == bookSeries.Name)
                    .FirstOrDefaultAsync();
                if (dbBookSeries is not null)
                {
                    Message = "Book series already exists";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                db.BookSerieses.Add((BookSeries)bookSeries);
                await db.SaveChangesAsync();
            }
            Message = "Book series created";
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
        public async Task EditBookSeries()
        {
            using (StoreContext db = new StoreContext(options))
            {
                BookSeries dbBookSeries = await db.BookSerieses.FindAsync(bookSeries.Id);
                if (dbBookSeries is null)
                {
                    Message = "Not found book series";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                BookSeries repeatBookSeries = await db.BookSerieses
                    .Where(a => a.Name == bookSeries.Name)
                    .FirstOrDefaultAsync();
                if (repeatBookSeries is not null && dbBookSeries.Id != repeatBookSeries.Id)
                {
                    Message = "Book series already exists";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                dbBookSeries.Name = bookSeries.Name;
                db.Entry<BookSeries>(dbBookSeries).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            Message = "Book series edited";
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
    }
}
