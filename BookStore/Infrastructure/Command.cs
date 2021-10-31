using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore.Infrastructure
{
    internal abstract class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute() => true;

        public abstract Task ExecuteAsync();
        public abstract Task ExecuteAsync(object parameter);

        protected async virtual Task OnCanExecuteAsyncChanged(EventArgs e) => await CanExecuteChanged?.InvokeAsync(this, e);
        public async Task RaiseCanExecuteAsyncChanged() => await OnCanExecuteAsyncChanged(EventArgs.Empty);

        bool ICommand.CanExecute(object parameter) => CanExecute();

        async void ICommand.Execute(object parameter)
        {
            try
            {
                await ExecuteAsync();
                await ExecuteAsync(parameter);
            }
            catch(Exception ex) { }
        }
    }
}
