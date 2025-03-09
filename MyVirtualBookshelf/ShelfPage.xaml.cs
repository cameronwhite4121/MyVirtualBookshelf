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
    /// Used when EditBookBtn_Clicked is called
    /// </summary>
    public Book BookToEdit { get; set; } = new Book();

    /// <summary>
    /// Used when DeleteBookBtn_Clicked is called
    /// </summary>
    public Book BookToDelete { get; set; } = new Book();

    /// <summary>
    /// Needed for when adding and deleting books from this shelf.
    /// </summary>
    public int ShelfId { get; set; }

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
            try
            {
                // throw new Exception(); // For debugging
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
            catch (Exception ex)
            {   // Api call failed - Add SearchBar text as a book
                Book titleOnlyBook = new Book(ShelfId, BookSearchbar.Text);
                _db.AddBook(ShelfId, titleOnlyBook);
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
            BookToDelete.Id = book.Id;
            BookToDelete.Title = book.Title;

            ConfirmDeleteLabel.Text = $"Delete \"{BookToDelete.Title}\" ?";

            OpaqueBackground.IsVisible = true;
            OpaqueBackground.Opacity = 0.3;
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
        OpaqueBackground.IsVisible = false;

        _db.DeleteBook(ShelfId, BookToDelete.Id);
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
        OpaqueBackground.IsVisible = false;
    }

    public void EditBookBtn_Clicked(object sender, EventArgs e)
    {
        if (!ConfirmMenu.IsVisible || !EditMenu.IsVisible)
        {
            // Cast sender as a button to access BindingContext
            Button clickedButton = sender as Button;

            // Display BookTitle to the confirmation menu 
            Book book = clickedButton.BindingContext as Book;
            BookToEdit.Id = book.Id;
            BookToEdit.Title = book.Title;
            BookToEdit.Author = book.Author;
            BookToEdit.Genre = book.Genre;
            BookToEdit.Isbn = book.Isbn;
            BookToEdit.ShelfId = book.ShelfId;

            OpaqueBackground.IsVisible = true;
            OpaqueBackground.Opacity = 0.3;
            EditMenu.IsVisible = true;

            BookTitleEntry.Text = BookToEdit.Title;
            BookAuthorEntry.Text = BookToEdit.Author;
            BookGenreEntry.Text = BookToEdit.Genre;
            BookIsbnEntry.Text = BookToEdit.Isbn;
        }
    }

    public void SaveEditBtn_Clicked(object sender, EventArgs e)
    {
        Book updatedBook = new Book();
        updatedBook.Id = BookToEdit.Id;
        updatedBook.Title = BookTitleEntry.Text;
        updatedBook.Author = BookAuthorEntry.Text;
        updatedBook.Genre = BookGenreEntry.Text;
        updatedBook.Isbn = BookIsbnEntry.Text;
        updatedBook.ShelfId = BookToEdit.ShelfId;

        _db.UpdateBook(updatedBook);

        PopulateShelf();

        EditMenu.IsVisible = false;
        OpaqueBackground.IsVisible = false;
    }

    public void CancelEditBtn_Clicked(object sender, EventArgs e)
    {
        EditMenu.IsVisible = false;
        OpaqueBackground.IsVisible = false;
    }
}