﻿using BookStore.Models.Db;
using BookStore.Models.Presenters;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Interfaces
{
    public interface INewViewFactory<TContext> where TContext : DbContext
    {
        void CreateReserveBookView(DbContextOptions<TContext> options, BookViewShow book, Account account);
        bool? CreateAccountView(DbContextOptions<TContext> options, AccountView account = null);
        bool? CreateStockView(DbContextOptions<TContext> options, StockView stock = null);
        bool? CreateBookView(DbContextOptions<TContext> options, BookView book = null);
        bool? CreateAuthorView(DbContextOptions<TContext> options, AuthorView author = null);
        bool? CreateGenreView(DbContextOptions<TContext> options, GenreView author = null);
    }
}
