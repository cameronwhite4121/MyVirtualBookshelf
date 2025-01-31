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

            _db = new SQLiteConnection("path/to/your/database");
            _db.CreateTable<Shelf>();
            _db.CreateTable<Book>();
            _db.CreateTable<ShelfContents>();
        }

    }
}
