using MyVirtualBookshelf.Models;
using System.Collections.ObjectModel;
namespace MyVirtualBookshelf;
#nullable disable

public partial class BookshelfPage : ContentPage
{
    public int BookshelfId { get; set; }
    public ObservableCollection<Shelf> Shelves { get; set; }
    private DatabaseHandler _db;

    public BookshelfPage(int bookshelfId)
    {
        this.BookshelfId = bookshelfId;
        InitializeComponent();

        // BindingContext is set for data binding
        this.BindingContext = this;

        // Display the shelves in the bokshelf 
        _db = new DatabaseHandler();
        Shelves = new ObservableCollection<Shelf>();
        PopulateBookshelf();
    }

    public void PopulateBookshelf()
    {
        Shelves.Clear();

        List<Shelf> updatedShelves = _db.GetBookshelfContents(BookshelfId);

        foreach (Shelf shelf in updatedShelves)
        {
            Shelves.Add(shelf);
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
            await Navigation.PushAsync(new ShelfPage(selectedShelf.Id));
        }
    }
}