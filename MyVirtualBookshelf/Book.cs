using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace MyVirtualBookshelf
{
    /// <summary>
    /// Book class represents a book that is being
    /// tracked by the user and holds various properties
    /// like title, author, genre and isbn.
    /// </summary>
    class Book
    {
        /// <summary>
        /// Title of the book.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Author of the book.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Genre of the book.
        /// </summary>
        public string Genre { get; set; }
        
        /// <summary>
        /// Unique identifier of the book.
        /// </summary>
        public string Isbn { get; set; }

    }
}
