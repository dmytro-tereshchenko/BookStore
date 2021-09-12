using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.Db
{
    public class Stock
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required, Column(TypeName = "decimal(2,2)")]
        public decimal Discount { get; set; }
        [Required, DataType(DataType.Date), Column(TypeName = "date")]
        public DateTime DateStart { get; set; }
        [Required, DataType(DataType.Date), Column(TypeName = "date")]
        public DateTime DateEnd { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
    }
}
