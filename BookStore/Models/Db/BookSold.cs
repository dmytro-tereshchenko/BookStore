using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models.Db
{
    public class BookSold
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public int BookInStoreId { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime DateSold { get; set; }
        public int? AccountId { get; set; }
        [Required, Column(TypeName = "decimal(8,2)")]
        public decimal SoldPrice { get; set; }
        [ForeignKey("BookInStoreId")]
        public virtual BookInStore BookInStore { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
