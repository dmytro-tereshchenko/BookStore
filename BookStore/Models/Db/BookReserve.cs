﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.Db
{
    public class BookReserve
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public int BookInStoreId { get; set; }
        public int? AccountId { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime DateReserve { get; set; }
        [ForeignKey("BookInStoreId")]
        public virtual BookInStore BookInStore { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}