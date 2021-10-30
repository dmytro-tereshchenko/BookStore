using BookStore.Infrastructure;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookStore.ViewModels
{
    internal class AccountViewModel : ViewModel
    {
        private AccountModel model;
        private ICommand ok;
        private ICommand cancel;
        public AccountViewModel(AccountModel model, bool isEdit = true)
        {
            this.model = model;
            ok = isEdit ? new DialogCommand(EditAccount) : new DialogCommand(CreateAccount);
            cancel = new DialogCommand(CloseWindow);
            model.MessageChanged += OnMessageChanged;
        }
        public ICommand Ok { get => ok; }
        public ICommand Cancel { get => cancel; }
        public AccountView Account { get => model.Account; }
        private async Task CreateAccount(object window)
        {
            await model.AddAccount();
            if (window is Window)
            {
                (window as Window).DialogResult = true;
            }
        }
        private async Task EditAccount(object window)
        {
            await model.EditAccount();
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
