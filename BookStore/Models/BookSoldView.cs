using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BookSoldView
    {
        public int Id { get; set; }
        [DisplayName("Book's name")]
        public string BookName { get; set; }
        public string Authors { get; set; }
        [DisplayName("Cost price")]
        public string CostPrice { get; set; }
        [DisplayName("Selling price")]
        public string SoldPrice { get; set; }
        [DisplayName("Accaunt's name")]
        public string AccountLogin { get; set; }
        [DisplayName("Date of sale")]
        public string DateSold { get; set; }
    }
}
