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
    internal class GenreModel
    {
        private DbContextOptions<StoreContext> options;
        private GenreView genre;
        public GenreModel(DbContextOptions<StoreContext> options, GenreView genre = null)
        {
            this.options = options;
            this.genre = genre ?? new GenreView();
        }
        public GenreView Genre { get => genre; }
        public string Message { get; set; }

        public event EventHandler<EventArgs> MessageChanged;

        private async Task OnMessageChanged(EventArgs e) => await MessageChanged?.InvokeAsync(this, e);

        public async Task AddGenre()
        {
            using (StoreContext db = new StoreContext(options))
            {
                Genre dbGenre = await db.Genres
                    .Where(a => a.Name == genre.Name)
                    .FirstOrDefaultAsync();
                if (dbGenre is not null)
                {
                    Message = "Genre already exists";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                db.Genres.Add((Genre)genre);
                await db.SaveChangesAsync();
            }
            Message = "Genre created";
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
        public async Task EditGenre()
        {
            using (StoreContext db = new StoreContext(options))
            {
                Genre dbGenre = await db.Genres.FindAsync(genre.Id);
                if (dbGenre is null)
                {
                    Message = "Not found genre";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                Genre repeatGenre = await db.Genres
                    .Where(a => a.Name == genre.Name)
                    .FirstOrDefaultAsync();
                if (repeatGenre is not null && dbGenre.Id != repeatGenre.Id)
                {
                    Message = "Genre already exists";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                dbGenre.Name = genre.Name;
                db.Entry<Genre>(dbGenre).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            Message = "Genre edited";
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
    }
}
