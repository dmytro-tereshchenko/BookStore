using BookStore.Infrastructure;
using BookStore.Models.Presenters;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookStore.ViewModels
{
    internal class BookSeriesViewModel : ViewModel
    {
        private BookSeriesModel model;
        private ICommand ok;
        private ICommand cancel;
        public BookSeriesViewModel(BookSeriesModel model, bool isEdit = true)
        {
            this.model = model;
            ok = isEdit ? new DialogCommand(EditBookSeries) : new DialogCommand(CreateBookSeries);
            cancel = new DialogCommand(CloseWindow);
            model.MessageChanged += OnMessageChanged;
        }
        public ICommand Ok { get => ok; }
        public ICommand Cancel { get => cancel; }
        public BookSeriesView BookSeries { get => model.BookSeries; }
        private async Task CreateBookSeries(object window)
        {
            await model.AddBookSeries();
            if (window is Window)
            {
                (window as Window).DialogResult = true;
            }
        }
        private async Task EditBookSeries(object window)
        {
            await model.EditBookSeries();
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
    }
}
