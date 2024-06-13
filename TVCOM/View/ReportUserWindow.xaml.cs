using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TVCOM.Model;
using TVCOM.ViewModel;

namespace TVCOM.View
{
    /// <summary>
    /// Логика взаимодействия для ReportUserWindow.xaml
    /// </summary>
    public partial class ReportUserWindow : Window
    {
        public ReportUserWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(new LoginService(), new RegistService(), new ProverkaService(), new InsertService(), new ReportService());
        }

        private void StartMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ComboBoxItem)StartMonth.SelectedItem;
            int selectedMonth = int.Parse((string)selectedItem.Tag);

            // Определяем количество дней в выбранном месяце
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, selectedMonth);

            // Очищаем комбобокс
            StartDay.Items.Clear();

            // Заполняем первый комбобокс числами от 1 до daysInMonth
            for (int i = 1; i <= daysInMonth; i++)
            {
                StartDay.Items.Add(i);
            }
        }

        private void FinMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ComboBoxItem)FinMonth.SelectedItem;
            int selectedMonth = int.Parse((string)selectedItem.Tag);

            // Определяем количество дней в выбранном месяце
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, selectedMonth);

            // Очищаем комбобокс
            FinDay.Items.Clear();

            // Заполняем первый комбобокс числами от 1 до daysInMonth
            for (int i = 1; i <= daysInMonth; i++)
            {
                FinDay.Items.Add(i);
            }
        }
    }
}
