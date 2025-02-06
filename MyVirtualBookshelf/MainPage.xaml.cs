using MyVirtualBookshelf.Models;
#nullable disable

namespace MyVirtualBookshelf
{
    public partial class MainPage : ContentPage
    {
        private DatabaseHandler _db;

        public MainPage()
        { 
            InitializeComponent();
            _db = new DatabaseHandler();
            PopulateShelves();
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
            List<Shelf> shelves = _db.GetAllShelves();

            // Set the shelf count label
            int numShelves = shelves.Count;
            ShelfCounterLabel.Text = $"{numShelves} / 8"; // Update the counter text

            ShelvesListView.ItemsSource = shelves;
        }  
    }
}
