using LibraryWpf.Data;
using LibraryWpf.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibraryWpf.UserControls
{
    public partial class ReturnControl : UserControl
    {
        public ReturnControl()
        {
            InitializeComponent();
            LoadActiveLoans();
        }

        public void RefreshData()
        {
            LoadActiveLoans();
        }

        private void LoadActiveLoans()
        {
            using (var db = new LibraryContext())
            {
                var list = db.Loans
                    .Include(l => l.Book)
                    .Include(l => l.Reader)
                    .Where(l => l.ReturnDate == null)
                    .Select(l => new LoanView
                    {
                        Id = l.Id,
                        BookTitle = l.Book != null ? l.Book.Title : "",
                        ReaderName = l.Reader != null ? l.Reader.FullName : "",
                        IssueDate = l.IssueDate
                    })
                    .AsNoTracking()
                    .ToList();

                DgLoans.ItemsSource = list;
            }
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (DgLoans.SelectedItem is not LoanView selected)
            {
                MessageBox.Show("Выберите выдачу в таблице.");
                return;
            }

            using (var db = new LibraryContext())
            {
                var loan = db.Loans
                    .Include(l => l.Book)
                    .FirstOrDefault(l => l.Id == selected.Id);

                if (loan == null) return;

                loan.ReturnDate = DateTime.Now;

                if (loan.Book != null)
                    loan.Book.Copies += 1;

                db.SaveChanges();
            }

            MessageBox.Show("Возврат принят!");
            LoadActiveLoans(); // обновляем сразу
        }
    }
}

