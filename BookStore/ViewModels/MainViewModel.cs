using BookStore.Infrastructure;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore.ViewModels
{
    internal class MainViewModel : ViewModel
    {
        private DbSqlRepository storeRepository;
        private ICommand login;
        private ICommand logout;
        public MainViewModel(DbSqlRepository storeRepository)
        {
            this.storeRepository = storeRepository;
            storeRepository.CurrentUserChanged += OnCurrentUserChanged;
            /*IsAdmin = Visibility.Collapsed;
            IsLogIn = Visibility.Collapsed;*/
            IsPeriodBarUsed = Visibility.Collapsed;
            login = new DialogCommand(LoginUser /*() => !String.IsNullOrEmpty(LoginField)*/);
            logout = new DelegateCommand(LogoutUser);
        }
        public Visibility IsAdmin { get => (storeRepository?.CurrentUser?.Admin ?? false) == true ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsLogIn { get => storeRepository?.CurrentUser != null ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsLogOut { get => IsLogIn == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsPeriodBarUsed { get; set; }
        public ICommand Login => login;
        public ICommand Logout => logout;
        public string LoginField 
        { 
            get => storeRepository.LoginField; 
            set => storeRepository.LoginField = value; 
        }
        /*public string PasswordField 
        { 
            get => storeRepository.PasswordField; 
            set => storeRepository.PasswordField = value; 
        }*/
        public string LoginText { get => storeRepository?.CurrentUser?.Login ?? ""; }
        private void OnCurrentUserChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAdmin)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLogIn)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLogOut)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(LoginText)));
        }
        private void LoginUser(object password)
        {
            storeRepository.UserLogIn((password as PasswordBox).Password);
            (password as PasswordBox).Password = "";
        }
        
        private void LogoutUser() => storeRepository.UserLogout();
    }
}
