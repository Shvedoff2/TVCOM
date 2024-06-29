using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    public partial class ReportDoljWindow : Window
    {
        public ReportDoljWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(new LoginService(), new RegistService(), new ProverkaService(), new InsertService(), new ReportService(), new ReportAllService(), new ReportDoljService());
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=Тивиком;Integrated Security=True"))
            {
                conn.Open();
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Должности", conn))
                {
                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt);
                    Doljnost.ItemsSource = dt.DefaultView;
                    Doljnost.DisplayMemberPath = "Название";
                    Doljnost.SelectedValuePath = "ID_Должности";
                }
            }
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
