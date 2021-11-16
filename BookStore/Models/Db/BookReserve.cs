using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models.Db
{
    public class BookReserve
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public int BookInStoreId { get; set; }
#nullable enable
        public string? Description { get; set; }
#nullable disable
        public int? AccountId { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime DateReserve { get; set; }
        [ForeignKey("BookInStoreId")]
        public virtual BookInStore BookInStore { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
