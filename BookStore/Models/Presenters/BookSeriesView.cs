using BookStore.Models.Db;
using System.ComponentModel;

namespace BookStore.Models.Presenters
{
    public class BookSeriesView
    {
        public int Id { get; set; }
        [DisplayName("Book series")]
        public string Name { get; set; }
        public static explicit operator BookSeries(BookSeriesView obj) =>
            new BookSeries { Id = obj.Id, Name = obj.Name };
    }
}
