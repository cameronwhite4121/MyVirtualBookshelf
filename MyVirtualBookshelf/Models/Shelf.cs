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
        /// Name of the shelf.
        /// </summary>
        [Column("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Shelf() {}
    }
}
