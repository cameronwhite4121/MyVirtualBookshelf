using Microsoft.Maui.Controls;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVirtualBookshelf.Models
{
    internal class DatabaseHandler
    {
        private SQLiteConnection _db;

        public DatabaseHandler()
        {
            // Make folder path for database destination
            string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/mydatabase.db3";
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<Shelf>();
            _db.CreateTable<Book>();
            _db.CreateTable<ShelfContents>();
        }
        public void DeleteDb()
        {
            string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/mydatabase.db3";

            // Delete the existing database file if it exists
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
        }

        public void CreateShelf()
        {
            if (GetAllShelves().Count < 8) // Only allow 8 shelves at once
            {
                Shelf shelf = new Shelf();
                _db.Insert(shelf);
            }
        }

        public void DeleteShelf(int shelfId)
        {
            // Use query syntax to find the shelf to delete by its Id
            Shelf shelfToDelete = (from shelf in _db.Table<Shelf>()
                                   where shelf.Id == shelfId
                                   select shelf).FirstOrDefault();

            // If the shelf is found, delete it
            if (shelfToDelete != null)
            {
                _db.Delete(shelfToDelete);
            }
        }

        public List<ShelfContents> getShelfContents(int shelfId)
        {
            List<ShelfContents> shelfContents = (from sc in _db.Table<ShelfContents>()
                                           where sc.ShelfId == shelfId
                                           select sc).ToList();

            return shelfContents;
        }

        public List<Shelf> GetAllShelves()
        {
            return _db.Table<Shelf>().ToList();
        }

        public void AddBookToShelf(int shelfId, string bookTitle)
        {
            // Call GetBook
            Book bookToAdd = GetBook(bookTitle);

            // Add book to ShelfContents
            ShelfContents bookToAddToShelf = new ShelfContents(shelfId, bookTitle);
            _db.Insert(bookToAddToShelf);
        }

        public Book GetBook(string bookTitle)
        {
            // Search for bookName in books table
            Book bookToReturn = (from book in _db.Table<Book>()
                                   where book.Title == bookTitle
                                   select book).FirstOrDefault();

            // If found, don't add it to books table, return book
            if (bookToReturn != null)
            {
                return bookToReturn;
            }
            else // Else, add it to books table, return book
            {
                _db.Insert(bookToReturn);
                Book newBook = new(bookTitle);
                return newBook;
            }
            
        }
    }
}
