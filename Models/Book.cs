using System.Collections.Generic;

namespace LibraryWpf.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Author { get; set; } = "";
        public string Title { get; set; } = "";

        public int Year { get; set; }
        public int Copies { get; set; }   // сколько экземпляров доступно

        public List<Loan> Loans { get; set; } = new List<Loan>();
    }
}

