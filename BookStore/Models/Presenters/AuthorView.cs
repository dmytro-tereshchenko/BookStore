using BookStore.Models.Db;
using System.ComponentModel;

namespace BookStore.Models.Presenters
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
            return obj is not AuthorView ? false : (obj as AuthorView).Id == this.Id;
        }
        public static explicit operator Author(AuthorView obj) =>
            new Author { Id = obj.Id, FirstName = obj.FirstName, MiddleName = obj.MiddleName, LastName = obj.LastName };
    }
}
