using MyVirtualBookshelf.Models;
using System.Collections.ObjectModel;
using System.Globalization;
namespace MyVirtualBookshelf;
#nullable disable

public partial class BookshelfPage : ContentPage
{
    private DatabaseHandler _db;
    public ObservableCollection<Shelf> Shelves { get; set; }
    int BookshelfId { get; set; }
    
    public BookshelfPage(int bookshelfId)
    {
        this.BookshelfId = bookshelfId;
        InitializeComponent();

        // Display the shelves in the bookshelf 
        _db = new DatabaseHandler();
        Shelves = new ObservableCollection<Shelf>();

        PopulateBookshelf();
        this.BindingContext = this;
    }

    public void PopulateBookshelf()
    {
        Shelves.Clear();

        List<Shelf> updatedShelves = _db.GetBookshelfContents(BookshelfId);
        
        int i = 1;
        foreach (Shelf shelf in updatedShelves)
        {
            // Create shelf name
            shelf.ShelfName = "Shelf " + i.ToString();

            DisplayShelfHint(shelf);

            Shelves.Add(shelf);
            i++;
        }
    }

    /// <summary>
    /// This method grabs all the books in a shelf, if any, and
    /// creates a string that is used to display a hint at what
    /// is in the shelf.
    /// </summary>
    /// <param name="shelf"></param>
    public void DisplayShelfHint(Shelf shelf)
    {       
        List<Book> allBooks = _db.GetShelfContents(shelf.Id);

        // Avoid null-reference exception by checking ahead
        if (allBooks.Count == 0)
        {
            shelf.ShelfContentsHint = "This shelf is empty";
        }
        else
        {
            // Create a string using fencepost method           
            for (int i = 0; i < allBooks.Count && i < 3; i++)
            {
                // Add the title to ShelfContentsHint
                if (i == 0)
                {
                    shelf.ShelfContentsHint += allBooks[i].Title;
                }
                else
                {
                    shelf.ShelfContentsHint += ", " + allBooks[i].Title;
                }
            }
        }       
    }

    public async void OpenShelfBtn_Clicked(object sender, EventArgs e)
    {
        // Ensure the sender is a Button
        Button clickedButton = sender as Button;

        // Check if the clickedButton is not null and safely access its BindingContext
        if (clickedButton != null)
        {
            Shelf selectedShelf = clickedButton.BindingContext as Shelf;
            ShelfPage shelfPage = new ShelfPage(selectedShelf.Id, this);
            await Navigation.PushAsync(shelfPage);
        }
    }
}