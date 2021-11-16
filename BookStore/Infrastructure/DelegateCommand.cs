using System;
using System.Threading.Tasks;

namespace BookStore.Infrastructure
{
    internal sealed class DelegateCommand : Command
    {
        private static readonly Func<bool> defaultCanExecuteMethod = () => true;

        private Func<bool> canExecuteMethod;
        private readonly Func<Task> executeMethod;

        public DelegateCommand(Func<Task> executeMethod) :
            this(executeMethod, defaultCanExecuteMethod)
        { }

        public DelegateCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod)
        {
            this.canExecuteMethod = canExecuteMethod;
            this.executeMethod = executeMethod;
        }

        public override bool CanExecute() => canExecuteMethod();

        public async override Task ExecuteAsync() => await executeMethod();
        public async override Task ExecuteAsync(object parameter) { await Task.CompletedTask; }
    }
}
