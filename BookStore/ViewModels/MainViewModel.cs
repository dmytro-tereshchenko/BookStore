using BookStore.Infrastructure;
using BookStore.Interfaces;
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
        private ICommand allBooksView;
        private ICommand newBooksView;
        private ICommand bestSellingBooksView;
        public MainViewModel(DbSqlRepository storeRepository)
        {
            this.storeRepository = storeRepository;
            storeRepository.CurrentUserChanged += OnCurrentUserChanged;
            storeRepository.ResultViewChanged += OnResultViewChanged;
            IsPeriodBarUsed = Visibility.Collapsed;
            login = new DialogCommand(LoginUser); ;
            logout = new DelegateCommand(LogoutUser);
            allBooksView = new DelegateCommand(ShowAllBooks);
            newBooksView = new DelegateCommand(ShowNewBooks);
            bestSellingBooksView = new DelegateCommand(ShowBestSellingBooks);
        }
        public Visibility IsAdmin { get => (storeRepository?.CurrentUser?.Admin ?? false) == true ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsLogIn { get => storeRepository?.CurrentUser != null ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsLogOut { get => IsLogIn == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsPeriodBarUsed { get; set; }
        public List<BookView> ResultView { get => storeRepository.ResultBooks; set => storeRepository.ResultBooks = value; }
        public ICommand Login => login;
        public ICommand Logout => logout;
        public ICommand AllBooksView => allBooksView;
        public ICommand NewBooksView => newBooksView;
        public ICommand BestSellingBooksView => bestSellingBooksView;
        public string LoginField 
        { 
            get => storeRepository.LoginField;
            set => storeRepository.LoginField = value;
        }
        public string TableName
        {
            get => storeRepository.TableName;
            set => storeRepository.TableName = value;
        }
        public string LoginText { get => storeRepository?.CurrentUser?.Login ?? ""; }
        private void OnCurrentUserChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAdmin)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLogIn)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLogOut)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(LoginText)));
        }
        private void OnResultViewChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(ResultView)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TableName)));
        }
        private void LoginUser(object password)
        {
            storeRepository.UserLogIn((password as PasswordBox).Password);
            (password as PasswordBox).Password = "";
        }
        
        private void LogoutUser() => storeRepository.UserLogout();
        private void ShowAllBooks() => storeRepository.AllBooksView();
        private void ShowNewBooks() => storeRepository.NewBooksView();
        private void ShowBestSellingBooks() => storeRepository.BestSellingBooksView();
    }
}
