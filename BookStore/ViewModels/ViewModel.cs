using BookStore.Infrastructure;
using BookStore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    internal abstract class ViewModel : INotifyPropertyChanged, IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected async virtual Task OnPropertyChanged(PropertyChangedEventArgs e) => await PropertyChanged?.InvokeAsync(this, e);
    }
}
