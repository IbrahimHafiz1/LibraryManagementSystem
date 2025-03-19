using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem
{
    public class Library
    {
        private List<Book> books = new List<Book>();

        public void AddBook(Book book)
        {
            books.Add(book);
        }

        public List<Book> SearchBooks(string query)
        {
            return books.Where(b => b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                    b.Author.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                    b.ISBN.Contains(query)).ToList();
        }

        public List<Book> GetAllBooks()
        {
            return books;
        }

        public bool BorrowBook(string isbn, string author, string userID)
        {
            var book = books.FirstOrDefault(b => b.ISBN == isbn && b.Author == author && b.IsAvailable);
            if (book != null)
            {
                book.BorrowedCount++;
                book.BorrowedBy.Add(userID);  // Track user who borrowed it
                return true;
            }
            return false;
        }

        public bool ReturnBook(string isbn, string author, string userID)
        {
            var book = books.FirstOrDefault(b => b.ISBN == isbn && b.Author == author && b.BorrowedBy.Contains(userID));
            if (book != null)
            {
                book.BorrowedCount--;
                book.BorrowedBy.Remove(userID); // Remove user from borrowed list
                return true;
            }
            return false;
        }
    }
}
