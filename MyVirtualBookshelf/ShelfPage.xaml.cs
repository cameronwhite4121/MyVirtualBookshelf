using MyVirtualBookshelf.Models;
#nullable disable
namespace MyVirtualBookshelf;

public partial class ShelfPage : ContentPage
{
    public int ShelfId { get; set; }
    private DatabaseHandler _db;

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
        List<ShelfContents> allBooks = _db.getShelfContents(ShelfId);
        ShelfContentView.ItemsSource = allBooks;
    }
}