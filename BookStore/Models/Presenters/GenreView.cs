using System.ComponentModel;

namespace BookStore.Models.Presenters
{
    public class GenreView
    {
        public int Id { get; set; }
        [DisplayName("Genre name")]
        public string Name { get; set; }
    }
}
