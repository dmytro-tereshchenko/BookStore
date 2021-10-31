using BookStore.Models;
using BookStore.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Interfaces
{
    public interface INewViewFactory<TContext> where TContext : DbContext
    {
        void CreateReserveBookView(DbContextOptions<TContext> options, BookViewShow book, Account account);
        bool? CreateAccountView(DbContextOptions<TContext> options, AccountView account = null);
        bool? CreateStockView(DbContextOptions<TContext> options, StockView stock = null);
        bool? CreateBookView(DbContextOptions<TContext> options, BookView book = null);
    }
}
