using BookStore.Infrastructure;
using BookStore.Models;
using BookStore.Models.Presenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookStore.ViewModels
{
    internal class BookInStoreViewModel : ViewModel
    {
        private BookInStoreModel model;
        private ICommand ok;
        private ICommand cancel;
        private ICommand searchBook;
        private ICommand setBook;
        public BookInStoreViewModel(BookInStoreModel model, bool isEdit = true)
        {
            this.model = model;
            ok = isEdit ? new DialogCommand(EditBookInStore) : new DialogCommand(CreateBookInStore);
            cancel = new DialogCommand(CloseWindow);
            searchBook = new DelegateCommand(SearchBookFromDb);
            setBook = new DialogCommand(SetBookFromDb);
            model.MessageChanged += OnMessageChanged;
            model.PropertyChanged += OnPropertyModelChanged;
        }
        public ICommand Ok { get => ok; }
        public ICommand Cancel { get => cancel; }
        public ICommand SearchBook { get => searchBook; }
        public ICommand SetBook { get => setBook; }
        public BookInStoreView BookInStore { get => model.BookInStore; }
        public string SearchBookName { get => model.SearchBookName; set => model.SearchBookName = value; }
        public IEnumerable<BookView> Books { get => model.Books; set => model.Books = value; }
        private async Task SearchBookFromDb()
        {
            await model.SearchBook();
        }
        private async Task SetBookFromDb(object book)
        {
            if (book is not null)
            {
                await model.SetBook(book as BookView);
            }
        }
        private async Task CreateBookInStore(object window)
        {
            await model.AddBookInStore();
            if (window is Window)
            {
                (window as Window).DialogResult = true;
            }
        }
        private async Task EditBookInStore(object window)
        {
            await model.EditBookInStore();
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
