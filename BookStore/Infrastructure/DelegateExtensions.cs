using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure
{
    public static class DelegateExtensions
    {
        public static Task InvokeAsync<TArgs>(this EventHandler<EventArgs> func, object sender, TArgs e)
        {
            return func == null ? Task.CompletedTask
                : Task.WhenAll(func.GetInvocationList().Cast<EventHandler<EventArgs>>().Select((f) => Task.Run(() => f.DynamicInvoke(sender, e))));
        }
        public static Task InvokeAsync(this EventHandler func, object sender, EventArgs e)
        {
            return func == null ? Task.CompletedTask
                : Task.WhenAll(func.GetInvocationList().Cast<EventHandler>().Select((f) => Task.Run(() => f.DynamicInvoke(sender, e))));
        }
        public static Task InvokeAsync(this PropertyChangedEventHandler func, object sender, PropertyChangedEventArgs e)
        {
            return func == null ? Task.CompletedTask
                : Task.WhenAll(func.GetInvocationList().Cast<PropertyChangedEventHandler>().Select((f) => Task.Run(() => f.DynamicInvoke(sender, e))));
        }
        public static Task InvokeAsync<TArgs>(this EventHandler<PropertyChangedEventArgs> func, object sender, TArgs e)
        {
            return func == null ? Task.CompletedTask
                : Task.WhenAll(func.GetInvocationList().Cast<EventHandler<PropertyChangedEventArgs>>().Select((f) => Task.Run(() => f.DynamicInvoke(sender, e))));
        }
    }
}
