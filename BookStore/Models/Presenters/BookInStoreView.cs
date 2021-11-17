using System.ComponentModel;

namespace BookStore.Models.Presenters
{
    public class BookInStoreView
    {
        public int Id { get; set; }
        [DisplayName("Disabled")]
        public int BookId { get; set; }
        public string Book { get; set; }
        [DisplayName("Cost price")]
        public string CostPrice { get; set; }
        public string Price { get; set; }
        public int Amount { get; set; }
        [DisplayName("Date added")]
        public string DateAdded { get; set; }
    }
}
