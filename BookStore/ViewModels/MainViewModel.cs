using BookStore.Infrastructure;
using BookStore.Interfaces;
using BookStore.Models.Db;
using BookStore.Models.Presenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore.ViewModels
{
    internal class MainViewModel : ViewModel
    {
        private IDbRepository<StoreContext> storeRepository;
        private INewViewFactory<StoreContext> newViewFactory;
        private IDictionary<string, ICommand> commands;
        public MainViewModel(IDbRepository<StoreContext> storeRepository, INewViewFactory<StoreContext> newViewFactory)
        {
            this.storeRepository = storeRepository;
            this.newViewFactory = newViewFactory;
            storeRepository.CurrentUserChanged += OnCurrentUserChanged;
            storeRepository.MessageChanged += OnMessageChanged;
            storeRepository.ResultViewChanged += OnResultViewChanged;
            IsPeriodBarUsed = Visibility.Collapsed;
            commands = new Dictionary<string, ICommand>();
            commands.Add("Login", new DialogCommand(LoginUser));
            commands.Add("Logout", new DelegateCommand(LogoutUser));
            commands.Add("PeriodChanged", new DelegateCommand(PeriodChange));
            commands.Add("AllBooksView", new DelegateCommand(ShowAllBooks));
            commands.Add("NewBooksView", new DelegateCommand(ShowNewBooks));
            commands.Add("BestSellingBooksView", new DelegateCommand(ShowBestSellingBooks));
            commands.Add("MostPopularAuthorsView", new DelegateCommand(ShowMostPopularAuthors));
            commands.Add("MostPopularGenresView", new DelegateCommand(ShowMostPopularGenres));
            commands.Add("ReservedBooksView", new DelegateCommand(ShowReservedBooks));
            commands.Add("SoldBooksView", new DelegateCommand(ShowSoldBooks));
            commands.Add("SearchView", new DelegateCommand(SearchBook));
            commands.Add("BuyBook", new DialogCommand(BuyBookFromStore, () => IsResultBooksUsed == Visibility.Visible));
            commands.Add("ReserveBook", new DialogCommand(ReserveBookInStore, () => IsResultBooksUsed == Visibility.Visible));
            commands.Add("ManageAccountsView", new DelegateCommand(ManagedAccountsAdmin));
            commands.Add("ManageAuthorsView", new DelegateCommand(ManagedAuthorsAdmin));
            commands.Add("ManageGenresView", new DelegateCommand(ManagedGenresAdmin));
            commands.Add("ManagePublishersView", new DelegateCommand(ManagedPublishersAdmin));
            commands.Add("ManageBooksView", new DelegateCommand(ManagedBooksAdmin));
            commands.Add("ManageBooksInStoreView", new DelegateCommand(ManagedBooksInStoreAdmin));
            commands.Add("ManageStocksView", new DelegateCommand(ManagedStocksAdmin));
            commands.Add("ManageBookSeriesView", new DelegateCommand(ManagedBookSeriesAdmin));
            commands.Add("NewAccount", new DelegateCommand(CreateAccountInRepository));
            commands.Add("NewAuthor", new DelegateCommand(CreateAuthorInRepository));
            commands.Add("NewGenre", new DelegateCommand(CreateGenreInRepository));
            commands.Add("NewPublisher", new DelegateCommand(CreatePublisherInRepository));
            commands.Add("NewBook", new DelegateCommand(CreateBookInRepository));
            commands.Add("NewBookInStore", new DelegateCommand(CreateBookInStoreInRepository));
            commands.Add("NewStock", new DelegateCommand(CreateStockInRepository));
            commands.Add("NewBookSeries", new DelegateCommand(CreateBookSeriesInRepository));
            commands.Add("EditAccount", new DialogCommand(EditAccountInRepository));
            commands.Add("EditAuthor", new DialogCommand(EditAuthorInRepository));
            commands.Add("EditGenre", new DialogCommand(EditGenreInRepository));
            commands.Add("EditPublisher", new DialogCommand(EditPublisherInRepository));
            commands.Add("EditBook", new DialogCommand(EditBookInRepository));
            commands.Add("EditBookInStore", new DialogCommand(EditBookInStoreInRepository));
            commands.Add("EditStock", new DialogCommand(EditStockInRepository));
            commands.Add("EditBookSeries", new DialogCommand(EditBookSeriesInRepository));
            commands.Add("DeleteAccount", new DialogCommand(DeleteAccountInRepository));
            commands.Add("DeleteAuthor", new DialogCommand(DeleteAuthorInRepository));
            commands.Add("DeleteGenre", new DialogCommand(DeleteGenreInRepository));
            commands.Add("DeletePublisher", new DialogCommand(DeletePublisherInRepository));
            commands.Add("DeleteBook", new DialogCommand(DeleteBookInRepository));
            commands.Add("DeleteBookInStore", new DialogCommand(DeleteBookInStoreInRepository));
            commands.Add("DeleteStock", new DialogCommand(DeleteStockInRepository));
            commands.Add("DeleteBookSeries", new DialogCommand(DeleteBookSeriesInRepository));
        }
        public Visibility IsAdmin { get => (storeRepository?.CurrentUser?.Admin ?? false) == true ? Visibility.Visible : Visibility.Collapsed; }
        //public Visibility IsAdmin { get => Visibility.Visible; } //testing
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
        public Visibility IsManageAuthorsUsed { get => storeRepository.TypeResultView == TypeResultView.ResultManageAuthorsView ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsManageGenresUsed { get => storeRepository.TypeResultView == TypeResultView.ResultManageGenresView ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsManagePublishersUsed { get => storeRepository.TypeResultView == TypeResultView.ResultManagePublishersView ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsManageBooksUsed { get => storeRepository.TypeResultView == TypeResultView.ResultManageBooksView ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsManageBooksInStoreUsed { get => storeRepository.TypeResultView == TypeResultView.ResultManageBooksInStoreView ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsManageStocksUsed { get => storeRepository.TypeResultView == TypeResultView.ResultManageStocksView ? Visibility.Visible : Visibility.Collapsed; }
        public Visibility IsManageBookSeriesUsed { get => storeRepository.TypeResultView == TypeResultView.ResultManageBookSeriesView ? Visibility.Visible : Visibility.Collapsed; }
        public IEnumerable<BookViewShow> ResultBooksView { get => storeRepository.ResultBooksView; set => storeRepository.ResultBooksView = value; }
        public IEnumerable<SimpleEntityView> ResultSimpleEnitiesView { get => storeRepository.ResultSimpleEnitiesView; set => storeRepository.ResultSimpleEnitiesView = value; }
        public IEnumerable<BookReservedView> ResultReservedBooksView { get => storeRepository.ResultReservedBooksView; set => storeRepository.ResultReservedBooksView = value; }
        public IEnumerable<BookSoldView> ResultSoldBooksView { get => storeRepository.ResultSoldBooksView; set => storeRepository.ResultSoldBooksView = value; }
        public IEnumerable<AccountView> ResultManageAccountsView { get => storeRepository.ResultManageAccountsView; set => storeRepository.ResultManageAccountsView = value; }
        public IEnumerable<AuthorView> ResultManageAuthorsView { get => storeRepository.ResultManageAuthorsView; set => storeRepository.ResultManageAuthorsView = value; }
        public IEnumerable<GenreView> ResultManageGenresView { get => storeRepository.ResultManageGenresView; set => storeRepository.ResultManageGenresView = value; }
        public IEnumerable<PublisherView> ResultManagePublishersView { get => storeRepository.ResultManagePublishersView; set => storeRepository.ResultManagePublishersView = value; }
        public IEnumerable<BookView> ResultManageBooksView { get => storeRepository.ResultManageBooksView; set => storeRepository.ResultManageBooksView = value; }
        public IEnumerable<BookInStoreView> ResultManageBooksInStoreView { get => storeRepository.ResultManageBooksInStoreView; set => storeRepository.ResultManageBooksInStoreView = value; }
        public IEnumerable<StockView> ResultManageStocksView { get => storeRepository.ResultManageStocksView; set => storeRepository.ResultManageStocksView = value; }
        public IEnumerable<BookSeriesView> ResultManageBookSeriesView { get => storeRepository.ResultManageBookSeriesView; set => storeRepository.ResultManageBookSeriesView = value; }
        public IDictionary<string, ICommand> Commands => commands;
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
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsManageAuthorsUsed)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsManageGenresUsed)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsManagePublishersUsed)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsManageBooksUsed)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsManageBooksInStoreUsed)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsManageStocksUsed)));
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsManageBookSeriesUsed)));

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
                await storeRepository.BuyBook(book as BookViewShow);
            }
        }
        private async Task ReserveBookInStore(object book)
        {
            if (book is not null)
            {
                newViewFactory.CreateReserveBookView(storeRepository.DbOptions, book as BookViewShow, storeRepository.CurrentUser);
            }
            await OnPropertyChanged(new PropertyChangedEventArgs(nameof(ResultReservedBooksView)));
        }
        private async Task ManagedAccountsAdmin()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            await storeRepository.ManageAccountsView();
        }
        private async Task ManagedAuthorsAdmin()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            await storeRepository.ManageAuthorsView();
        }
        private async Task ManagedGenresAdmin()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            await storeRepository.ManageGenresView();
        }
        private async Task ManagedPublishersAdmin()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            await storeRepository.ManagePublishersView();
        }
        private async Task ManagedBooksAdmin()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            await storeRepository.ManageBooksView();
        }
        private async Task ManagedBooksInStoreAdmin()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            await storeRepository.ManageBooksInStoreView();
        }
        private async Task ManagedStocksAdmin()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            await storeRepository.ManageStocksView();
        }
        private async Task ManagedBookSeriesAdmin()
        {
            IsPeriodBarUsed = Visibility.Collapsed;
            await storeRepository.ManageBookSeriesView();
        }
        private async Task CreateAccountInRepository()
        {
            if ((newViewFactory.CreateAccountView(storeRepository.DbOptions)).Value)
                await ManagedAccountsAdmin();
        }
        private async Task CreateAuthorInRepository()
        {
            if ((newViewFactory.CreateAuthorView(storeRepository.DbOptions)).Value)
                await ManagedAuthorsAdmin();
        }
        private async Task CreateGenreInRepository()
        {
            if ((newViewFactory.CreateGenreView(storeRepository.DbOptions)).Value)
                await ManagedGenresAdmin();
        }
        private async Task CreatePublisherInRepository()
        {
            if ((newViewFactory.CreatePublisherView(storeRepository.DbOptions)).Value)
                await ManagedPublishersAdmin();
        }
        private async Task CreateBookInRepository()
        {
            if ((newViewFactory.CreateBookView(storeRepository.DbOptions)).Value)
                await ManagedBooksAdmin();
        }
        private async Task CreateBookInStoreInRepository()
        {
            if ((newViewFactory.CreateBookInStoreView(storeRepository.DbOptions)).Value)
                await ManagedBooksInStoreAdmin();
        }
        private async Task CreateStockInRepository()
        {
            if ((newViewFactory.CreateStockView(storeRepository.DbOptions)).Value)
                await ManagedStocksAdmin();
        }
        private async Task CreateBookSeriesInRepository()
        {
            if ((newViewFactory.CreateBookSeriesView(storeRepository.DbOptions)).Value)
                await ManagedBookSeriesAdmin();
        }
        private async Task EditAccountInRepository(object account)
        {
            if ((newViewFactory.CreateAccountView(storeRepository.DbOptions, account as AccountView)).Value)
            await ManagedAccountsAdmin();
        }
        private async Task EditAuthorInRepository(object author)
        {
            if ((newViewFactory.CreateAuthorView(storeRepository.DbOptions, author as AuthorView)).Value)
                await ManagedAuthorsAdmin();
        }
        private async Task EditGenreInRepository(object genre)
        {
            if ((newViewFactory.CreateGenreView(storeRepository.DbOptions, genre as GenreView)).Value)
                await ManagedGenresAdmin();
        }
        private async Task EditPublisherInRepository(object publisher)
        {
            if ((newViewFactory.CreatePublisherView(storeRepository.DbOptions, publisher as PublisherView)).Value)
                await ManagedPublishersAdmin();
        }
        private async Task EditBookInRepository(object book)
        {
            if ((newViewFactory.CreateBookView(storeRepository.DbOptions, book as BookView)).Value)
                await ManagedBooksAdmin();
        }
        private async Task EditBookInStoreInRepository(object bookInStore)
        {
            if ((newViewFactory.CreateBookInStoreView(storeRepository.DbOptions, bookInStore as BookInStoreView)).Value)
                await ManagedBooksInStoreAdmin();
        }
        private async Task EditStockInRepository(object stock)
        {
            if ((newViewFactory.CreateStockView(storeRepository.DbOptions, stock as StockView)).Value)
                await ManagedStocksAdmin();
        }
        private async Task EditBookSeriesInRepository(object bookSeries)
        {
            if ((newViewFactory.CreateBookSeriesView(storeRepository.DbOptions, bookSeries as BookSeriesView)).Value)
                await ManagedBookSeriesAdmin();
        }
        private async Task DeleteAccountInRepository(object account)
        {
            if (account is not null)
            {
                await storeRepository.DeleteAccount(account as AccountView);
            }
        }
        private async Task DeleteAuthorInRepository(object author)
        {
            if (author is not null)
            {
                await storeRepository.DeleteAuthor(author as AuthorView);
            }
        }
        private async Task DeleteGenreInRepository(object genre)
        {
            if (genre is not null)
            {
                await storeRepository.DeleteGenre(genre as GenreView);
            }
        }
        private async Task DeletePublisherInRepository(object publisher)
        {
            if (publisher is not null)
            {
                await storeRepository.DeletePublisher(publisher as PublisherView);
            }
        }
        private async Task DeleteBookInRepository(object book)
        {
            if (book is not null)
            {
                await storeRepository.DeleteBook(book as BookView);
            }
        }
        private async Task DeleteBookInStoreInRepository(object bookInStore)
        {
            if (bookInStore is not null)
            {
                await storeRepository.DeleteBookInStore(bookInStore as BookInStoreView);
            }
        }
        private async Task DeleteStockInRepository(object stock)
        {
            if (stock is not null)
            {
                await storeRepository.DeleteStock(stock as StockView);
            }
        }
        private async Task DeleteBookSeriesInRepository(object bookSeries)
        {
            if (bookSeries is not null)
            {
                await storeRepository.DeleteBookSeries(bookSeries as BookSeriesView);
            }
        }
    }
}
