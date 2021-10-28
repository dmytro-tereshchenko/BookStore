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
        private NewViewFactory newViewFactory;
        private ICommand login;
        private ICommand logout;
        private ICommand allBooksView;
        private ICommand newBooksView;
        private ICommand bestSellingBooksView;
        private ICommand mostPopularAuthorsView;
        private ICommand mostPopularGenresView;
        private ICommand reservedBooksView;
        private ICommand soldBooksView;
        private ICommand periodChanged;
        private ICommand search;
        private ICommand buyBook;
        private ICommand reserveBook;
        public MainViewModel(DbSqlRepository storeRepository, NewViewFactory newViewFactory)
        {
            this.storeRepository = storeRepository;
            this.newViewFactory = newViewFactory;
            storeRepository.CurrentUserChanged += OnCurrentUserChanged;
            storeRepository.ResultBooksViewChanged += OnResultBooksViewChanged;
            storeRepository.ResultSimpleEntitiesViewChanged += OnResultSimpleEnitiesViewChanged;
            storeRepository.ResultBooksReservedViewChanged += OnResultReservedBooksViewChanged;
            storeRepository.ResultBooksSoldViewChanged += OnResultSoldBooksViewChanged;
            storeRepository.MessageChanged += OnMessageChanged;
            IsPeriodBarUsed = Visibility.Collapsed;
            IsResultBooksUsed = Visibility.Visible;
            IsSimpleEntitiesUsed = Visibility.Collapsed;
            IsReservedBookUsed = Visibility.Collapsed;
            IsSoldBookUsed = Visibility.Collapsed;
            login = new DialogCommand(LoginUser); ;
            logout = new DelegateCommand(LogoutUser);
            periodChanged = new DelegateCommand(PeriodChange);
            allBooksView = new DelegateCommand(ShowAllBooks);
            newBooksView = new DelegateCommand(ShowNewBooks);
            bestSellingBooksView = new DelegateCommand(ShowBestSellingBooks);
            mostPopularAuthorsView = new DelegateCommand(ShowMostPopularAuthors);
            mostPopularGenresView = new DelegateCommand(ShowMostPopularGenres);
            reservedBooksView = new DelegateCommand(ShowReservedBooks);
            soldBooksView = new DelegateCommand(ShowSoldBooks);
            search = new DelegateCommand(SearchBook);
            buyBook = new DialogCommand(BuyBookFromStore, () => IsResultBooksUsed == Visibility.Visible);
            reserveBook = new DialogCommand(ReserveBookInStore, () => IsResultBooksUsed == Visibility.Visible);
        }
        public Visibility IsAdmin { get => (storeRepository?.CurrentUser?.Admin ?? false) == true ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsLogIn { get => storeRepository?.CurrentUser != null ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsLogOut { get => IsLogIn == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsPeriodBarUsed { get; set; }
        public Visibility IsResultBooksUsed { get; set; }
        public Visibility IsSimpleEntitiesUsed { get; set; }
        public Visibility IsReservedBookUsed { get; set; }
        public Visibility IsSoldBookUsed { get; set; }
        public IEnumerable<BookView> ResultBooksView { get => storeRepository.ResultBooks; set => storeRepository.ResultBooks = value; }
        public IEnumerable<SimpleEntityView> ResultSimpleEnitiesView { get => storeRepository.ResultSimpleEntities; set => storeRepository.ResultSimpleEntities = value; }
        public IEnumerable<BookReservedView> ResultReservedBooksView { get => storeRepository.ResultBooksReserved; set => storeRepository.ResultBooksReserved = value; }
        public IEnumerable<BookSoldView> ResultSoldBooksView { get => storeRepository.ResultBooksSold; set => storeRepository.ResultBooksSold = value; }
        public ICommand Login => login;
        public ICommand Logout => logout;
        public ICommand PeriodChanged => periodChanged;
        public ICommand AllBooksView => allBooksView;
        public ICommand NewBooksView => newBooksView;
        public ICommand BestSellingBooksView => bestSellingBooksView;
        public ICommand MostPopularAuthorsView => mostPopularAuthorsView;
        public ICommand MostPopularGenresView => mostPopularGenresView;
        public ICommand ReservedBooksView => reservedBooksView;
        public ICommand SoldBooksView => soldBooksView;
        public ICommand Search => search;
        public ICommand BuyBook => buyBook;
        public ICommand ReserveBook => reserveBook;
        public RadioButtonRepository Period { get => storeRepository.Period; }
        public string LoginField 
        {
            get => storeRepository.LoginField;
            set => storeRepository.LoginField = value;
        }
        public string BookSearch
        {
            get => storeRepository.BookSearch;
            set => storeRepository.BookSearch = value;
        }
        public string AuthorSearch
        {
            get => storeRepository.AuthorSearch;
            set => storeRepository.AuthorSearch = value;
        }
        public string GenreSearch
        {
            get => storeRepository.GenreSearch;
            set => storeRepository.GenreSearch = value;
        }
        public string TableName { get => storeRepository.TableName; }
        public string LoginText { get => storeRepository?.CurrentUser?.Login ?? ""; }
        private void OnCurrentUserChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAdmin)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLogIn)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLogOut)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(LoginText)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsPeriodBarUsed)));
        }
        private void OnResultBooksViewChanged(object sender, EventArgs e)
        {
            IsSimpleEntitiesUsed = Visibility.Collapsed;
            IsReservedBookUsed = Visibility.Collapsed;
            IsSoldBookUsed = Visibility.Collapsed;
            IsResultBooksUsed = Visibility.Visible;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(ResultBooksView)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TableName)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSimpleEntitiesUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsReservedBookUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSoldBookUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsResultBooksUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsPeriodBarUsed)));
        }
        private void OnResultSimpleEnitiesViewChanged(object sender, EventArgs e)
        {
            IsResultBooksUsed = Visibility.Collapsed;
            IsReservedBookUsed = Visibility.Collapsed;
            IsSoldBookUsed = Visibility.Collapsed;
            IsSimpleEntitiesUsed = Visibility.Visible;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(ResultSimpleEnitiesView)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TableName)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsResultBooksUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsReservedBookUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSoldBookUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSimpleEntitiesUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsPeriodBarUsed)));
        }
        private void OnResultReservedBooksViewChanged(object sender, EventArgs e)
        {
            IsResultBooksUsed = Visibility.Collapsed;
            IsSimpleEntitiesUsed = Visibility.Collapsed;
            IsSoldBookUsed = Visibility.Collapsed;
            IsReservedBookUsed = Visibility.Visible;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(ResultReservedBooksView)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TableName)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsResultBooksUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSimpleEntitiesUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSoldBookUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsReservedBookUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsPeriodBarUsed)));
        }
        private void OnResultSoldBooksViewChanged(object sender, EventArgs e)
        {
            IsResultBooksUsed = Visibility.Collapsed;
            IsSimpleEntitiesUsed = Visibility.Collapsed;
            IsReservedBookUsed = Visibility.Collapsed;
            IsSoldBookUsed = Visibility.Visible;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(ResultSoldBooksView)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TableName)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsResultBooksUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSimpleEntitiesUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsReservedBookUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSoldBookUsed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsPeriodBarUsed)));
        }
        private void OnMessageChanged(object sender, EventArgs e)
        {
            MessageBox.Show(storeRepository.Message);
        }
        private void LoginUser(object password)
        {
            storeRepository.UserLogIn((password as PasswordBox).Password);
            (password as PasswordBox).Password = "";
        }
        
        private void PeriodChange() => storeRepository.PeriodChanged();
        private void LogoutUser() => storeRepository.UserLogout();
        private void ShowAllBooks()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            storeRepository.AllBooksView();
        }
        private void ShowNewBooks()
        {
            IsPeriodBarUsed = Visibility.Visible;
            storeRepository.NewBooksView();
        }
        private void ShowBestSellingBooks()
        {
            IsPeriodBarUsed = Visibility.Visible;
            storeRepository.BestSellingBooksView();
        }
        private void ShowMostPopularAuthors()
        {
            IsPeriodBarUsed = Visibility.Visible;
            storeRepository.MostPopularAuthorsView();
        }
        private void ShowMostPopularGenres()
        {
            IsPeriodBarUsed = Visibility.Visible;
            storeRepository.MostPopularGenresView();
        }
        private void ShowReservedBooks()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            storeRepository.ReservedBooksView();
        }
        private void ShowSoldBooks()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            storeRepository.SoldBooksView();
        }
        private void SearchBook()
        {
            storeRepository.SearchBook();
        }
        private void BuyBookFromStore(object book)
        {
            if (book is not null)
            {
                storeRepository.BuyBook(book as BookView);
            }
        }
        private void ReserveBookInStore(object book)
        {
            if (book is not null)
            {
                newViewFactory.CreateReserveBookView(storeRepository.DbOptions, book as BookView, storeRepository.CurrentUser);
            }
        }
    }
}
