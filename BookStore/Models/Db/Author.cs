using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models.Db
{
    public class Author
    {
        [Key, Required]
        public int Id { get; set; }
        [MaxLength(30), Required]
        public string FirstName { get; set; }
        [MaxLength(30)]
#nullable enable
        public string? MiddleName { get; set; }
#nullable disable
        [MaxLength(30), Required]
        public string LastName { get; set; }
        public virtual List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        [NotMapped]
        public string FullName { get => $"{FirstName} {(MiddleName is null ? "" : MiddleName + " ")}{LastName}"; }
    }
}
