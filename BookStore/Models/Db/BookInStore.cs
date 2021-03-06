using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models.Db
{
    public class BookInStore
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        [Required, Column(TypeName="decimal(8,2)")]
        public decimal CostPrice { get; set; }
        [Required, Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required, Column(TypeName ="date")]
        public DateTime DateAdded { get; set; }
        public virtual List<BookReserve> BookReserves { get; set; } = new List<BookReserve>();
        public virtual List<BookSold> BookSolds { get; set; } = new List<BookSold>();
        public virtual List<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
