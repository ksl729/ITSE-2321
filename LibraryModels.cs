using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryObject_KLeach
{
    public class LibraryItem
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; } = new List<string>();
        public string Isbn { get; set; }
        public bool IsCheckedOut { get; set; }
        public DateTime DueDate { get; set; }

        public LibraryItem() { }

        public LibraryItem(string title, string author, string isbn)
        {
            Title = title;
            Isbn = isbn;
            if (!string.IsNullOrWhiteSpace(author))
            {
                Authors = author.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(a => a.Trim()).ToList();
            }
        }
    }

    public class Book : LibraryItem
    {
        public Book() : base() { }
        public Book(string title, string author, string isbn) : base(title, author, isbn) { }
    }

    public class Media : LibraryItem
    {
        public Media() : base() { }
        public Media(string title, string author, string isbn) : base(title, author, isbn) { }
    }

    public class Periodical : LibraryItem
    {
        public Periodical() : base() { }
        public Periodical(string title, string author, string isbn) : base(title, author, isbn) { }
    }

    public class LibraryCatalog
    {
        private List<LibraryItem> _items = new List<LibraryItem>();
        public void AddItem(LibraryItem item) => _items.Add(item);
        public List<LibraryItem> GetAllItems() => _items;
    }

    public class CheckoutLogEntry
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string Isbn { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
