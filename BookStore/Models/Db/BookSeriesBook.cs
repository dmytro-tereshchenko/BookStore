using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models.Db
{
    public class BookSeriesBook
    {
        public int? Position { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int BookSeriesId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        [ForeignKey("BookSeriesId")]
        public virtual BookSeries BookSeries { get; set; }
    }
}
