using System.ComponentModel;

namespace BookStore.Interfaces
{
    internal interface IViewModel
    {
        event PropertyChangedEventHandler PropertyChanged;
    }
}
