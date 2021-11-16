using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models.Db
{
    public class Book
    {
        [Key, Required]
        public int Id { get; set; }
        [MaxLength(50), Required]
        public string Name { get; set; }
        [Required]
        public int Pages { get; set; }
        [Required]
        public int YearOfPublished { get; set; }
        [Required]
        public int PublisherId { get; set; }
        [Required]
        public int GenreId { get; set; }
        [ForeignKey("PublisherId")]
        public virtual Publisher Publisher { get; set; }
        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }
        public virtual List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public virtual List<BookSeriesBook> BookSeriesBooks { get; set; } = new List<BookSeriesBook>();
        public virtual List<BookInStore> BookInStores { get; set; } = new List<BookInStore>();
    }
}
