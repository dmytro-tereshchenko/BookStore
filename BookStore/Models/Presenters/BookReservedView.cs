using System.ComponentModel;

namespace BookStore.Models.Presenters
{
    public class BookReservedView
    {
        public int Id { get; set; }
        [DisplayName("Book's name")]
        public string BookName { get; set; }
        public string Authors { get; set; }
        public string Description { get; set; }
        public string Price { get; set; } 
        [DisplayName("Accaunt's name")]
        public string AccountLogin { get; set; }
        [DisplayName("Date of reserve")]
        public string DateReserve { get; set; }
    }
}
