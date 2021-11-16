using System.ComponentModel;

namespace BookStore.Models.Presenters
{
    public class PublisherView
    {
        public int Id { get; set; }
        [DisplayName("Publisher name")]
        public string Name { get; set; }
    }
}
