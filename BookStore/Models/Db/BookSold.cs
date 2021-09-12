using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.Db
{
    public class BookSold
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public int BookInStoreId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime DateSold { get; set; }
        [Required, Column(TypeName = "decimal(5,2)")]
        public decimal SoldPrice { get; set; }
        [ForeignKey("BookInStoreId")]
        public virtual BookInStore BookInStore { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}
