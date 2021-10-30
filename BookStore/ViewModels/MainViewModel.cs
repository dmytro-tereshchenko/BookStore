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
        private ICommand searchView;
        private ICommand buyBook;
        private ICommand reserveBook;
        private ICommand manageAccountsView;
        private ICommand newAccount;
        private ICommand editAccount;
        private ICommand deleteAccount;
        public MainViewModel(DbSqlRepository storeRepository, NewViewFactory newViewFactory)
        {
            this.storeRepository = storeRepository;
            this.newViewFactory = newViewFactory;
            storeRepository.CurrentUserChanged += OnCurrentUserChanged;
            storeRepository.MessageChanged += OnMessageChanged;
            storeRepository.ResultViewChanged += OnResultViewChanged;
            IsPeriodBarUsed = Visibility.Collapsed;
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
            searchView = new DelegateCommand(SearchBook);
            buyBook = new DialogCommand(BuyBookFromStore, () => IsResultBooksUsed == Visibility.Visible);
            reserveBook = new DialogCommand(ReserveBookInStore, () => IsResultBooksUsed == Visibility.Visible);
            manageAccountsView = new DelegateCommand(ManagedAccountsAdmin);
            newAccount = new DelegateCommand(CreateAccountInRepository);
            editAccount = new DialogCommand(EditAccountInRepository);
            deleteAccount = new DialogCommand(DeleteAccountInRepository);
        }
        /*public Visibility IsAdmin { get => (storeRepository?.CurrentUser?.Admin ?? false) == true ? Visibility.Visible : Visibility.Collapsed; }*/
        public Visibility IsAdmin { get => Visibility.Visible; }
        public Visibility IsLogIn { get => storeRepository?.CurrentUser != null ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsLogOut { get => IsLogIn == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsPeriodBarUsed { get; set; }
        public Visibility IsResultBooksUsed
        {
            get => storeRepository.TypeResultView == TypeResultView.AllBooksView ||
                storeRepository.TypeResultView == TypeResultView.NewBooksView ||
                storeRepository.TypeResultView == TypeResultView.BestSellingBooksView ||
                storeRepository.TypeResultView == TypeResultView.ResultSearchView ? Visibility.Visible : Visibility.Collapsed;
        }
        public Visibility IsSimpleEntitiesUsed
        {
            get => storeRepository.TypeResultView == TypeResultView.MostPopularAuthorsView ||
                storeRepository.TypeResultView == TypeResultView.MostPopularGenresView ? Visibility.Visible : Visibility.Collapsed;
        }
        public Visibility IsReservedBookUsed { get=> storeRepository.TypeResultView == TypeResultView.ReservedBooksView ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsSoldBookUsed { get => storeRepository.TypeResultView == TypeResultView.SoldBooksView ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsManageAccountsUsed { get => storeRepository.TypeResultView == TypeResultView.ResultManageAccountsView ? Visibility.Visible : Visibility.Collapsed; }
        public IEnumerable<BookView> ResultBooksView { get => storeRepository.ResultBooksView; set => storeRepository.ResultBooksView = value; }
        public IEnumerable<SimpleEntityView> ResultSimpleEnitiesView { get => storeRepository.ResultSimpleEnitiesView; set => storeRepository.ResultSimpleEnitiesView = value; }
        public IEnumerable<BookReservedView> ResultReservedBooksView { get => storeRepository.ResultReservedBooksView; set => storeRepository.ResultReservedBooksView = value; }
        public IEnumerable<BookSoldView> ResultSoldBooksView { get => storeRepository.ResultSoldBooksView; set => storeRepository.ResultSoldBooksView = value; }
        public IEnumerable<AccountView> ResultManageAccountsView { get => storeRepository.ResultManageAccountsView; set => storeRepository.ResultManageAccountsView = value; }
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
        public ICommand SearchView => searchView;
        public ICommand BuyBook => buyBook;
        public ICommand ReserveBook => reserveBook;
        public ICommand ManageAccountsView => manageAccountsView;
        public ICommand NewAccount => newAccount;
        public ICommand EditAccount => editAccount;
        public ICommand DeleteAccount => deleteAccount;
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
        private async void OnCurrentUserChanged(object sender, EventArgs e)
        {
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAdmin)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLogIn)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLogOut)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(LoginText)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsPeriodBarUsed)));
        }
        private async void OnResultViewChanged(object sender, PropertyChangedEventArgs e)
        {
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(TableName)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsResultBooksUsed)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSimpleEntitiesUsed)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsReservedBookUsed)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSoldBookUsed)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsPeriodBarUsed)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsManageAccountsUsed)));

            await OnPropertyChanged(e);
        }
        private async void OnMessageChanged(object sender, EventArgs e)
        {
            MessageBox.Show(storeRepository.Message);
            await Task.CompletedTask;
        }
        private async Task LoginUser(object password)
        {
            await storeRepository.UserLogIn((password as PasswordBox).Password);
            (password as PasswordBox).Password = "";
        }
        
        private async Task PeriodChange() => await storeRepository.PeriodChanged();
        private async Task LogoutUser() => await storeRepository.UserLogout();
        private async Task ShowAllBooks()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            await storeRepository.AllBooksView();
        }
        private async Task ShowNewBooks()
        {
            IsPeriodBarUsed = Visibility.Visible;
            await storeRepository.NewBooksView();
        }
        private async Task ShowBestSellingBooks()
        {
            IsPeriodBarUsed = Visibility.Visible;
            await storeRepository.BestSellingBooksView();
        }
        private async Task ShowMostPopularAuthors()
        {
            IsPeriodBarUsed = Visibility.Visible;
            await storeRepository.MostPopularAuthorsView();
        }
        private async Task ShowMostPopularGenres()
        {
            IsPeriodBarUsed = Visibility.Visible;
            await storeRepository.MostPopularGenresView();
        }
        private async Task ShowReservedBooks()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            await storeRepository.ReservedBooksView();
        }
        private async Task ShowSoldBooks()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            await storeRepository.SoldBooksView();
        }
        private async Task SearchBook()
        {
            await storeRepository.SearchBook();
        }
        private async Task BuyBookFromStore(object book)
        {
            if (book is not null)
            {
                await storeRepository.BuyBook(book as BookView);
            }
        }
        private async Task ReserveBookInStore(object book)
        {
            if (book is not null)
            {
                newViewFactory.CreateReserveBookView(storeRepository.DbOptions, book as BookView, storeRepository.CurrentUser);
            }
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(ResultReservedBooksView)));
        }
        private async Task ManagedAccountsAdmin()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            await storeRepository.ManageAccountsView();
        }
        private async Task CreateAccountInRepository()
        {
            if ((newViewFactory.CreateAccountView(storeRepository.DbOptions)).Value)
                await ManagedAccountsAdmin();
        }
        private async Task EditAccountInRepository(object account)
        {
            if ((newViewFactory.CreateAccountView(storeRepository.DbOptions, account as AccountView)).Value)
            await ManagedAccountsAdmin();
        }
        private async Task DeleteAccountInRepository(object account)
        {
            if (account is not null)
            {
                await storeRepository.DeleteAccount(account as AccountView);
            }
        }
    }
}
