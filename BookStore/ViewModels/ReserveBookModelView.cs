using BookStore.Infrastructure;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookStore.ViewModels
{
    internal class ReserveBookModelView : ViewModel
    {
        private ReserveBookModel model;
        private ICommand ok;
        private ICommand cancel;
        public ReserveBookModelView(ReserveBookModel model)
        {
            this.model = model;
            ok = new DialogCommand(ReserveBook);
            cancel = new DialogCommand(CloseWindow);
            model.MessageChanged += OnMessageChanged;
        }
        public ICommand Ok { get => ok; }
        public ICommand Cancel { get => cancel; }
        public string NameBook { get => model.Book.Name; }
        public string Authors { get => model.Book.Authors; }
        public int PublicationYear { get => model.Book.YearOfPublished; }
        public string Publisher { get => model.Book.Publisher; }
        public string Genre { get => model.Book.Genre; }
        public string Series { get => model.Book.Series; }
        public string Price { get => model.Book.Price; }
        public string Description { get => model.Description; set => model.Description = value; }
        private void ReserveBook(object window)
        {
            model.AddReservedBook();
            CloseWindow(window);
        }
        private void CloseWindow(object window)
        {
            if (window is Window)
            {
                (window as Window).Close();
            }
        }
        private void OnMessageChanged(object sender, EventArgs e)
        {
            MessageBox.Show(model.Message);
        }
    }
}
