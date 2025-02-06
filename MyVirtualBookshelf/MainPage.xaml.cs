using MyVirtualBookshelf.Models;
using System.Collections.ObjectModel;
#nullable disable

namespace MyVirtualBookshelf
{
    public partial class MainPage : ContentPage
    {
        private DatabaseHandler _db;
        public ObservableCollection<Shelf> Shelves { get; set; }

        public MainPage()
        { 
            InitializeComponent();
            _db = new DatabaseHandler();
            Shelves = new ObservableCollection<Shelf>();
            PopulateShelves();
            BindingContext = this;
        }

        public void CreateShelfBtn(object sender, EventArgs e)
        {
            _db.CreateShelf();
            PopulateShelves();
        }

        public void DeleteShelfBtn(object sender, EventArgs e)
        {
            // Cast sender as a button to access BindingContext
            Button clickedButton = sender as Button;

            // The shelf that the button was bound to. 
            Shelf shelfToDelete = clickedButton.BindingContext as Shelf;

            _db.DeleteShelf(shelfToDelete.Id);
            PopulateShelves();
        }

        public async void OpenShelfBtn(object sender, EventArgs e)
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

        public void PopulateShelves()
        {
            List<Shelf> updatedShelves = _db.GetAllShelves();

            Shelves.Clear();

            foreach (Shelf shelf in updatedShelves)
            {
                Shelves.Add(shelf);
            }

            // Update ShelfCounterLabel
            int numShelves = Shelves.Count;
            ShelfCounterLabel.Text = $"{numShelves} / 8";
        }
    }
}
