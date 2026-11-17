using System;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryObject_KLeach
{
    public class Form1 : Form
    {
        private LibraryCatalog _catalog = new LibraryCatalog();
        private DatabaseManager _db = new DatabaseManager();

        // Controls
        private ComboBox cmbType;
        private TextBox txtTitle;
        private TextBox txtAuthors;
        private TextBox txtIsbn;
        private Button btnSave;
        private Button btnCheckout;
        private Button btnRefresh;
        private ListBox lstItems;

        public Form1()
        {
            // Form settings
            Text = "Library Catalog";
            Size = new Size(500, 520);
            StartPosition = FormStartPosition.CenterScreen;

            // Item type
            Controls.Add(new Label { Text = "Item Type:", Location = new Point(20, 20), AutoSize = true });
            cmbType = new ComboBox
            {
                Location = new Point(130, 17),
                Width = 320,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbType.Items.AddRange(new object[] { "Book", "Media", "Periodical" });
            cmbType.SelectedIndex = 0;
            Controls.Add(cmbType);

            // Title
            Controls.Add(new Label { Text = "Title:", Location = new Point(20, 60), AutoSize = true });
            txtTitle = new TextBox { Location = new Point(130, 57), Width = 320 };
            Controls.Add(txtTitle);

            // Authors
            Controls.Add(new Label { Text = "Author(s):", Location = new Point(20, 100), AutoSize = true });
            txtAuthors = new TextBox { Location = new Point(130, 97), Width = 320 };
            Controls.Add(txtAuthors);

            Controls.Add(new Label
            {
                Text = "(Separate multiple authors with ; or ,)",
                Location = new Point(130, 122),
                AutoSize = true,
                ForeColor = Color.Gray
            });

            // ISBN
            Controls.Add(new Label { Text = "ISBN:", Location = new Point(20, 150), AutoSize = true });
            txtIsbn = new TextBox { Location = new Point(130, 147), Width = 320 };
            Controls.Add(txtIsbn);

            // Buttons
            btnSave = new Button { Text = "Save Item", Location = new Point(130, 190), Width = 150 };
            btnSave.Click += btnSave_Click;
            Controls.Add(btnSave);

            btnCheckout = new Button { Text = "Check Out by ISBN", Location = new Point(300, 190), Width = 150 };
            btnCheckout.Click += btnCheckout_Click;
            Controls.Add(btnCheckout);

            // Refresh button
            btnRefresh = new Button { Text = "Refresh List", Location = new Point(350, 236), Width = 100 };
            btnRefresh.Click += btnRefresh_Click;
            Controls.Add(btnRefresh);

            // Item list
            Controls.Add(new Label { Text = "Saved Items:", Location = new Point(20, 240), AutoSize = true });
            lstItems = new ListBox { Location = new Point(20, 265), Size = new Size(430, 200) };
            Controls.Add(lstItems);

            // Load existing items when the form opens
            LoadItemsFromDatabase();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtIsbn.Text))
            {
                MessageBox.Show("Please enter a title and ISBN.");
                return;
            }

            // Create the correct object type
            LibraryItem item;
            switch (cmbType.SelectedItem.ToString())
            {
                case "Media":
                    item = new Media(txtTitle.Text, txtAuthors.Text, txtIsbn.Text);
                    break;
                case "Periodical":
                    item = new Periodical(txtTitle.Text, txtAuthors.Text, txtIsbn.Text);
                    break;
                default:
                    item = new Book(txtTitle.Text, txtAuthors.Text, txtIsbn.Text);
                    break;
            }

            try
            {
                _catalog.AddItem(item);          // store the object
                _db.SaveItemToDatabase(item);    // store it in the database

                LoadItemsFromDatabase();         // refresh the list from the database
                MessageBox.Show("Item saved to the database!");

                txtTitle.Clear();
                txtAuthors.Clear();
                txtIsbn.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIsbn.Text))
            {
                MessageBox.Show("Enter the ISBN of the item to check out.");
                return;
            }

            try
            {
                int id = _db.GetItemIdByIsbn(txtIsbn.Text);
                if (id == -1)
                {
                    MessageBox.Show("Item not found.");
                    return;
                }

                _db.LogCheckout(id, DateTime.Now, DateTime.Now.AddDays(14));
                MessageBox.Show("Item checked out! Due in 14 days.");

                LoadItemsFromDatabase();         // show the [CHECKED OUT] tag
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadItemsFromDatabase();
        }

        private void LoadItemsFromDatabase()
        {
            try
            {
                lstItems.Items.Clear();
                foreach (string line in _db.LoadAllItems())
                {
                    lstItems.Items.Add(line);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load items: " + ex.Message);
            }
        }
    }
}
