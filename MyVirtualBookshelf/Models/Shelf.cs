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
    class Shelf
    {
        /// <summary>
        /// Primary key of Shelves.
        /// </summary>
        [PrimaryKey]
        [Column("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the shelf.
        /// </summary>
        [Column("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Shelf() {}

        /// <summary>
        /// Adds a book to the shelf.
        /// </summary>
        /// <param name="bookToAdd">The book to add to the shelf.</param>
        public void AddBook(Book bookToAdd)
        {
            ShelfContents shelfContents = new ShelfContents();
            shelfContents.ShelfId = Id;
        }

        /// <summary>
        /// Removes a book from the shelf.
        /// </summary>
        /// <param name="bookToRemove">The book to remove from the shelf.</param>
        public void RemoveBook(Book bookToRemove)
        {
            ShelfContents shelfContents = new ShelfContents();
            shelfContents.ShelfId = Id;
        }
    }
}
