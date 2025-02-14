using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace MyVirtualBookshelf.Models
{
    [Table("ShelfContents")]
    public class ShelfContents
    {
        /// <summary>
        /// Foreign key to Shelf class
        /// </summary>
        [Column("ShelfId")]
        public int ShelfId { get; set; }

        /// <summary>
        /// Foreign key to Book class
        /// </summary>
        [Column("BookId")]
        public int BookId { get; set; }

        /// <summary>
        /// Tracks how many of a specific book are in a specific shelf
        /// </summary>
        [Column("Quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Constructor for adding books to the shelf.
        /// </summary>
        /// <param name="shelfId">Id of the current shelf</param>
        /// <param name="bookId">Id of the book being added</param>
        public ShelfContents(int shelfId, int bookId)
        {
            ShelfId = shelfId;
            BookId = bookId;
        }

        /// <summary>
        /// Parameterless constructor for SQLite.
        /// </summary>
        public ShelfContents() { }

    }
}
