using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class StockView
    {
        public int Id { get; set; }
        [DisplayName("Book in store")]
        public string BookInStore { get; set; }
        public string Discount { get; set; }
        [DisplayName("Start date")]
        public string DateStart { get; set; }
        [DisplayName("Date end")]
        public string DateEnd { get; set; }
    }
}
