using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.Db
{
    public class Account
    {
        [Key, Required]
        public int Id { get; set; }
        [MaxLength(20), Required]
        public string Login { get; set; }
        [MaxLength(40), Required]
        public string Password { get; set; }
        [Required]
        public bool Admin { get; set; }
    }
}
