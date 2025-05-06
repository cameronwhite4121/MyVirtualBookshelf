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
    /// Bookshelf stores references to shelves
    /// </summary>
    [Table("Bookshelves")]
    public class Bookshelf
    {
        /// <summary>
        /// Primary key of bookshelf.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Display for bookshelves
        /// </summary>
        [Column("BookshelfName")]
        public string BookshelfName { get; set; }

        /// <summary>
        /// Total amount of books in the shelf
        /// </summary>
        [Column("BookCount")]
        public int BookCount { get; set; }

        /// <summary>
        /// Total amount of books in the shelf
        /// </summary>
        [Column("BookCountString")]
        public string BookCountString { get; set; }

        /// <summary>
        /// Meant to display a string that hints at the contents of the shelf.
        /// </summary>
        [Column("ShelfContentsHint")]
        public string ShelfContentsHint { get; set; }

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Bookshelf() { }
    }
}
