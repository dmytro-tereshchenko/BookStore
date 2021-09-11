using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.Db
{
    public class BookSeries
    {
        [Key, Required]
        public int Id { get; set; }
        [MaxLength(40)]
#nullable enable
        public string? Name { get; set; }
#nullable disable
        public virtual List<BookSeriesBook> BookSeriesBooks { get; set; } = new List<BookSeriesBook>();
    }
}
