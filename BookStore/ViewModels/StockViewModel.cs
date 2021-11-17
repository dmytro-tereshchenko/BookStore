using BookStore.Infrastructure;
using BookStore.Models;
using BookStore.Models.Presenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookStore.ViewModels
{
    internal class StockViewModel : ViewModel
    {
        private StockModel model;
        private ICommand ok;
        private ICommand cancel;
        private ICommand searchBookInStore;
        private ICommand setBookInStore;
        public StockViewModel(StockModel model, bool isEdit = true)
        {
            this.model = model;
            ok = isEdit ? new DialogCommand(EditStock) : new DialogCommand(CreateStock);
            cancel = new DialogCommand(CloseWindow);
            searchBookInStore = new DelegateCommand(SearchBook);
            setBookInStore = new DialogCommand(SetBook);
            model.MessageChanged += OnMessageChanged;
            model.PropertyChanged += OnPropertyModelChanged;
        }
        public ICommand Ok { get => ok; }
        public ICommand Cancel { get => cancel; }
        public ICommand SearchBookInStore { get => searchBookInStore; }
        public ICommand SetBookInStore { get => setBookInStore; }
        public string SearchBookName { get => model.SearchBookName; set => model.SearchBookName = value; }
        public IEnumerable<BookInStoreView> BookInStores { get => model.BookInStores; set => model.BookInStores = value; }
        public StockView Stock { get => model.Stock; set => model.Stock = value; }
        public DateTime StartDate { get => model.StartDate; set => model.StartDate = value; }
        public DateTime EndDate { get => model.EndDate; set => model.EndDate = value; }
        private async Task SearchBook()
        {
            await model.SearchBookInStore();
        }
        private async Task SetBook(object bookInStore)
        {
            if (bookInStore is not null)
            {
                await model.SetBookInStore(bookInStore as BookInStoreView);
            }
        }
        private async Task CreateStock(object window)
        {
            await model.AddStock();
            if (window is Window)
            {
                (window as Window).DialogResult = true;
            }
        }
        private async Task EditStock(object window)
        {
            await model.EditStock();
            if (window is Window)
            {
                (window as Window).DialogResult = true;
            }
        }
        private async Task CloseWindow(object window)
        {
            if (window is Window)
            {
                (window as Window).DialogResult = false;
            }
            await Task.CompletedTask;
        }
        private void OnMessageChanged(object sender, EventArgs e)
        {
            MessageBox.Show(model.Message);
        }
        private async void OnPropertyModelChanged(object sender, PropertyChangedEventArgs e)
        {
            await OnPropertyChanged(e);
        }
    }
}
