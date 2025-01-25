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
    class Shelf
    {
        /// <summary>
        /// Name of the shelf.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of all the books in the shelf.
        /// </summary>
        public List<Book> Books { get; set; }

        /// <summary>
        /// Initializes a new instance of the Shelf class with a specified name.
        /// </summary>
        /// <param name="name">The name of the shelf.</param>
        public Shelf(string name)
        {
            Name = name;
            Books = new List<Book>();
        }

        /// <summary>
        /// Adds a book to the shelf.
        /// </summary>
        /// <param name="bookToAdd">The book to add to the shelf.</param>
        public void AddBook(Book bookToAdd)
        {
            Books.Add(bookToAdd);
        }

        /// <summary>
        /// Removes a book from the shelf.
        /// </summary>
        /// <param name="bookToRemove">The book to remove from the shelf.</param>
        public void RemoveBook(Book bookToRemove)
        {
            Books.Remove(bookToRemove);
        }
    }
}
