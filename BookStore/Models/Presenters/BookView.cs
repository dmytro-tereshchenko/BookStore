using System.ComponentModel;

namespace BookStore.Models.Presenters
{
    public class BookView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Authors { get; set; }
        public int Pages { get; set; }
        [DisplayName("Publication year")]
        public int YearOfPublished { get; set; }
        public string Publisher { get; set; }
        public string Genre { get; set; }
        public string Series { get; set; }
        [DisplayName("Position in series")]
        public string SeriesPosition { get; set; }
    }
}
