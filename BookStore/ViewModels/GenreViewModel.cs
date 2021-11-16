using BookStore.Infrastructure;
using BookStore.Models;
using BookStore.Models.Presenters;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookStore.ViewModels
{
    internal class GenreViewModel : ViewModel
    {
        private GenreModel model;
        private ICommand ok;
        private ICommand cancel;
        public GenreViewModel(GenreModel model, bool isEdit = true)
        {
            this.model = model;
            ok = isEdit ? new DialogCommand(EditGenre) : new DialogCommand(CreateGenre);
            cancel = new DialogCommand(CloseWindow);
            model.MessageChanged += OnMessageChanged;
        }
        public ICommand Ok { get => ok; }
        public ICommand Cancel { get => cancel; }
        public GenreView Genre { get => model.Genre; }
        private async Task CreateGenre(object window)
        {
            await model.AddGenre();
            if (window is Window)
            {
                (window as Window).DialogResult = true;
            }
        }
        private async Task EditGenre(object window)
        {
            await model.EditGenre();
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
