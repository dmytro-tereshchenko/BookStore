using BookStore.Infrastructure;
using BookStore.Models;
using BookStore.Models.Presenters;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookStore.ViewModels
{
    internal class AuthorViewModel : ViewModel
    {
        private AuthorModel model;
        private ICommand ok;
        private ICommand cancel;
        public AuthorViewModel(AuthorModel model, bool isEdit = true)
        {
            this.model = model;
            ok = isEdit ? new DialogCommand(EditAuthor) : new DialogCommand(CreateAuthor);
            cancel = new DialogCommand(CloseWindow);
            model.MessageChanged += OnMessageChanged;
        }
        public ICommand Ok { get => ok; }
        public ICommand Cancel { get => cancel; }
        public AuthorView Author { get => model.Author; }
        private async Task CreateAuthor(object window)
        {
            await model.AddAuthor();
            if (window is Window)
            {
                (window as Window).DialogResult = true;
            }
        }
        private async Task EditAuthor(object window)
        {
            await model.EditAuthor();
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
