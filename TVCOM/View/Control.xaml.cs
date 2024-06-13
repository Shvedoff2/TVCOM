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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TVCOM.Model;
using TVCOM.ViewModel;

namespace TVCOM.View
{
    /// <summary>
    /// Логика взаимодействия для Control.xaml
    /// </summary>
    public partial class Control : UserControl
    {
        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(Control), new PropertyMetadata(null, OnFileNameChanged));

        public string FileName
        {
            get { return FileNameLabel.Content.ToString(); }
            set { FileNameLabel.Content = value; }
        }
        private static void OnFileNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Control control = d as Control;
            string fileName = System.IO.Path.GetFileName(e.NewValue as string);
            control.FileNameLabel.Content = fileName;
        }
        public Control()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(new LoginService(), new RegistService(), new ProverkaService(), new InsertService(), new ReportService());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=Тивиком;Integrated Security=True"))
            {
                conn.Open();
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Формы", conn))
                {
                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt);
                    ComboForms.ItemsSource = dt.DefaultView;
                    ComboForms.DisplayMemberPath = "Название";
                    ComboForms.SelectedValuePath = "ID_Формы";
                }
            }
        }
    }

}
