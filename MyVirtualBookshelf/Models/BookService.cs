using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
#nullable disable

namespace MyVirtualBookshelf.Models
{
    public class BookService
    {
        private readonly BooksService _booksService;

        // Constructor to initialize Google Books API client
        public BookService()
        {
            // Initialize the BooksService with the retrieved API key
            _booksService = new BooksService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyAqbg1glYeFYGA4vkU6wMYWut21f3bFSFc",
                ApplicationName = "MyVirtualBookshelf"
            });
        }

        /// <summary>
        /// Returns 5 results in a list based off of the searchQuery parameter.
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public async Task<List<Book>> SearchGoogleBooksAsync(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return new List<Book>();  // Return empty list if query is empty
            }

            try
            {
                var request = _booksService.Volumes.List(searchQuery);
                request.MaxResults = 5;  // Limit the number of results to 5 (this can be adjusted)
                var response = await request.ExecuteAsync();

                List<Book> searchResults = new List<Book>();

                foreach (Volume item in response.Items)
                {
                    Book result = new Book
                    {
                        Title = item.VolumeInfo.Title,
                        Author = item.VolumeInfo.Authors?.FirstOrDefault(),
                        Genre = item.VolumeInfo.Categories?.FirstOrDefault(),
                        Isbn = GetIsbn13(item)
                    };

                    searchResults.Add(result);
                }

                return searchResults;  // Return the list of books
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching books");
                return new List<Book>();  // Return empty list if there's an error
            }
        }

        /// <summary>
        /// Returns a string form of the searched book's isbn number
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetIsbn13(Volume item)
        {
            foreach (var identifier in item.VolumeInfo.IndustryIdentifiers)
            {
                if (identifier.Type == "ISBN_13")
                {
                    return identifier.Identifier;
                }
            }
            return null;  // Return null if no ISBN_13 is found
        }

    }
}
