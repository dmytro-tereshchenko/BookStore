using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BookSeriesView
    {
        public int Id { get; set; }
        [DisplayName("Book series")]
        public string Name { get; set; }
    }
}
