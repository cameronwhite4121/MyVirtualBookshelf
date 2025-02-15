using MyVirtualBookshelf.Models;
#nullable disable
namespace MyVirtualBookshelf;

public partial class ShelfPage : ContentPage
{
    public int ShelfId { get; set; }
    private DatabaseHandler _db;
    private string SearchedBook {  get; set; }

    public ShelfPage(int sid)
	{
		this.ShelfId = sid;
        InitializeComponent();
        
        // BindingContext is set for data binding
        this.BindingContext = this;

        // Displays list of books on the page 
        _db = new DatabaseHandler();
        populateShelf();
	}

    /// <summary>
    /// Calls the database and retrieves every book for the shelf via
    /// the ShelfId property.
    /// </summary>
    private void populateShelf()
    {
        List<Book> allBooks = _db.GetShelfContents(ShelfId);
        ShelfContentView.ItemsSource = allBooks;
    }

    /// <summary>
    /// When the searchbar button is clicked, it adds the value
    /// in the searchbar to the current shelf.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void BookSearchbarButton_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(BookSearchbar.Text)) 
        { 
            _db.AddBookToShelf(ShelfId, BookSearchbar.Text);
            BookSearchbar.Text = null;
            populateShelf();
        }
    }
}