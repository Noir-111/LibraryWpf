using LibraryWpf.Data;
using LibraryWpf.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibraryWpf.UserControls
{
    public partial class ReadersControl : UserControl
    {
        private int selectedId = 0;

        public ReadersControl()
        {
            InitializeComponent();
            LoadReaders();
        }

        private void LoadReaders()
        {
            using (var db = new LibraryContext())
            {
                DgReaders.ItemsSource = db.Readers.AsNoTracking().ToList();
            }
        }

        private void DgReaders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgReaders.SelectedItem is Reader r)
            {
                selectedId = r.Id;
                TbName.Text = r.FullName;
                TbPhone.Text = r.Phone;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var reader = new Reader
            {
                FullName = TbName.Text.Trim(),
                Phone = TbPhone.Text.Trim()
            };

            using (var db = new LibraryContext())
            {
                db.Readers.Add(reader);
                db.SaveChanges();
            }

            LoadReaders();
            ClearForm();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Выберите читателя в таблице.");
                return;
            }

            using (var db = new LibraryContext())
            {
                var reader = db.Readers.FirstOrDefault(x => x.Id == selectedId);
                if (reader == null) return;

                reader.FullName = TbName.Text.Trim();
                reader.Phone = TbPhone.Text.Trim();
                db.SaveChanges();
            }

            LoadReaders();
            ClearForm();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Выберите читателя в таблице.");
                return;
            }

            using (var db = new LibraryContext())
            {
                bool hasActiveLoans = db.Loans.Any(l => l.ReaderId == selectedId && l.ReturnDate == null);
                if (hasActiveLoans)
                {
                    MessageBox.Show("Нельзя удалить: у читателя есть невозвращенные книги.");
                    return;
                }

                var reader = db.Readers.FirstOrDefault(x => x.Id == selectedId);
                if (reader == null) return;

                db.Readers.Remove(reader);
                db.SaveChanges();
            }

            LoadReaders();
            ClearForm();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            selectedId = 0;
            TbName.Text = "";
            TbPhone.Text = "";
            DgReaders.SelectedItem = null;
        }
    }
}

