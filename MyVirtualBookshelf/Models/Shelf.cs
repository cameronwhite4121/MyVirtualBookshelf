using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace MyVirtualBookshelf.Models
{
    /// <summary>
    /// The shelf class is representative of a real life
    /// bookshelf in that it holds a collection of books.
    /// </summary>
    [Table("Shelves")]
    public class Shelf
    {
        /// <summary>
        /// Primary key of Shelves.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Used for shelf display
        /// </summary>
        [Column("ShelfName")]
        public int ShelfName { get; set; }

        /// <summary>
        /// Reference to the bookshelf that this shelf is contained in
        /// </summary>
        [Column("BookshelfId")]
        public int BookshelfId { get; set; }

        /// <summary>
        /// Total amount of books in the shelf
        /// </summary>
        [Column("BookCount")]
        public int BookCount { get; set; }

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Shelf() {}

        /// <summary>
        /// References a bookshelf to this shelf.
        /// </summary>
        public Shelf(int bookshelfId) 
        {
            BookshelfId = bookshelfId;
        }
    }
}
