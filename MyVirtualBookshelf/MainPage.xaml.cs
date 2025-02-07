using MyVirtualBookshelf.Models;
using System.Collections.ObjectModel;
#nullable disable

namespace MyVirtualBookshelf
{
    public partial class MainPage : ContentPage
    {
        private DatabaseHandler _db;
        public ObservableCollection<Shelf> Shelves { get; set; }

        int ShelfToDelete {  get; set; }

        public MainPage()
        { 
            InitializeComponent();
            _db = new DatabaseHandler();
            Shelves = new ObservableCollection<Shelf>();
            PopulateShelves();
            BindingContext = this;
        }

        public void CreateShelfBtn_Clicked(object sender, EventArgs e)
        {
            if(ConfirmMenu.IsVisible != true)
            {
                _db.CreateShelf();
                PopulateShelves();
            } 
        }

        public void DeleteShelfBtn_Clicked(object sender, EventArgs e)
        {
            if (ConfirmMenu.IsVisible != true)
            {
                // Cast sender as a button to access BindingContext
                Button clickedButton = sender as Button;

                // The shelf that the button was bound to. 
                Shelf shelf = clickedButton.BindingContext as Shelf;

                ShelfToDelete = shelf.Id;

                ConfirmMenu.IsVisible = true;
                ConfirmMenuBackground.IsVisible = true;
                ConfirmMenuBackground.Opacity = 0.6;
            }
        }

        public void ConfirmDeleteBtn_Clicked(object sender, EventArgs e) 
        {
            ConfirmMenu.IsVisible = false;
            ConfirmMenuBackground.IsVisible = false;
            _db.DeleteShelf(ShelfToDelete);
            PopulateShelves();
        }

        public void DontDeleteBtn_Clicked(object sender, EventArgs e)
        {
            ConfirmMenu.IsVisible = false;
            ConfirmMenuBackground.IsVisible = false;
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
