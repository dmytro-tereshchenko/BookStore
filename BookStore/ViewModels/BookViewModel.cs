using BookStore.Infrastructure;
using BookStore.Models;
using BookStore.Models.Presenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookStore.ViewModels
{
    internal class BookViewModel:ViewModel
    {
        private BookModel model;
        private IDictionary<string, ICommand> commands;
        public BookViewModel(BookModel model, bool isEdit = true)
        {
            this.model = model;
            commands = new Dictionary<string, ICommand>();
            commands.Add("Ok", isEdit ? new DialogCommand(EditBook) : new DialogCommand(CreateBook));
            commands.Add("Cancel", new DialogCommand(CloseWindow));
            commands.Add("SearchAuthorsInStore", new DelegateCommand(SearchAuthorsInStore));
            commands.Add("SearchSeriesInStore", new DelegateCommand(SearchSeriesInStore));
            commands.Add("SearchGenreInStore", new DelegateCommand(SearchGenreInStore));
            commands.Add("SearchPublisherInStore", new DelegateCommand(SearchPublisherInStore));
            commands.Add("AddAuthor", new DialogCommand(AddAuthor));
            commands.Add("DeleteAuthor", new DialogCommand(DeleteAuthor));
            commands.Add("SetSeries", new DialogCommand(SetSeries));
            commands.Add("SetGenre", new DialogCommand(SetGenre));
            commands.Add("SetPublisher", new DialogCommand(SetPublisher));
            commands.Add("CreateBook", new DialogCommand(CreateBook));
            commands.Add("EditBook", new DialogCommand(EditBook));

            model.MessageChanged += OnMessageChanged;
            model.PropertyChanged += OnPropertyModelChanged;
        }
        public IDictionary<string, ICommand> Commands { get => commands; }
        public BookView Book { get => model.Book; set => model.Book = value; }
        public string SearchAuthorName { get => model.SearchAuthorName; set => model.SearchAuthorName = value; }
        public string SearchSeriesName { get => model.SearchSeriesName; set => model.SearchSeriesName = value; }
        public string SearchGenreName { get => model.SearchGenreName; set => model.SearchGenreName = value; }
        public string SearchPublisherName { get => model.SearchPublisherName; set => model.SearchPublisherName = value; }
        public IEnumerable<AuthorView> SearchAuthors { get => model.SearchAuthors; set => model.SearchAuthors = value; }
        public IEnumerable<AuthorView> ResultAuthors { get => model.ResultAuthors; set => model.ResultAuthors = value; }
        public IEnumerable<BookSeriesView> SearchSeries { get => model.SearchSeries; set => model.SearchSeries = value; }
        public IEnumerable<GenreView> SearchGenre { get => model.SearchGenre; set => model.SearchGenre = value; }
        public IEnumerable<PublisherView> SearchPublisher { get => model.SearchPublisher; set => model.SearchPublisher = value; }
        private async Task SearchAuthorsInStore()
        {
            await model.SearchAuthorsInStore();
        }
        private async Task SearchSeriesInStore()
        {
            await model.SearchSeriesInStore();
        }
        private async Task SearchGenreInStore()
        {
            await model.SearchGenreInStore();
        }
        private async Task SearchPublisherInStore()
        {
            await model.SearchPublisherInStore();
        }
        private async Task AddAuthor(object author)
        {
            if (author is not null)
            {
                await model.AddAuthor(author as AuthorView);
            }
        }
        private async Task DeleteAuthor(object author)
        {
            if (author is not null)
            {
                await model.DeleteAuthor(author as AuthorView);
            }
        }
        private async Task SetSeries(object series)
        {
            if (series is not null)
            {
                await model.SetSeries(series as BookSeriesView);
            }
        }
        private async Task SetGenre(object genre)
        {
            if (genre is not null)
            {
                await model.SetGenre(genre as GenreView);
            }
        }
        private async Task SetPublisher(object publisher)
        {
            if (publisher is not null)
            {
                await model.SetPublisher(publisher as PublisherView);
            }
        }
        private async Task CreateBook(object window)
        {
            await model.AddBook();
            if (window is Window)
            {
                (window as Window).DialogResult = true;
            }
        }
        private async Task EditBook(object window)
        {
            await model.EditBook();
            if (window is Window)
            {
                (window as Window).DialogResult = true;
            }
        }
        private async Task CloseWindow(object window)
        {
            if (window is Window)
            {
                (window as Window).DialogResult = false;
            }
            await Task.CompletedTask;
        }
        private void OnMessageChanged(object sender, EventArgs e)
        {
            MessageBox.Show(model.Message);
        }
        private async void OnPropertyModelChanged(object sender, PropertyChangedEventArgs e)
        {
            await OnPropertyChanged(e);
        }
    }
}
