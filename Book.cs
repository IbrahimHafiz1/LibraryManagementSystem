public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int Quantity { get; set; }
    public int BorrowedCount { get; set; }
    public List<string> BorrowedBy { get; set; } // Track users who borrowed the book

    public bool IsAvailable => Quantity > BorrowedCount; 

    public Book(string title, string author, string isbn, int quantity)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        Quantity = quantity;
        BorrowedCount = 0;
        BorrowedBy = new List<string>();  // Initialize list
    }
}
