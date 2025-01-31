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
            string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/mydatabase.db3";
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<Shelf>();
            _db.CreateTable<Book>();
            _db.CreateTable<ShelfContents>();
        }

        public void CreateShelf()
        {
            Shelf shelf = new Shelf();
            _db.Insert(shelf);
        }

        public List<Shelf> GetAllShelves()
        {
            return _db.Table<Shelf>().ToList();
        }
    }
}
