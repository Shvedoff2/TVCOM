using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.IO;
using System.Windows.Media.Animation;

namespace TVCOM.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Control> Controls { get; set; } = new ObservableCollection<Control>();

        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel viewModel = new MainWindowViewModel(new LoginService(), new RegistService(), new ProverkaService(), new InsertService(), new ReportService(), new ReportAllService(), new ReportDoljService());
            viewModel.Controls = this.Controls;
            this.DataContext = new MainWindowViewModel(new LoginService(), new RegistService(), new ProverkaService(), new InsertService(), new ReportService(), new ReportAllService(), new ReportDoljService());
        }
        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var viewModel = this.DataContext as MainWindowViewModel;
                viewModel.Controls.Clear();

                foreach (var file in files)
                {
                    Control control = new Control();
                    control.FileNameLabel.Content = System.IO.Path.GetFileName(file);
                    viewModel.Controls.Add(control);
                }
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ComboBoxItem)Month.SelectedItem;
            int selectedMonth = int.Parse((string)selectedItem.Tag);

            // Определяем количество дней в выбранном месяце
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, selectedMonth);

            // Очищаем комбобокс
            Day.Items.Clear();

            // Заполняем первый комбобокс числами от 1 до daysInMonth
            for (int i = 1; i <= daysInMonth; i++)
            {
                Day.Items.Add(i);
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Menu.Width == 40) 
            {
                DoubleAnimation MenuAnimation = new DoubleAnimation();
                MenuAnimation.From = Menu.Width;
                MenuAnimation.From = 40;
                MenuAnimation.To = 300;
                MenuAnimation.Duration = TimeSpan.FromSeconds(0.5);
                MenuAnimation.EasingFunction = new CubicEase();
                Menu.BeginAnimation(WidthProperty, MenuAnimation);
                DoubleAnimation SVAnimation = new DoubleAnimation();
                SVAnimation.From = SV.Width;
                SVAnimation.From = 1025;
                SVAnimation.To = 750;
                SVAnimation.Duration = TimeSpan.FromSeconds(0.5);
                SVAnimation.EasingFunction = new CubicEase();
                SV.BeginAnimation(WidthProperty, SVAnimation);
            }
            if (Menu.Width == 300)
            {
                DoubleAnimation borderAnimation = new DoubleAnimation();
                borderAnimation.From = Menu.Width;
                borderAnimation.From = 300;
                borderAnimation.To = 40;
                borderAnimation.Duration = TimeSpan.FromSeconds(0.5);
                borderAnimation.EasingFunction = new CubicEase();
                Menu.BeginAnimation(WidthProperty, borderAnimation);
                DoubleAnimation SVAnimation = new DoubleAnimation();
                SVAnimation.From = SV.Width;
                SVAnimation.From = 750;
                SVAnimation.To = 1025;
                SVAnimation.Duration = TimeSpan.FromSeconds(0.5);
                SVAnimation.EasingFunction = new CubicEase();
                SV.BeginAnimation(WidthProperty, SVAnimation);
            }
        }
    }
}
