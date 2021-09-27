using BookStore.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    internal class DbSqlRepository
    {
        private DbContextOptions<StoreContext> options;
        private Account currentUser;
        private string loginField;
        /*private string passwordField;*/
        public DbSqlRepository(DbContextOptions<StoreContext> options)
        {
            this.options = options;
            currentUser = null;
        }
        public Account CurrentUser { get => currentUser; }
        public string LoginField { get => loginField; set => loginField = value; }
        /*public string PasswordField { get => passwordField; set => passwordField = value; }*/

        public event EventHandler<EventArgs> CurrentUserChanged;

        private void OnCurrentUserChanged(EventArgs e) => CurrentUserChanged?.Invoke(this, e);
       /* private void OnCurrentUserChanged(EventArgs e)
        {
            var eventListeners = CurrentUserChanged.GetInvocationList();

            Console.WriteLine("Raising Event");
            for (int index = 0; index < eventListeners.Count(); index++)
            {
                var methodToInvoke = (EventHandler)eventListeners[index];
                methodToInvoke.BeginInvoke(this, e, EndAsyncEventCurrentUserChanged, null);
            }
            //CurrentUserChanged?.Invoke(this, e);
        }*/
        /*private void EndAsyncEventCurrentUserChanged(IAsyncResult iar)
        {
            var ar = (System.Runtime.Remoting.Messaging.AsyncResult)iar;
            var invokedMethod = (EventHandler)ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(iar);
            }
            catch
            {
                // Handle any exceptions that were thrown by the invoked method
                Console.WriteLine("An event listener went kaboom!");
            }
        }*/
        public void UserLogIn(string password)
        {
            using (StoreContext db = new StoreContext(options))
            {
                currentUser = db.Accounts.Where(ac => ac.Login == loginField && ac.Password == password).FirstOrDefault();
            }
            if (currentUser != null)
            {
                OnCurrentUserChanged(new PropertyChangedEventArgs(nameof(CurrentUser)));
            }
        }
        public void UserLogout()
        {
            currentUser = null;
            OnCurrentUserChanged(new PropertyChangedEventArgs(nameof(CurrentUser)));
        }
    }
}
