using BookStore.Infrastructure;
using BookStore.Models;
using BookStore.Models.Presenters;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookStore.ViewModels
{
    internal class PublisherViewModel : ViewModel
    {
        private PublisherModel model;
        private ICommand ok;
        private ICommand cancel;
        public PublisherViewModel(PublisherModel model, bool isEdit = true)
        {
            this.model = model;
            ok = isEdit ? new DialogCommand(EditPublisher) : new DialogCommand(CreatePublisher);
            cancel = new DialogCommand(CloseWindow);
            model.MessageChanged += OnMessageChanged;
        }
        public ICommand Ok { get => ok; }
        public ICommand Cancel { get => cancel; }
        public PublisherView Publisher { get => model.Publisher; }
        private async Task CreatePublisher(object window)
        {
            await model.AddPublisher();
            if (window is Window)
            {
                (window as Window).DialogResult = true;
            }
        }
        private async Task EditPublisher(object window)
        {
            await model.EditPublisher();
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
