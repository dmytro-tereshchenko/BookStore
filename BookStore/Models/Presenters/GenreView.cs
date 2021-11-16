using BookStore.Models.Db;
using System.ComponentModel;

namespace BookStore.Models.Presenters
{
    public class GenreView
    {
        public int Id { get; set; }
        [DisplayName("Genre name")]
        public string Name { get; set; }
        public static explicit operator Genre(GenreView obj) =>
            new Genre { Id = obj.Id, Name = obj.Name };
    }
}
