using System;

namespace LibraryWpf.Models
{
    public class LoanView
    {
        public int Id { get; set; }
        public string BookTitle { get; set; } = "";
        public string ReaderName { get; set; } = "";
        public DateTime IssueDate { get; set; }
    }
}

