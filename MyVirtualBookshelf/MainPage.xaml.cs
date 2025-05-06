using MyVirtualBookshelf.Models;
using System.Collections.ObjectModel;
#nullable disable

namespace MyVirtualBookshelf
{
    public partial class MainPage : ContentPage
    {
        private DatabaseHandler _db;

        /// <summary>
        /// The bookshelves in the db
        /// </summary>
        public ObservableCollection<Bookshelf> Bookshelves { get; set; }

        /// <summary>
        /// Used to store a bookshelf's id when deleting bookshelves.
        /// </summary>
        int BookshelfId {  get; set; }

        public MainPage()
        { 
            InitializeComponent();

            _db = new DatabaseHandler();
            Bookshelves = new ObservableCollection<Bookshelf>();

            PopulateBookshelves();
            BindingContext = this;
        }

        /// <summary>
        /// Add a bookshelf to the db and resets the bookshelf display.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CreateBookshelfBtn_Clicked(object sender, EventArgs e)
        {
            if(!ConfirmMenu.IsVisible)
            {
                _db.CreateBookshelf();
                PopulateBookshelves();
            } 
        }

        /// <summary>
        /// Opens the delete menu and stores the selected bookshelf's id.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeleteBookshelfBtn_Clicked(object sender, EventArgs e)
        {
            if (!ConfirmMenu.IsVisible)
            {
                // Cast sender as a button to access BindingContext
                Button clickedButton = sender as Button;

                // The shelf that the button was bound to. 
                Bookshelf bookshelf = clickedButton.BindingContext as Bookshelf;
                BookshelfId = bookshelf.Id;

                ConfirmMenu.IsVisible = true;
                ConfirmMenuBackground.IsVisible = true;
                ConfirmMenuBackground.Opacity = 0.5;
            }
        }

        /// <summary>
        /// Hides the delete menu, then deletes the bookshelf from the db and
        /// updates the shelf.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ConfirmDeleteBtn_Clicked(object sender, EventArgs e) 
        {
            ConfirmMenu.IsVisible = false;
            ConfirmMenuBackground.IsVisible = false;

            _db.DeleteBookshelf(BookshelfId);
            PopulateBookshelves();
        }

        /// <summary>
        /// Hides the delete menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DontDeleteBtn_Clicked(object sender, EventArgs e)
        {
            ConfirmMenu.IsVisible = false;
            ConfirmMenuBackground.IsVisible = false;
        }

        /// <summary>
        /// When a bookshelf is clicked, opens that specific bookshelf.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void OpenBookshelfBtn_Clicked(object sender, EventArgs e)
        {
            // Ensure the sender is a Button
            Button clickedButton = sender as Button;

            // Check if the clickedButton is not null and safely access its BindingContext
            if (clickedButton != null)
            {
                Bookshelf selectedBookshelf = clickedButton.BindingContext as Bookshelf;
                await Navigation.PushAsync(new BookshelfPage(selectedBookshelf.Id, this));
            }
        }

        /// <summary>
        /// Updates the bookshelf display, useful when creating
        /// and deleting bookshelves.
        /// </summary>
        public void PopulateBookshelves()
        {
            Bookshelves.Clear();

            List<Bookshelf> updatedBookshelves = _db.GetAllBookshelves();

            int i = 1;
            foreach (Bookshelf bookshelf in updatedBookshelves)
            {
                bookshelf.BookshelfName = "Bookshelf " + i.ToString();

                if (bookshelf.BookCount == 1)
                {
                    bookshelf.BookCountString = "|  1 Book  |   ";
                }
                else
                {
                    bookshelf.BookCountString = "|  " + bookshelf.BookCount + " Books  |   ";
                }

                Bookshelves.Add(bookshelf);
                DisplayShelfHint(bookshelf);
                i++;
            }

            // Update BookshelfCount.Text
            int numBookshelves = Bookshelves.Count;
            BookshelfCountLabel.Text = $"{numBookshelves} / 8";           
        }

        /// <summary>
        /// This method grabs all the books in a shelf, if any, and
        /// creates a string that is used to display a hint at what
        /// is in the shelf.
        /// </summary>
        /// <param name="bookshelf"></param>
        public void DisplayShelfHint(Bookshelf bookshelf)
        {
            List<Book> allBooks = _db.GetBookshelfContents(bookshelf.Id);

            // Avoid null-reference exception by checking ahead
            if (allBooks.Count == 0)
            {
               bookshelf.ShelfContentsHint = "This bookshelf is empty";
            }
            else
            {
                // Create a string using fencepost method           
                for (int i = 0; i < allBooks.Count && i < 2; i++)
                {
                    // Add the title to ShelfContentsHint
                    if (i == 0)
                    {
                        bookshelf.ShelfContentsHint += allBooks[i].Title;
                    }
                    else
                    {
                        bookshelf.ShelfContentsHint += ", " + allBooks[i].Title;
                    }
                }
                bookshelf.ShelfContentsHint += " ...";
            }
        }
    }
}
