using System;
using System.Collections.Generic;

namespace LibraryManagementSystem
{
    public class User
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public List<string> BorrowedBooks { get; set; }

        public User(string userID, string name)
        {
            UserID = userID;
            Name = name;
            BorrowedBooks = new List<string>();
        }

        public void BorrowBook(string isbn)
        {
            BorrowedBooks.Add(isbn);
        }

        public void ReturnBook(string isbn)
        {
            BorrowedBooks.Remove(isbn);
        }

        public override string ToString()
        {
            return $"{UserID} - {Name} (Borrowed: {string.Join(", ", BorrowedBooks)})";
        }
    }
}
