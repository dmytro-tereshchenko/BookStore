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
        private ICommand mostPopularAuthorsView;
        private ICommand mostPopularGenresView;
        private ICommand reservedBooksView;
        public MainViewModel(DbSqlRepository storeRepository)
        {
            this.storeRepository = storeRepository;
            storeRepository.CurrentUserChanged += OnCurrentUserChanged;
            storeRepository.ResultBooksViewChanged += OnResultBooksViewChanged;
            storeRepository.ResultSimpleEntitiesViewChanged += OnResultSimpleEnitiesViewChanged;
            storeRepository.ResultBooksReservedViewChanged += OnResultReservedBooksViewChanged;
            IsPeriodBarUsed = Visibility.Collapsed;
            IsResultBooksUsed = Visibility.Visible;
            IsSimpleEntitiesUsed = Visibility.Collapsed;
            IsReservedBookUsed = Visibility.Collapsed;
            login = new DialogCommand(LoginUser); ;
            logout = new DelegateCommand(LogoutUser);
            allBooksView = new DelegateCommand(ShowAllBooks);
            newBooksView = new DelegateCommand(ShowNewBooks);
            bestSellingBooksView = new DelegateCommand(ShowBestSellingBooks);
            mostPopularAuthorsView = new DelegateCommand(ShowMostPopularAuthors);
            mostPopularGenresView = new DelegateCommand(ShowMostPopularGenres);
            reservedBooksView = new DelegateCommand(ShowReservedBooks);
        }
        public Visibility IsAdmin { get => (storeRepository?.CurrentUser?.Admin ?? false) == true ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsLogIn { get => storeRepository?.CurrentUser != null ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsLogOut { get => IsLogIn == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsPeriodBarUsed { get; set; }
        public Visibility IsResultBooksUsed { get; set; }
        public Visibility IsSimpleEntitiesUsed { get; set; }
        public Visibility IsReservedBookUsed { get; set; }
        public List<BookView> ResultBooksView { get => storeRepository.ResultBooks; set => storeRepository.ResultBooks = value; }
        public List<SimpleEntityView> ResultSimpleEnitiesView { get => storeRepository.ResultSimpleEntities; set => storeRepository.ResultSimpleEntities = value; }
        public List<BookReservedView> ResultReservedBooksView { get => storeRepository.ResultBooksReserved; set => storeRepository.ResultBooksReserved = value; }
        public ICommand Login => login;
        public ICommand Logout => logout;
        public ICommand AllBooksView => allBooksView;
        public ICommand NewBooksView => newBooksView;
        public ICommand BestSellingBooksView => bestSellingBooksView;
        public ICommand MostPopularAuthorsView => mostPopularAuthorsView;
        public ICommand MostPopularGenresView => mostPopularGenresView;
        public ICommand ReservedBooksView => reservedBooksView;
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
        private void OnResultBooksViewChanged(object sender, EventArgs e)
        {
            IsSimpleEntitiesUsed = Visibility.Collapsed;
            IsReservedBookUsed = Visibility.Collapsed;
            IsResultBooksUsed = Visibility.Visible;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(ResultBooksView)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TableName)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSimpleEntitiesUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsReservedBookUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsResultBooksUsed)));
        }
        private void OnResultSimpleEnitiesViewChanged(object sender, EventArgs e)
        {
            IsResultBooksUsed = Visibility.Collapsed;
            IsReservedBookUsed = Visibility.Collapsed;
            IsSimpleEntitiesUsed = Visibility.Visible;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(ResultSimpleEnitiesView)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TableName)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsResultBooksUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsReservedBookUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSimpleEntitiesUsed)));
        }
        private void OnResultReservedBooksViewChanged(object sender, EventArgs e)
        {
            IsResultBooksUsed = Visibility.Collapsed;
            IsSimpleEntitiesUsed = Visibility.Collapsed;
            IsReservedBookUsed = Visibility.Visible;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(ResultReservedBooksView)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TableName)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsResultBooksUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSimpleEntitiesUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsReservedBookUsed)));
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
        private void ShowMostPopularAuthors() => storeRepository.MostPopularAuthorsView();
        private void ShowMostPopularGenres() => storeRepository.MostPopularGenresView();
        private void ShowReservedBooks() => storeRepository.ReservedBooksView();
    }
}
