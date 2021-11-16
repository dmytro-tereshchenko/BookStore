using BookStore.Models.Db;
using System.ComponentModel;

namespace BookStore.Models.Presenters
{
    public class PublisherView
    {
        public int Id { get; set; }
        [DisplayName("Publisher name")]
        public string Name { get; set; }
        public static explicit operator Publisher(PublisherView obj) =>
            new Publisher { Id = obj.Id, Name = obj.Name };
    }
}
