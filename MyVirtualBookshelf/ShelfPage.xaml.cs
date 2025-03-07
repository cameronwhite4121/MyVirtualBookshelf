using MyVirtualBookshelf.Models;
using System.Collections.ObjectModel;
#nullable disable
namespace MyVirtualBookshelf;

public partial class ShelfPage : ContentPage
{
    
    private DatabaseHandler _db;

    /// <summary>
    /// Google books service
    /// </summary>
    private BookService _bookService;
    
    /// <summary>
    /// The books in this shelf.
    /// </summary>
    public ObservableCollection<Book> Books { get; set; }

    /// <summary>
    /// Storage for when needing to delete a book.
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Needed for when adding and deleting books from this shelf.
    /// </summary>
    public int ShelfId { get; set; }

    /// <summary>
    /// Required for the delete menu
    /// </summary>
    public string BookTitle { get; set; }

    /// <summary>
    /// Reference to the bookshelf that this shelf resides in.
    /// Required for updating BookCount property.
    /// </summary>
    public BookshelfPage BookshelfPageToUpdate { get; set; }

    public ShelfPage(int sid, BookshelfPage bookshelfPage)
	{
        InitializeComponent();
        this.ShelfId = sid;
        BookshelfPageToUpdate = bookshelfPage;

        // Google books service
        _bookService = new BookService();

        // Displays list of books on the page 
        _db = new DatabaseHandler();
        Books = new ObservableCollection<Book>();

        PopulateShelf();
        this.BindingContext = this;
    }

    /// <summary>
    /// Calls the database and retrieves every book for the shelf via
    /// the ShelfId property.
    /// </summary>
    public void PopulateShelf()
    {
        Books.Clear();

        List<Book> updatedShelf = _db.GetShelfContents(ShelfId);

        foreach (Book book in updatedShelf)
        {
            Books.Add(book);
        }
    }

    /// <summary>
    /// When the search bar button is clicked, it adds the value
    /// in the search bar to the current shelf.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public async void BookSearchBarButton_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(BookSearchbar.Text))
        {
            // Call the Google Books API service here
            string searchQuery = BookSearchbar.Text;
            List<Book> searchResults = await _bookService.SearchGoogleBooksAsync(searchQuery);

            // Add the first result from the Google Books query to the db and perform
            // necessary updates.
            if (searchResults.Count > 0)
            {
                Book bookToAdd = searchResults[0];
                _db.AddBook(ShelfId, bookToAdd);
                BookSearchbar.Text = null;
                PopulateShelf();
                BookshelfPageToUpdate.PopulateBookshelf();
            }
        }
    }

    /// <summary>
    /// Opens the delete menu and stores the specified book's title and id.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void DeleteBookBtn_Clicked(object sender, EventArgs e)
    {
        if (!ConfirmMenu.IsVisible)
        {
            // Cast sender as a button to access BindingContext
            Button clickedButton = sender as Button;

            // Display BookTitle to the confirmation menu 
            Book book = clickedButton.BindingContext as Book;
            BookId = book.Id;
            BookTitle = book.Title;

            ConfirmDeleteLabel.Text = $"Delete \"{BookTitle}\" ?";

            ConfirmMenuBackground.IsVisible = true;
            ConfirmMenuBackground.Opacity = 0.3;
            ConfirmMenu.IsVisible = true;
        }
    }

    /// <summary>
    /// Deletes the specified book and hides the delete menu.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ConfirmDeleteBtn_Clicked(object sender, EventArgs e)
    {
        ConfirmMenu.IsVisible = false;
        ConfirmMenuBackground.IsVisible = false;

        _db.DeleteBook(ShelfId, BookId);
        PopulateShelf();
        BookshelfPageToUpdate.PopulateBookshelf();
    }

    /// <summary>
    /// Hides the delete menu.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void DeclineDeleteBtn_Clicked(object sender, EventArgs e)
    {
        ConfirmMenu.IsVisible = false;
        ConfirmMenuBackground.IsVisible = false;
    }
}