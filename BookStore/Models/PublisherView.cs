using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class PublisherView
    {
        public int Id { get; set; }
        [DisplayName("Publisher name")]
        public string Name { get; set; }
    }
}
