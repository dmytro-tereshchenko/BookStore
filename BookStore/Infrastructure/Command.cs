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

        protected async virtual void OnCanExecuteAsyncChanged(EventArgs e) => await CanExecuteChanged?.InvokeAsync(this, e);
        public void RaiseCanExecuteAsyncChanged() => OnCanExecuteAsyncChanged(EventArgs.Empty);

        bool ICommand.CanExecute(object parameter) => CanExecute();

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync();
            ExecuteAsync(parameter);
        }
    }
}
