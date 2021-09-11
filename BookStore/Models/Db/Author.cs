using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.Db
{
    public class Author
    {
        [Key, Required]
        public int Id { get; set; }
        [MaxLength(30), Required]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string MiddleName { get; set; }
        [MaxLength(30), Required]
        public string LastName { get; set; }
        public virtual List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        [NotMapped]
        public string FullName { get => $"{FirstName} {(MiddleName is null ? "" : MiddleName + " ")}{LastName}"; }
    }
}
