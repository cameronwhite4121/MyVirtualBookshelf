﻿using MyVirtualBookshelf.Models;
using System.Collections.ObjectModel;
#nullable disable

namespace MyVirtualBookshelf
{
    public partial class MainPage : ContentPage
    {
        private DatabaseHandler _db;

        public ObservableCollection<Bookshelf> Bookshelves { get; set; }

        int BookshelfId {  get; set; }

        public MainPage()
        { 
            InitializeComponent();

            _db = new DatabaseHandler();
            Bookshelves = new ObservableCollection<Bookshelf>();

            PopulateBookshelves();
            BindingContext = this;
        }

        public void CreateBookshelfBtn_Clicked(object sender, EventArgs e)
        {
            if(!ConfirmMenu.IsVisible)
            {
                _db.CreateBookshelf();
                PopulateBookshelves();
            } 
        }

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
            }
        }

        public void ConfirmDeleteBtn_Clicked(object sender, EventArgs e) 
        {
            ConfirmMenu.IsVisible = false;
            ConfirmMenuBackground.IsVisible = false;

            _db.DeleteBookshelf(BookshelfId);
            PopulateBookshelves();
        }

        public void DontDeleteBtn_Clicked(object sender, EventArgs e)
        {
            ConfirmMenu.IsVisible = false;
            ConfirmMenuBackground.IsVisible = false;
        }

        public async void OpenBookshelfBtn_Clicked(object sender, EventArgs e)
        {
            // Ensure the sender is a Button
            Button clickedButton = sender as Button;

            // Check if the clickedButton is not null and safely access its BindingContext
            if (clickedButton != null)
            {
                Bookshelf selectedBookshelf = clickedButton.BindingContext as Bookshelf;
                await Navigation.PushAsync(new BookshelfPage(selectedBookshelf.Id));
            }
        }

        public void PopulateBookshelves()
        {
            Bookshelves.Clear();

            List<Bookshelf> updatedBookshelves = _db.GetAllBookshelves();

            int i = 1;
            foreach (Bookshelf bookshelf in updatedBookshelves)
            {
                bookshelf.BookshelfName = "Bookshelf " + i.ToString();
                Bookshelves.Add(bookshelf);
                i++;
            }

            // Update BookshelfCount.Text
            int numBookshelves = Bookshelves.Count;
            BookshelfCountLabel.Text = $"{numBookshelves} / 8";
        }
    }
}
