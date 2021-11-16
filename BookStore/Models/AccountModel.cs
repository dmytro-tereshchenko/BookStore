using BookStore.Infrastructure;
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
    internal class AccountModel
    {
        private DbContextOptions<StoreContext> options;
        private AccountView account;
        public AccountModel(DbContextOptions<StoreContext> options, AccountView account = null)
        {
            this.options = options;
            this.account = account ?? new AccountView();
        }
        public AccountView Account { get => account; }
        public string Message { get; set; }

        public event EventHandler<EventArgs> MessageChanged;

        private async Task OnMessageChanged(EventArgs e) => await MessageChanged?.InvokeAsync(this, e);

        public async Task AddAccount(string password)
        {
            using (StoreContext db = new StoreContext(options))
            {
                Account dbAccount = await db.Accounts.Where(a => a.Login == account.Login).FirstOrDefaultAsync();
                if (dbAccount is not null)
                {
                    Message = "Login already exists";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                db.Accounts.Add(new Account()
                {
                    Id = account.Id,
                    Login = account.Login,
                    Password = password,
                    Admin=account.IsAdmin
                });
                await db.SaveChangesAsync();
            }
            Message = "Account created";
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
        public async Task EditAccount(string password)
        {
            using (StoreContext db = new StoreContext(options))
            {
                Account dbAccount = await db.Accounts.FindAsync(account.Id);
                if (dbAccount is null)
                {
                    Message = "Not found account";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                Account repeatAccount = await db.Accounts.Where(a => a.Login == account.Login).FirstOrDefaultAsync();
                if (repeatAccount is not null && dbAccount.Login != repeatAccount.Login)
                {
                    Message = "Login already exists";
                    await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
                    return;
                }
                dbAccount.Login = account.Login;
                dbAccount.Password = password == "" ? dbAccount.Password : password;
                dbAccount.Admin = account.IsAdmin;
                db.Entry<Account>(dbAccount).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            Message = "Account edited";
            await OnMessageChanged(new PropertyChangedEventArgs(nameof(Message)));
        }
    }
}
