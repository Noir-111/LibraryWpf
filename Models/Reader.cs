using System.Collections.Generic;

namespace LibraryWpf.Models
{
    public class Reader
    {
        public int Id { get; set; }

        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";

        public List<Loan> Loans { get; set; } = new List<Loan>();
    }
}
