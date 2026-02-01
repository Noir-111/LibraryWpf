using System.Windows;
using System.Windows.Controls;

namespace LibraryWpf.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Чтобы событие не срабатывало лишний раз от внутренних элементов
            if (e.Source is not TabControl)
                return;

            // Обновляем то, что должно подтягивать свежие данные
            IssueUc.RefreshData();
            ReturnUc.RefreshData();
        }
    }
}

