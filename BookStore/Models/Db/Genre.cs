using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Db
{
    public class Genre
    {
        [Key, Required]
        public int Id { get; set; }
        [MaxLength(40), Required]
        public string Name { get; set; }
        public virtual List<Book> Books { get; set; } = new List<Book>();
    }
}
