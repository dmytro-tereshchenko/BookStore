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

        public abstract void Execute();
        public abstract void Execute(object parameter);

        protected virtual void OnCanExecuteChanged(EventArgs e) => CanExecuteChanged?.Invoke(this, e);
        public void RaiseCanExecuteChanged() => OnCanExecuteChanged(EventArgs.Empty);

        bool ICommand.CanExecute(object parameter) => CanExecute();

        void ICommand.Execute(object parameter)
        {
            Execute();
            Execute(parameter);
        }
    }
}
