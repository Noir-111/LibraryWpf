using LibraryWpf.Data;
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

        private void LoadActiveLoans()
        {
            using (var db = new LibraryContext())
            {
                // Загружаем выдачи, где ReturnDate == null
                var list = db.Loans
                    .Include(l => l.Book)
                    .Include(l => l.Reader)
                    .Where(l => l.ReturnDate == null)
                    .Select(l => new
                    {
                        l.Id,
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
            if (DgLoans.SelectedItem == null)
            {
                MessageBox.Show("Выберите выдачу в таблице.");
                return;
            }

            // Так как ItemsSource анонимный тип, достанем Id через reflection (просто и без MVVM)
            var idProp = DgLoans.SelectedItem.GetType().GetProperty("Id");
            int loanId = (int)idProp.GetValue(DgLoans.SelectedItem);

            using (var db = new LibraryContext())
            {
                var loan = db.Loans.Include(l => l.Book).FirstOrDefault(l => l.Id == loanId);
                if (loan == null) return;

                loan.ReturnDate = DateTime.Now;

                if (loan.Book != null)
                    loan.Book.Copies += 1;

                db.SaveChanges();
            }

            MessageBox.Show("Возврат принят!");
            LoadActiveLoans();
        }
    }
}

