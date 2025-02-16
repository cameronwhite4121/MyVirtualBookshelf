using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Parameterless constructor
        /// </summary>
        public Bookshelf() { }
    }
}
