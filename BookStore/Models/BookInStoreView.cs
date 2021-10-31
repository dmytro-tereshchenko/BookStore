using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BookInStoreView
    {
        public int Id { get; set; }
        public string Book { get; set; }
        [DisplayName("Cost price")]
        public string CostPrice { get; set; }
        public string Price { get; set; }
        public int Amount { get; set; }
        [DisplayName("Date added")]
        public string DateAdded { get; set; }
    }
}
