using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models.Db
{
    public class Stock
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public int BookInStoreId { get; set; }
        [Required, Column(TypeName = "decimal(4,2)")]
        public decimal Discount { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime DateStart { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime DateEnd { get; set; }
        [ForeignKey("BookInStoreId")]
        public virtual BookInStore BookInStore { get; set; }
    }
}
