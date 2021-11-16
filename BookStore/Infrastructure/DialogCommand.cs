using System;
using System.Threading.Tasks;

namespace BookStore.Infrastructure
{
    internal sealed class DialogCommand : Command
    {
        private static readonly Func<bool> defaultCanExecuteMethod = () => true;

        private Func<bool> canExecuteMethod;
        private readonly Func<object, Task> executeMethod;

        public DialogCommand(Func<object, Task> executeMethod) :
            this(executeMethod, defaultCanExecuteMethod)
        { }

        public DialogCommand(Func<object, Task> executeMethod, Func<bool> canExecuteMethod)
        {
            this.canExecuteMethod = canExecuteMethod;
            this.executeMethod = executeMethod;
        }

        public override bool CanExecute() => canExecuteMethod();

        public async override Task ExecuteAsync(object parameter) => await executeMethod(parameter);
        public async override Task ExecuteAsync() { await Task.CompletedTask; }
    }
}
