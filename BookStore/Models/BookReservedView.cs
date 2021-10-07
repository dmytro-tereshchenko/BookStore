using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
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
