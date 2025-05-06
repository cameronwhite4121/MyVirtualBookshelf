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

        /// <summary>
        /// Creates a bookshelf and 8 shelves that are associated with that bookshelf.
        /// </summary>
        public void CreateBookshelf()
        {
            if (GetAllBookshelves().Count < 8) // Only allow 8 shelves at once
            {
                Bookshelf bookshelf = new();
                _db.Insert(bookshelf);
            }
        }

        /// <summary>
        /// Deletes the bookshelf, starting with its contents to remove any
        /// associations.
        /// </summary>
        /// <param name="bookshelfId"></param>
        public void DeleteBookshelf(int bookshelfId)
        {        
            // Use query syntax to find the shelf to delete by its Id
            Bookshelf bookshelfToDelete = (from bks in _db.Table<Bookshelf>()
                                           where bks.Id == bookshelfId
                                           select bks).FirstOrDefault();

            // If the bookshelf is found, delete it and its contents from top to bottom
            if (bookshelfToDelete != null)
            { 
                // Delete every book in the shelf
                List<Book> booksToDelete = (from b in _db.Table<Book>()
                                            where b.BookshelfId == bookshelfToDelete.Id
                                            select b).ToList();

                foreach (Book book in booksToDelete)
                {
                    _db.Delete(book);
                }             

                // Delete the bookshelf when its contents are deleted
                _db.Delete(bookshelfToDelete);
            }
        }

        /// <summary>
        /// Grabs every book in a specific shelf by the bookshelfid
        /// </summary>
        /// <param name="bookshelfId"></param>
        /// <returns></returns>
        public List<Book> GetBookshelfContents(int bookshelfId)
        {
            List<Book> bookshelfContents = (from b in _db.Table<Book>()
                                            where b.BookshelfId == bookshelfId
                                            select b).ToList();

            return bookshelfContents;
        }

        /// <summary>
        /// Returns every bookshelf in the db
        /// </summary>
        /// <returns></returns>
        public List<Bookshelf> GetAllBookshelves()
        {
            return _db.Table<Bookshelf>().ToList();
        }

        /// <summary>
        /// Adds a book to a shelf with the bookshelfId and a book object
        /// </summary>
        /// <param name="bookshelfId"></param>
        /// <param name="bookToAdd"></param>
        public void AddBook(int bookshelfId, Book bookToAdd)
        {
            bookToAdd.BookshelfId = bookshelfId;
            _db.Insert(bookToAdd);
            IncrementBookCount(bookshelfId);
        }

        /// <summary>
        /// Deletes a book from a shelf with the bookshelfId and bookId
        /// </summary>
        /// <param name="bookshelfId"></param>
        /// <param name="bookId"></param>
        public void DeleteBook(int bookshelfId, int bookId)
        {
            // Use query syntax to find the book to delete by its Id and the Shelf Id
            Book bookToDelete = (from b in _db.Table<Book>()
                                 where b.Id == bookId && b.BookshelfId == bookshelfId                                      
                                 select b).FirstOrDefault();

            // If the book is found, delete it from the shelf
            if (bookToDelete != null)
            {
                _db.Delete(bookToDelete);
                DecrementBookCount(bookshelfId);
            }
        }

        /// <summary>
        /// Updates a book in a shelf using a book object.
        /// </summary>
        /// <param name="bookToUpdate"></param>
        public void UpdateBook(Book bookToUpdate)
        {
            _db.Update(bookToUpdate);
        }

        /// <summary>
        /// Increments a shelf's BookCount property. Used to keep track 
        /// of the number of books in a shelf.
        /// </summary>
        /// <param name="bookshelfId"></param>
        public void IncrementBookCount(int bookshelfId)
        {
            Bookshelf bookshelfToUpdate = (from s in _db.Table<Bookshelf>()
                                   where s.Id == bookshelfId
                                   select s).FirstOrDefault();

            bookshelfToUpdate.BookCount++;
            _db.Update(bookshelfToUpdate);
        }

        /// <summary>
        /// Decrements a shelf's BookCount property. Used to keep track 
        /// of the number of books in a shelf.
        /// </summary>
        /// <param name="bookshelfId"></param>
        public void DecrementBookCount(int bookshelfId)
        {
            Bookshelf bookshelfToUpdate = (from s in _db.Table<Bookshelf>()
                                   where s.Id == bookshelfId
                                   select s).FirstOrDefault();

            bookshelfToUpdate.BookCount--;
            _db.Update(bookshelfToUpdate);
        }
    }
}
