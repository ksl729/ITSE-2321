using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryObject_KLeach
{
    public class LibraryItem
    {
        // Five attributes with corresponding access modifiers
        private string title;
        private List<string> authors;
        private string isbn;
        private bool isCheckedOut;
        private DateTime dueDate;

        // Properties for the attributes - Title, Author, Isbn & isCheckedOut
        public string Title
        {
            get { return title; }
            set { title = value; }
        }




        // Single-author convenience property (first author) kept for compatibility
        public string Author
        {
            get { return authors != null && authors.Count > 0 ? authors[0] : "Unknown Author"; }
            set { authors = new List<string> { value }; }
        }

        // Multi-author support
        public List<string> Authors
        {
            get { return authors; }
            set { authors = value ?? new List<string>(); }
        }

        public string Isbn
        {
            get { return isbn; }
            set { isbn = value; }
        }

        public bool IsCheckedOut
        {
            get { return isCheckedOut; }
            set { isCheckedOut = value; }
        }

        // Default constructor
        public LibraryItem()
        {
            title = "Unknown Title";
            authors = new List<string> { "Unknown Author" };
            isbn = "000-0000000000";
            isCheckedOut = false;
            dueDate = DateTime.MinValue;
        }

        // Constructor with parameters
        public LibraryItem(string title, string author, string isbn)
        {
            this.title = title;
            // Support multiple authors separated by semicolon or comma
            if (!string.IsNullOrWhiteSpace(author))
            {
                var parts = author.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(a => a.Trim()).ToList();
                this.authors = parts.Count > 0 ? parts : new List<string> { "Unknown Author" };
            }
            else
            {
                this.authors = new List<string> { "Unknown Author" };
            }
            this.isbn = isbn;
            this.isCheckedOut = false;
            this.dueDate = DateTime.MinValue;
        }

        // Method 1: Check out book
        public virtual void CheckOut(int outDays)
        {
            if (!isCheckedOut)
            {
                isCheckedOut = true;
                dueDate = DateTime.Now.AddDays(outDays);
                Console.WriteLine($"'{title}' has been checked out. Due back on {dueDate.ToShortDateString()}.");
            }
            else
            {
                Console.WriteLine($"'{title}' is already checked out.");
            }
        }

        // Method 2: Return the item
        public virtual void ReturnItem()
        {
            if (isCheckedOut)
            {
                isCheckedOut = false;
                dueDate = DateTime.MinValue;
                Console.WriteLine($"'{title}' has been returned. Thank you!");
            }
            else
            {
                Console.WriteLine($"'{title}' was not checked out.");
            }
        }

        // Method 3: Display item information
        public void DisplayInfo()
        {
            Console.WriteLine("----- Library Item Info -----");
            Console.WriteLine($"Title: {title}");
            Console.WriteLine($"Author(s): {string.Join(", ", authors)}");
            Console.WriteLine($"ISBN: {isbn}");
            Console.WriteLine($"Checked Out: {isCheckedOut}");
            if (isCheckedOut)
            {
                Console.WriteLine($"Due Date: {dueDate.ToShortDateString()}");
            }
        }
    }

    // Catalog class to hold and search multiple LibraryItem objects
    public class LibraryCatalog
    {
        private List<LibraryItem> items;

        public LibraryCatalog()
        {
            items = new List<LibraryItem>();
        }

        public void AddItem(LibraryItem item)
        {
            items.Add(item);
        }

        public List<LibraryItem> SearchByTitle(string searchText)
        {
            return items.Where(i => i.Title.ToLower().Contains(searchText.ToLower())).ToList();
        }

        public List<LibraryItem> SearchByAuthor(string searchText)
        {
            return items.Where(i => i.Authors != null && i.Authors.Any(a => a.ToLower().Contains(searchText.ToLower()))).ToList();
        }

        public List<LibraryItem> SearchByIsbn(string searchText)
        {
            return items.Where(i => i.Isbn.ToLower().Contains(searchText.ToLower())).ToList();
        }
    }

    // Derived classes demonstrate inheritance and specialized checkout rules
    public class Book : LibraryItem
    {
        public Book() : base() { }
        public Book(string title, string author, string isbn) : base(title, author, isbn) { }

        // Default book loan period
        public void CheckOut()
        {
            CheckOut(14);
        }

        public override void CheckOut(int outDays)
        {
            bool wasCheckedOut = this.IsCheckedOut;
            base.CheckOut(outDays);
            // If checkout succeeded, log it
            if (!wasCheckedOut && this.IsCheckedOut)
            {
                BookCheckoutLog.AddEntry(this, DateTime.Now, DateTime.Now.AddDays(outDays));
            }
        }
    }

    public class Media : LibraryItem
    {
        public Media() : base() { }
        public Media(string title, string author, string isbn) : base(title, author, isbn) { }

        // Media items have shorter loan period
        public void CheckOut()
        {
            CheckOut(7);
        }
    }

    public class Periodical : LibraryItem
    {
        public Periodical() : base() { }
        public Periodical(string title, string author, string isbn) : base(title, author, isbn) { }

        // Periodicals often cannot be checked out or have very short periods
        public void CheckOut()
        {
            CheckOut(3);
        }
    }

    // Simple static log to track book checkouts
    public static class BookCheckoutLog
    {
        private static List<CheckoutLogEntry> entries = new List<CheckoutLogEntry>();

        public static void AddEntry(LibraryItem item, DateTime checkoutDate, DateTime dueDate)
        {
            entries.Add(new CheckoutLogEntry
            {
                Title = item.Title,
                Authors = item.Authors != null ? new List<string>(item.Authors) : new List<string> { item.Author },
                Isbn = item.Isbn,
                CheckoutDate = checkoutDate,
                DueDate = dueDate
            });
        }

        public static void DisplayLog()
        {
            Console.WriteLine();
            Console.WriteLine("----- Book Checkout Log -----");
            if (entries.Count == 0)
            {
                Console.WriteLine("No checkouts logged.");
                return;
            }

            foreach (var e in entries)
            {
                Console.WriteLine($"Title: {e.Title}");
                Console.WriteLine($"Author(s): {string.Join(", ", e.Authors)}");
                Console.WriteLine($"ISBN: {e.Isbn}");
                Console.WriteLine($"Checked Out: {e.CheckoutDate}");
                Console.WriteLine($"Due Date: {e.DueDate.ToShortDateString()}");
                Console.WriteLine();
            }
        }

        private class CheckoutLogEntry
        {
            public string Title { get; set; }
            public List<string> Authors { get; set; }
            public string Isbn { get; set; }
            public DateTime CheckoutDate { get; set; }
            public DateTime DueDate { get; set; }
        }
    }

    class LibraryObject_KLeach
    {
        static void Main(string[] args)
        {
            LibraryCatalog catalog = new LibraryCatalog();

            // Using the default constructor
            LibraryItem item1 = new LibraryItem();
            catalog.AddItem(item1);

            // Using constructor with parameters for Library Catalog - use Book derived class
            Book item2 = new Book("The Great Gatsby", "F. Scott Fitzgerald", "978-0743273565");
            catalog.AddItem(item2);

            Book item3 = new Book("To Kill a Mockingbird", "Harper Lee", "978-0061120084");
            catalog.AddItem(item3);

            Book item4 = new Book("1984", "George Orwell", "978-0451524935");
            catalog.AddItem(item4);

            Book item5 = new Book("Don Quixote", "Miguel de Cervantes Saavedra", "978-0060188702");
            catalog.AddItem(item5);

            Book item6 = new Book("A Tale of Two Cities", "Charles Dickens", "978-0582030473");
            catalog.AddItem(item6);

            // Example with multiple authors
            Book item7 = new Book("The Lord of The Rings", "J.R.R. Tolkien; Alan Lee", "978-0395647387");
            catalog.AddItem(item7);

            Book item8 = new Book("Pride and Prejudice", "Jane Austen", "978-0141439518");
            catalog.AddItem(item8);

            Book item9 = new Book("Moby Dick", "Herman Melville", "978-0679783275");
            catalog.AddItem(item9);

            Book item10 = new Book("The Catcher in the Rye", "J.D. Salinger", "978-1439576649");
            catalog.AddItem(item10);

            bool running = true;

            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("===== Library Catalog Menu =====");
                Console.WriteLine("1. Search by Title");
                Console.WriteLine("2. Search by Author");
                Console.WriteLine("3. Search by ISBN");
                Console.WriteLine("4. Check Out a Book");
                Console.WriteLine("5. Return a Book");
                Console.WriteLine("6. Exit");
                Console.WriteLine("7. Show Checkout Log");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                List<LibraryItem> results = new List<LibraryItem>();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter title to search: ");
                        string titleSearch = Console.ReadLine();
                        results = catalog.SearchByTitle(titleSearch);
                        DisplayResults(results);
                        break;

                    case "2":
                        Console.Write("Enter author to search: ");
                        string authorSearch = Console.ReadLine();
                        results = catalog.SearchByAuthor(authorSearch);
                        DisplayResults(results);
                        break;

                    case "3":
                        Console.Write("Enter ISBN to search: ");
                        string isbnSearch = Console.ReadLine();
                        results = catalog.SearchByIsbn(isbnSearch);
                        DisplayResults(results);
                        break;

                    case "4": // Allows user to checkout book they have searched if available
                        CheckOutBook(catalog);
                        break;

                    case "5":// Allows user to return book they have searched if available
                        ReturnBook(catalog);
                        break;

                    case "6": // Closes program
                        running = false;
                        break;

                    case "7":
                        BookCheckoutLog.DisplayLog();
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

            Console.WriteLine("Goodbye!");
        }

        // Helper method to display search results
        static void DisplayResults(List<LibraryItem> results)
        {
            if (results.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"Found {results.Count} result(s):");
                foreach (var item in results)
                {
                    item.DisplayInfo();
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No matching items found.");
            }
        }

        // Lets the user search for a book by title and check it out
        static void CheckOutBook(LibraryCatalog catalog)
        {
            Console.Write("Enter the title of the book to check out: ");
            string titleSearch = Console.ReadLine();
            List<LibraryItem> results = catalog.SearchByTitle(titleSearch);

            if (results.Count == 0)
            {
                Console.WriteLine("No matching items found.");
                return;
            }

            LibraryItem selectedItem = SelectItemFromResults(results);

            if (selectedItem != null)
            {
                Console.Write("Enter number of days to check out: ");
                string daysInput = Console.ReadLine();

                if (int.TryParse(daysInput, out int days))
                {
                    selectedItem.CheckOut(days);
                }
                else
                {
                    Console.WriteLine("Invalid number of days entered.");
                }
            }
        }

        // Lets the user search for a book by title and return it
        static void ReturnBook(LibraryCatalog catalog)
        {
            Console.Write("Enter the title of the book to return: ");
            string titleSearch = Console.ReadLine();
            List<LibraryItem> results = catalog.SearchByTitle(titleSearch);

            if (results.Count == 0)
            {
                Console.WriteLine("No matching items found.");
                return;
            }

            LibraryItem selectedItem = SelectItemFromResults(results);

            if (selectedItem != null)
            {
                selectedItem.ReturnItem();
            }
        }

        // Helper method that lets the user pick a specific item if multiple results are found
        static LibraryItem SelectItemFromResults(List<LibraryItem> results)
        {
            if (results.Count == 1)
            {
                return results[0];
            }

            Console.WriteLine();
            Console.WriteLine("Multiple matches found. Please select one:");
            for (int i = 0; i < results.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {results[i].Title} by {results[i].Author} (ISBN: {results[i].Isbn})");
            }

            Console.Write("Enter the number of your selection: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int selectionIndex) && selectionIndex >= 1 && selectionIndex <= results.Count)
            {
                return results[selectionIndex - 1];
            }

            Console.WriteLine("Invalid selection.");
            return null;
        }
    }
}