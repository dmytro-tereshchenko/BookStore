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
    internal class AuthorModel
    {
        private DbContextOptions<StoreContext> options;
        private AuthorView author;
        public AuthorModel(DbContextOptions<StoreContext> options, AuthorView author = null)
        {
            this.options = options;
            this.author = author ?? new AuthorView();
        }
        public AuthorView Author { get => author; }
        public string Message { get; set; }

        public event EventHandler<EventArgs> MessageChanged;

        private async Task OnMessageChanged(EventArgs e) => await MessageChanged?.InvokeAsync(this, e);

        public async Task AddAuthor()
        {
            using (StoreContext db = new StoreContext(options))
            {
                Author dbAuthor = await db.Authors
                    .Where(a => a.FirstName == author.FirstName &&
                    a.MiddleName == author.MiddleName &&
                    a.LastName == author.LastName).FirstOrDefaultAsync();
                if (dbAuthor is not null)
                {
                    Message = "Author already exists";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                /*db.Authors.Add(new Author()
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    MiddleName = author.MiddleName,
                    LastName = author.LastName
                });*/
                db.Authors.Add((Author)author);
                await db.SaveChangesAsync();
            }
            Message = "Author created";
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
        public async Task EditAuthor()
        {
            using (StoreContext db = new StoreContext(options))
            {
                Author dbAuthor = await db.Authors.FindAsync(author.Id);
                if (dbAuthor is null)
                {
                    Message = "Not found author";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                Author repeatAuthor = await db.Authors
                    .Where(a => a.FirstName == author.FirstName &&
                    a.MiddleName == author.MiddleName &&
                    a.LastName == author.LastName).FirstOrDefaultAsync();
                if (repeatAuthor is not null && dbAuthor.Id != repeatAuthor.Id)
                {
                    Message = "Author already exists";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                dbAuthor.FirstName = author.FirstName;
                dbAuthor.MiddleName = author.MiddleName;
                dbAuthor.LastName = author.LastName;
                db.Entry<Author>(dbAuthor).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            Message = "Author edited";
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
    }
}

