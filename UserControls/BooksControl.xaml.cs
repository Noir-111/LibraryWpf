using LibraryWpf.Data;
using LibraryWpf.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibraryWpf.UserControls
{
    public partial class BooksControl : UserControl
    {
        private int selectedId = 0;

        public BooksControl()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks()
        {
            using (var db = new LibraryContext())
            {
                DgBooks.ItemsSource = db.Books.AsNoTracking().ToList();
            }
        }

        private void DgBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgBooks.SelectedItem is Book b)
            {
                selectedId = b.Id;
                TbAuthor.Text = b.Author;
                TbTitle.Text = b.Title;
                TbYear.Text = b.Year.ToString();
                TbCopies.Text = b.Copies.ToString();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TbYear.Text.Trim(), out int year) ||
                !int.TryParse(TbCopies.Text.Trim(), out int copies))
            {
                MessageBox.Show("Год и количество должны быть числами!");
                return;
            }

            var book = new Book
            {
                Author = TbAuthor.Text.Trim(),
                Title = TbTitle.Text.Trim(),
                Year = year,
                Copies = copies
            };

            using (var db = new LibraryContext())
            {
                db.Books.Add(book);
                db.SaveChanges();
            }

            LoadBooks();
            ClearForm();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Выберите книгу в таблице.");
                return;
            }

            if (!int.TryParse(TbYear.Text.Trim(), out int year) ||
                !int.TryParse(TbCopies.Text.Trim(), out int copies))
            {
                MessageBox.Show("Год и количество должны быть числами!");
                return;
            }

            using (var db = new LibraryContext())
            {
                var book = db.Books.FirstOrDefault(x => x.Id == selectedId);
                if (book == null) return;

                book.Author = TbAuthor.Text.Trim();
                book.Title = TbTitle.Text.Trim();
                book.Year = year;
                book.Copies = copies;

                db.SaveChanges();
            }

            LoadBooks();
            ClearForm();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Выберите книгу в таблице.");
                return;
            }

            using (var db = new LibraryContext())
            {
                var book = db.Books.FirstOrDefault(x => x.Id == selectedId);
                if (book == null) return;

                // Простейшая защита: если есть активные выдачи этой книги — не удаляем
                bool hasActiveLoans = db.Loans.Any(l => l.BookId == selectedId && l.ReturnDate == null);
                if (hasActiveLoans)
                {
                    MessageBox.Show("Нельзя удалить: книга сейчас выдана.");
                    return;
                }

                db.Books.Remove(book);
                db.SaveChanges();
            }

            LoadBooks();
            ClearForm();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            selectedId = 0;
            TbAuthor.Text = "";
            TbTitle.Text = "";
            TbYear.Text = "";
            TbCopies.Text = "";
            DgBooks.SelectedItem = null;
        }
    }
}

