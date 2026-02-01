using LibraryWpf.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryWpf.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // ПРОСТО: строка подключения прямо здесь
            string cs = "Server=DESKTOP-2DRRT6E\\SQLEXPRESS;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(cs);
        }
    }
}

