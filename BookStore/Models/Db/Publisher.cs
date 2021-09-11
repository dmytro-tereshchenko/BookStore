using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.Db
{
    public class Publisher
    {
        [Key, Required]
        public int Id { get; set; }
        [MaxLength(40), Required]
        public string Name { get; set; }
        public virtual List<Book> Books { get; set; } = new List<Book>();
    }
}
