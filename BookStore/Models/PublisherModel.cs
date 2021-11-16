using BookStore.Infrastructure;
using BookStore.Models.Db;
using BookStore.Models.Presenters;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    internal class PublisherModel
    {
        private DbContextOptions<StoreContext> options;
        private PublisherView publisher;
        public PublisherModel(DbContextOptions<StoreContext> options, PublisherView publisher = null)
        {
            this.options = options;
            this.publisher = publisher ?? new PublisherView();
        }
        public PublisherView Publisher { get => publisher; }
        public string Message { get; set; }

        public event EventHandler<EventArgs> MessageChanged;

        private async Task OnMessageChanged(EventArgs e) => await MessageChanged?.InvokeAsync(this, e);

        public async Task AddPublisher()
        {
            using (StoreContext db = new StoreContext(options))
            {
                Publisher dbPublisher = await db.Publishers
                    .Where(a => a.Name == publisher.Name)
                    .FirstOrDefaultAsync();
                if (dbPublisher is not null)
                {
                    Message = "Publisher already exists";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                db.Publishers.Add((Publisher)publisher);
                await db.SaveChangesAsync();
            }
            Message = "Publisher created";
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
        public async Task EditPublisher()
        {
            using (StoreContext db = new StoreContext(options))
            {
                Publisher dbPublisher = await db.Publishers.FindAsync(publisher.Id);
                if (dbPublisher is null)
                {
                    Message = "Not found publisher";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                Publisher repeatPublisher = await db.Publishers
                    .Where(a => a.Name == publisher.Name)
                    .FirstOrDefaultAsync();
                if (repeatPublisher is not null && dbPublisher.Id != repeatPublisher.Id)
                {
                    Message = "Publisher already exists";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                dbPublisher.Name = publisher.Name;
                db.Entry<Publisher>(dbPublisher).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            Message = "Publisher edited";
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
    }
}
