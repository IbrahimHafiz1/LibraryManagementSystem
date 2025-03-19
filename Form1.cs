using System;
using System.Windows.Forms;
using System.Linq;

namespace LibraryManagementSystem
{
    public partial class Form1 : Form
    {
        private Library library = new Library();
        private string selectedBookISBN = "";
        private string selectedBookAuthor = "";
        private string userID = "Yes"; // Placeholder for user authentication

        public Form1()
        {
            InitializeComponent();
            lblSearchResult.Visible = false;
            btnBorrow.Visible = false;
            btnReturn.Visible = false;

            dgvBooks.CellClick += dgvBooks_CellClick;  // Ensure selection works

            UpdateBookGrid();
        }

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string author = txtAuthor.Text.Trim();
            string isbn = txtISBN.Text.Trim();

            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(author) && !string.IsNullOrWhiteSpace(isbn))
            {
                library.AddBook(new Book(title, author, isbn, 1)); // Add 1 copy
                MessageBox.Show($"Book '{title}' by {author} added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateBookGrid();
            }
            else
            {
                MessageBox.Show("Please fill all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = txtSearch.Text.Trim();
            var result = library.SearchBooks(query);

            if (result.Count > 0)
            {
                lblSearchResult.Text = $"Found {result.Count} books";
                lblSearchResult.Visible = true;
                btnBorrow.Visible = true;
                btnReturn.Visible = true;
            }
            else
            {
                lblSearchResult.Text = "No books found";
                lblSearchResult.Visible = true;
                btnBorrow.Visible = false;
                btnReturn.Visible = false;
            }
            UpdateBookGrid();
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedBookISBN) || string.IsNullOrEmpty(selectedBookAuthor))
            {
                MessageBox.Show("Please select a book first by clicking on a row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool success = library.BorrowBook(selectedBookISBN, selectedBookAuthor, userID);
            if (success)
            {
                MessageBox.Show($"Book '{selectedBookISBN}' by {selectedBookAuthor} borrowed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No available copies for borrowing!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            UpdateBookGrid();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedBookISBN) || string.IsNullOrEmpty(selectedBookAuthor))
            {
                MessageBox.Show("Please select a book first by clicking on a row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool success = library.ReturnBook(selectedBookISBN, selectedBookAuthor, userID);
            if (success)
            {
                MessageBox.Show($"Book '{selectedBookISBN}' by {selectedBookAuthor} returned successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("You cannot return this book because it was not borrowed by you!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            UpdateBookGrid();
        }

        private void dgvBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedBookISBN = dgvBooks.Rows[e.RowIndex].Cells["ISBN"].Value?.ToString();
                selectedBookAuthor = dgvBooks.Rows[e.RowIndex].Cells["Author"].Value?.ToString();

                if (!string.IsNullOrEmpty(selectedBookISBN) && !string.IsNullOrEmpty(selectedBookAuthor))
                {
                    MessageBox.Show($"Selected Book:\nTitle: {dgvBooks.Rows[e.RowIndex].Cells["Title"].Value}\nAuthor: {selectedBookAuthor}", "Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Invalid selection. Please select a valid book row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void UpdateBookGrid()
        {
            dgvBooks.DataSource = null;
            dgvBooks.DataSource = library.GetAllBooks()
                .GroupBy(b => new { b.Title, b.Author }) // Group books by title & author
                .Select(group => new
                {
                    Title = group.Key.Title,
                    Author = group.Key.Author,
                    ISBN = group.First().ISBN,
                    TotalCopies = group.Sum(b => b.Quantity),
                    AvailableCopies = group.Sum(b => b.Quantity) - group.Sum(b => b.BorrowedCount),
                    BorrowedCopies = group.Sum(b => b.BorrowedCount)
                })
                .OrderBy(book => book.Title)
                .ThenBy(book => book.Author)
                .ToList();
        }
    }
}
