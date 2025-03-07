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

        /// <summary>
        /// Creates a bookshelf and 8 shelves that are associated with that bookshelf.
        /// </summary>
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

        /// <summary>
        /// Grabs a bookshelves shelves by the bookshelfid
        /// </summary>
        /// <param name="bookshelfId"></param>
        /// <returns></returns>
        public List<Shelf> GetBookshelfContents(int bookshelfId)
        {
            List<Shelf> shelves = (from s in _db.Table<Shelf>()
                                   where s.BookshelfId == bookshelfId
                                   select s).ToList();

            return shelves;
        }

        /// <summary>
        /// Grabs every book in a specific shelf by the shelfid
        /// </summary>
        /// <param name="shelfId"></param>
        /// <returns></returns>
        public List<Book> GetShelfContents(int shelfId)
        {
            List<Book> shelfContents = (from b in _db.Table<Book>()
                                        where b.ShelfId == shelfId
                                        select b).ToList();

            return shelfContents;
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
        /// returns a specific shelf by its shelfid
        /// </summary>
        /// <param name="shelfId"></param>
        /// <returns></returns>
        public Shelf GetShelf(int shelfId)
        {
            Shelf shelf = (from s in _db.Table<Shelf>()
                                   where s.Id == shelfId
                                   select s).FirstOrDefault();

            return shelf;
        }

        /// <summary>
        /// Adds a book to a shelf with the shelfid and a book object
        /// </summary>
        /// <param name="shelfId"></param>
        /// <param name="bookToAdd"></param>
        public void AddBook(int shelfId, Book bookToAdd)
        {
            bookToAdd.ShelfId = shelfId;
            _db.Insert(bookToAdd);
            IncrementBookCount(shelfId);
        }

        /// <summary>
        /// Deletes a book from a shelf with the shelfid and bookid
        /// </summary>
        /// <param name="shelfId"></param>
        /// <param name="bookId"></param>
        public void DeleteBook(int shelfId, int bookId)
        {
            // Use query syntax to find the book to delete by its Id and the Shelf Id
            Book bookToDelete = (from b in _db.Table<Book>()
                                 where b.Id == bookId && b.ShelfId == shelfId                                      
                                 select b).FirstOrDefault();

            // If the book is found, delete it from the shelf
            if (bookToDelete != null)
            {
                _db.Delete(bookToDelete);
                DecrementBookCount(shelfId);
            }
        }

        /// <summary>
        /// Increments a shelf's BookCount property. Used to keep track 
        /// of the number of books in a shelf.
        /// </summary>
        /// <param name="shelfId"></param>
        public void IncrementBookCount(int shelfId)
        {
            Shelf shelfToUpdate = (from s in _db.Table<Shelf>()
                                   where s.Id == shelfId
                                   select s).FirstOrDefault();

            shelfToUpdate.BookCount++;
            _db.Update(shelfToUpdate);
        }

        /// <summary>
        /// Decrements a shelf's BookCount property. Used to keep track 
        /// of the number of books in a shelf.
        /// </summary>
        /// <param name="shelfId"></param>
        public void DecrementBookCount(int shelfId)
        {
            Shelf shelfToUpdate = (from s in _db.Table<Shelf>()
                                   where s.Id == shelfId
                                   select s).FirstOrDefault();

            shelfToUpdate.BookCount--;
            _db.Update(shelfToUpdate);
        }
    }
}
