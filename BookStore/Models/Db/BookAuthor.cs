using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models.Db
{
    public class BookAuthor
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }
    }
}
