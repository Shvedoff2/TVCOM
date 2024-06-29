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
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
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
                    Dolj.ItemsSource = dt.DefaultView;
                    Dolj.DisplayMemberPath = "Название";
                    Dolj.SelectedValuePath = "ID_Должности";
                }
            }
        }
    }
}
