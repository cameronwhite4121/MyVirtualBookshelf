﻿using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace MyVirtualBookshelf.Models
{
    /// <summary>
    /// Book class represents a book that is being
    /// tracked by the user and holds various properties
    /// like title, author, genre and isbn.
    /// </summary>
    [Table("Books")]
    class Book
    {
        /// <summary>
        /// Unique identifier of the book.
        /// </summary>
        [PrimaryKey]
        [Column("Isbn")]
        public string Isbn { get; set; }

        /// <summary>
        /// Title of the book.
        /// </summary>
        [Column("Title")]
        public string Title { get; set; }

        /// <summary>
        /// Author of the book.
        /// </summary>
        [Column("Author")]
        public string Author { get; set; }

        /// <summary>
        /// Genre of the book.
        /// </summary>
        [Column("Genre")]
        public string Genre { get; set; }

        /// <summary>
        /// Fully-typed constructor.
        /// </summary>
        /// <param name="title">Title of the book</param>
        /// <param name="author">Author of the book</param>
        /// <param name="genre">Book's genre</param>
        /// <param name="isbn">Book's unique isbn</param>
        public Book(string title, string author, string genre, string isbn)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Isbn = isbn;
        }
    }
}
