using System.ComponentModel;

namespace BookStore.Models.Presenters
{
    public class BookSeriesView
    {
        public int Id { get; set; }
        [DisplayName("Book series")]
        public string Name { get; set; }
    }
}
