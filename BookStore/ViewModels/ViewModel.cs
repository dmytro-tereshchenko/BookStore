using BookStore.Infrastructure;
using BookStore.Interfaces;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    internal abstract class ViewModel : INotifyPropertyChanged, IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected async virtual Task OnPropertyChanged(PropertyChangedEventArgs e) => await PropertyChanged?.InvokeAsync(this, e);
    }
}
