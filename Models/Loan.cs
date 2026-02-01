using System;

namespace LibraryWpf.Models
{
    public class Loan
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book? Book { get; set; }

        public int ReaderId { get; set; }
        public Reader? Reader { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime? ReturnDate { get; set; } // null = не возвращена
    }
}

