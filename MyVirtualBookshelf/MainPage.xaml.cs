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

        public void PopulateShelves()
        {
            List<Shelf> shelves = _db.GetAllShelves();
            ShelvesListView.ItemsSource = shelves;
        }  
    }
}
