using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class AuthorView
    {
        public int Id { get; set; }
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [DisplayName("Middle name")]
        public string? MiddleName { get; set; }
        [DisplayName("Last name")]
        public string LastName { get; set; }
        public override string ToString()
        {
            return $"{FirstName} {(MiddleName is null ? "" : MiddleName + " ")}{LastName}";
        }
        public override bool Equals(object obj)
        {
            return (obj as AuthorView).Id == this.Id;
        }
    }
}
