# BookStore
Desktop application for keeping track of the current assortment of books in the store.

Technologies: C#, .NET 5, WPF, MVVM, EF Core (MS SQL Server), Migrations, Asynchronous programming.

---

This application allows:
- show data from db
    - show all books
    - show new books
    - show best selling books
    - show most popular authors, genres
- sort by different periods
    - day
    - week
    - month
    - year
- search by different fields
    - name of book
    - author
    - genre
- work with accounts
    - admin
    - user
    - guest
- work on admin panel
    - create entities in db
    - read entities from db
    - edit entities in db
    - delete entities from db

## Beginning of work
1. Change connection string to db in [appsettings.json](/BookStore/appsettings.json)
2. Change administrator user details in method `OnModelCreating` in [StoreContext.cs](/BookStore/Models/Db/StoreContext.cs):
3. Run application

---

## Application example

![Watch the video](https://github.com/dmytro-tereshchenko/BookStore/blob/master/doc/exampleApp.gif)
