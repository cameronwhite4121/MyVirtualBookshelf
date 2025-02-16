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

        public List<Book> GetShelfContents(int shelfId)
        {
            List<Book> shelfContents = (from b in _db.Table<Book>()
                                           where b.ShelfId == shelfId
                                           select b).ToList();

            return shelfContents;
        }

        public List<Shelf> GetAllShelves()
        {
            return _db.Table<Shelf>().ToList();
        }

        public void AddBookToShelf(int shelfId, string bookTitle)
        {
            Book bookToAdd = new Book(shelfId, bookTitle);
            _db.Insert(bookToAdd);
        }
    }
}
