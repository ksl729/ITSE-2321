using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LibraryObject_KLeach
{
    public class DatabaseManager
    {
        private readonly string _connectionString =
            @"Server=IO126-1\SQLEXPRESS;Database=activity4_librarydb;Trusted_Connection=True;TrustServerCertificate=True;";

        public void SaveItemToDatabase(LibraryItem item)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // 1. Insert the Item into the Items table
                // We use SCOPE_IDENTITY() to get the ID of the row we just created
                string itemSql = @"INSERT INTO Items (Title, ISBN, ItemType, IsCheckedOut, DueDate) 
                                   VALUES (@title, @isbn, @type, @isCheckedOut, @dueDate);
                                   SELECT SCOPE_IDENTITY();";

                int newItemId;

                using (SqlCommand cmd = new SqlCommand(itemSql, conn))
                {
                    cmd.Parameters.AddWithValue("@title", item.Title);
                    cmd.Parameters.AddWithValue("@isbn", item.Isbn);
                    cmd.Parameters.AddWithValue("@type", item.GetType().Name);
                    cmd.Parameters.AddWithValue("@isCheckedOut", item.IsCheckedOut ? 1 : 0);
                    cmd.Parameters.AddWithValue("@dueDate", item.DueDate == DateTime.MinValue ? (object)DBNull.Value : item.DueDate);

                    newItemId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // 2. Handle Authors (Relational logic)
                foreach (string authorName in item.Authors)
                {
                    int authorId = GetOrCreateAuthor(conn, authorName);
                    LinkItemToAuthor(conn, newItemId, authorId);
                }
            }
        }

        private int GetOrCreateAuthor(SqlConnection conn, string authorName)
        {
            // Check if author already exists
            string checkSql = "SELECT AuthorID FROM Authors WHERE AuthorName = @name";
            using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
            {
                checkCmd.Parameters.AddWithValue("@name", authorName);
                object result = checkCmd.ExecuteScalar();
                if (result != null) return Convert.ToInt32(result);
            }

            // If not, insert them
            string insertSql = "INSERT INTO Authors (AuthorName) VALUES (@name); SELECT SCOPE_IDENTITY();";
            using (SqlCommand insertCmd = new SqlCommand(insertSql, conn))
            {
                insertCmd.Parameters.AddWithValue("@name", authorName);
                return Convert.ToInt32(insertCmd.ExecuteScalar());
            }
        }

        private void LinkItemToAuthor(SqlConnection conn, int itemId, int authorId)
        {
            string linkSql = "INSERT INTO ItemAuthors (ItemID, AuthorID) VALUES (@itemId, @authorId)";
            using (SqlCommand linkCmd = new SqlCommand(linkSql, conn))
            {
                linkCmd.Parameters.AddWithValue("@itemId", itemId);
                linkCmd.Parameters.AddWithValue("@authorId", authorId);
                linkCmd.ExecuteNonQuery();
            }
        }

        // Finds an item's database ID using its ISBN (returns -1 if not found)
        public int GetItemIdByIsbn(string isbn)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT ItemID FROM Items WHERE ISBN = @isbn";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@isbn", isbn);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        // Logs a checkout and marks the item as checked out
        public void LogCheckout(int itemId, DateTime checkoutDate, DateTime dueDate)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO CheckoutLog (ItemID, CheckoutDate, DueDate)
                               VALUES (@itemId, @checkoutDate, @dueDate);
                               UPDATE Items SET IsCheckedOut = 1, DueDate = @dueDate
                               WHERE ItemID = @itemId;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@checkoutDate", checkoutDate);
                    cmd.Parameters.AddWithValue("@dueDate", dueDate);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Loads all items (with their authors) from the database
        public List<string> LoadAllItems()
        {
            List<string> items = new List<string>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = @"SELECT i.Title, i.ISBN, i.ItemType, i.IsCheckedOut,
                                      ISNULL(STRING_AGG(a.AuthorName, ', '), 'Unknown') AS Authors
                               FROM Items i
                               LEFT JOIN ItemAuthors ia ON i.ItemID = ia.ItemID
                               LEFT JOIN Authors a ON ia.AuthorID = a.AuthorID
                               GROUP BY i.ItemID, i.Title, i.ISBN, i.ItemType, i.IsCheckedOut
                               ORDER BY i.Title;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string status = Convert.ToBoolean(reader["IsCheckedOut"]) ? " [CHECKED OUT]" : "";
                        items.Add($"{reader["ItemType"]}: {reader["Title"]} by {reader["Authors"]} (ISBN {reader["ISBN"]}){status}");
                    }
                }
            }

            return items;
        }
    }
}
