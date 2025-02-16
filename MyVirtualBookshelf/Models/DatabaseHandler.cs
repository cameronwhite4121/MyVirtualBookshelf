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
            _db.CreateTable<Bookshelf>();
            _db.CreateTable<Shelf>();
            _db.CreateTable<Book>();
        }
        /// <summary>
        /// For debugging purposes. If this method is ever called, make sure to
        /// comment it out or delete it after its done being used.
        /// </summary>
        public static void DeleteDb()
        {
            string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/mydatabase.db3";

            // Delete the existing database file if it exists
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
        }

        public void CreateBookshelf()
        {
            if (GetAllBookshelves().Count < 8) // Only allow 8 shelves at once
            {
                Bookshelf bookshelf = new();
                _db.Insert(bookshelf);

                List<Shelf> shelves = new List<Shelf>
                {
                     new(bookshelf.Id),
                     new(bookshelf.Id),
                     new(bookshelf.Id),
                     new(bookshelf.Id),
                     new(bookshelf.Id),
                     new(bookshelf.Id),
                     new(bookshelf.Id),
                     new(bookshelf.Id)
                };            
                _db.InsertAll(shelves);
            }
        }

        public void DeleteBookshelf(int bookshelfId)
        {        
            // Use query syntax to find the shelf to delete by its Id
            Bookshelf bookshelfToDelete = (from bks in _db.Table<Bookshelf>()
                                           where bks.Id == bookshelfId
                                           select bks).FirstOrDefault();

            // Delete every shelf in the bookshelf
            List<Shelf> shelvesToDelete = (from s in _db.Table<Shelf>()
                                           where s.BookshelfId == bookshelfId
                                           select s).ToList();

            


            // If the bookshelf is found, delete it and its contents from top to bottom
            if (bookshelfToDelete != null)
            {
                foreach (Shelf shelf in shelvesToDelete)
                {
                    // Delete every book in the shelf
                    List<Book> booksToDelete = (from b in _db.Table<Book>()
                                                where b.ShelfId == shelf.Id
                                                select b).ToList();

                    foreach (Book book in booksToDelete)
                    {
                        _db.Delete(book);
                    }

                    // Delete the shelf when its contents are deleted
                    _db.Delete(shelf);
                }

                // Delete the bookshelf when its contents are deleted
                _db.Delete(bookshelfToDelete);
            }
        }

        public List<Shelf> GetBookshelfContents(int bookshelfId)
        {
            List<Shelf> shelves = (from s in _db.Table<Shelf>()
                                   where s.BookshelfId == bookshelfId
                                   select s).ToList();

            return shelves;
        }

        public List<Book> GetShelfContents(int shelfId)
        {
            List<Book> shelfContents = (from b in _db.Table<Book>()
                                           where b.ShelfId == shelfId
                                           select b).ToList();

            return shelfContents;
        }

        public List<Bookshelf> GetAllBookshelves()
        {
            return _db.Table<Bookshelf>().ToList();
        }

        public void AddBookToShelf(int shelfId, string bookTitle)
        {
            Book bookToAdd = new Book(shelfId, bookTitle);
            _db.Insert(bookToAdd);
        }
    }
}
