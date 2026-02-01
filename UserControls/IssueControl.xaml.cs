using LibraryWpf.Data;
using LibraryWpf.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibraryWpf.UserControls
{
    public partial class IssueControl : UserControl
    {
        public IssueControl()
        {
            InitializeComponent();
            LoadData();
        }
        public void RefreshData()
        {
            LoadData();
        }


        private void LoadData()
        {
            using (var db = new LibraryContext())
            {
                CbBooks.ItemsSource = db.Books.AsNoTracking().ToList();
                CbReaders.ItemsSource = db.Readers.AsNoTracking().ToList();
            }

            // что показывать в комбобоксе
            CbBooks.DisplayMemberPath = "Title";
            CbBooks.SelectedValuePath = "Id";

            CbReaders.DisplayMemberPath = "FullName";
            CbReaders.SelectedValuePath = "Id";
        }

        private void BtnIssue_Click(object sender, RoutedEventArgs e)
        {
            if (CbBooks.SelectedItem is not Book book ||
                CbReaders.SelectedItem is not Reader reader)
            {
                MessageBox.Show("Выберите книгу и читателя.");
                return;
            }

            using (var db = new LibraryContext())
            {
                // берём книгу из БД (чтобы актуальные copies)
                var dbBook = db.Books.FirstOrDefault(b => b.Id == book.Id);
                if (dbBook == null) return;

                if (dbBook.Copies <= 0)
                {
                    MessageBox.Show("Нельзя выдать: экземпляров нет!");
                    return;
                }

                var loan = new Loan
                {
                    BookId = dbBook.Id,
                    ReaderId = reader.Id,
                    IssueDate = DateTime.Now,
                    ReturnDate = null
                };

                db.Loans.Add(loan);

                // уменьшаем количество экземпляров
                dbBook.Copies -= 1;

                db.SaveChanges();
            }

            MessageBox.Show("Книга выдана!");
            LoadData();
        }
    }
}

