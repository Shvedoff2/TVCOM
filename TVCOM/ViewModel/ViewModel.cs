using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows;
using TVCOM.View;
using TVCOM.Model;
using System.Windows.Controls;
using Control = TVCOM.View.Control;
using System.Data.SqlTypes;

namespace TVCOM.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Control> Controls { get; set; }
        public static int ID_author { get; set; }
        private ILoginService _loginService;
        public IProverkaService _proverkaService;
        public IRegistService _registService;
        public IInsertService _insertService;
        public IReportService _reportService;
        public IRegistService _AddUserService;
        public ICommand LoginCommand { get; set; }
        public ICommand ReportOpenCommand { get; set; }

        public string ErrorMessage { get; set; }
        public ICommand OpenRegisterCommand { get; set; }
        public ICommand RegistCommand { get; set; }
        public ICommand ReportUserCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand MinimizeCommand { get; set; } 
        public ICommand InsertCommand {  get; set; }
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        private string _userNameR;
        public string UserNameR
        {
            get { return _userNameR; }
            set
            {
                _userNameR = value;
                OnPropertyChanged("UserNameR");
            }
        }

        private string _passwordR;
        public string PasswordR
        {
            get { return _passwordR; }
            set
            {
                _passwordR = value;
                OnPropertyChanged("PasswordR");
            }
        }
        public string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string _loginRed;
        public string LoginRed
        {
            get { return _loginRed; }
            set
            {
                _loginRed = value;
                OnPropertyChanged("Login");
            }
        }
        public string _passwordRed;
        public string PasswordRed
        {
            get { return _passwordRed; }
            set
            {
                _passwordRed = value;
                OnPropertyChanged("Password");
            }
        }
        public string _nameRed;
        public string NameRed
        {
            get { return _nameRed; }
            set
            {
                _nameRed = value;
                OnPropertyChanged("Name");
            }
        }
        private int _year;
        public int Year
        {
            get { return _year; }
            set
            {
                _year = value;
                OnPropertyChanged("Year");
            }
        }
        private int _startyear;
        public int StartYear
        {
            get { return _startyear; }
            set
            {
                _startyear = value;
                OnPropertyChanged("StartYear");
            }
        }
        private int _finyear;
        public int FinYear
        {
            get { return _finyear; }
            set
            {
                _finyear = value;
                OnPropertyChanged("FinYear");
            }
        }

        private int _selectedMonth;
        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
            }
        }
        private int _selectedStartMonth;
        public int SelectedStartMonth
        {
            get { return _selectedStartMonth; }
            set
            {
                _selectedStartMonth = value;
            }
        }
        private int _selectedFinMonth;
        public int SelectedFinMonth
        {
            get { return _selectedFinMonth; }
            set
            {
                _selectedFinMonth = value;
            }
        }

        private int _day;
        public int Day
        {
            get { return _day; }
            set
            {
                _day = value;
                OnPropertyChanged("Day");
            }
        }
        private int _startday;
        public int StartDay
        {
            get { return _startday; }
            set
            {
                _startday = value;
                OnPropertyChanged("StartDay");
            }
        }
        private int _finday;
        public int FinDay
        {
            get { return _finday; }
            set
            {
                _finday = value;
                OnPropertyChanged("FinDay");
            }
        }
        public MainWindowViewModel(ILoginService loginService, IRegistService registService, IProverkaService proverkaService, IInsertService insertService, IReportService reportService)
        {
            Controls = new ObservableCollection<Control>();
            _loginService = loginService;
            _proverkaService = proverkaService;
            _registService = registService;
            _insertService = insertService;
            _reportService = reportService;
            LoginCommand = new RelayCommand(LoginUser);
            OpenRegisterCommand = new RelayCommand(OpenRegister);
            RegistCommand = new RelayCommand(Register);
            CloseCommand = new RelayCommand(CloseW);
            MinimizeCommand = new RelayCommand(Minimize);
            InsertCommand = new RelayCommand(Insert);
            ReportUserCommand = new RelayCommand(ReportUser);
            ReportOpenCommand = new RelayCommand(OpenReportUser);
        }
        public void LoginUser(object obj)
        {
            int? employeeId = _loginService.ValidateUser(UserName, Password);
            if (employeeId != null)
            {
                ErrorMessage = "";
                OnPropertyChanged("ErrorMessage");
                MainWindowViewModel.ID_author = employeeId.Value;
                MessageBox.Show($"Пароль верный");
                MainWindow mainWindow = new MainWindow();
                MainWindowViewModel viewModel = new MainWindowViewModel(new LoginService(), new RegistService(), new ProverkaService(), new InsertService(), new ReportService());
                viewModel.Controls = this.Controls;
                mainWindow.Show();
                Application.Current.Windows[0].Close();
            }
            else
            {
                ErrorMessage = "Неправильный логин или пароль";
                OnPropertyChanged("ErrorMessage");
            }
        }

        private void OpenRegister(object obj)
        {
            Register regist = new Register();
            regist.Show();
        }
        private void Register(object obj)
        {
            if (UserNameR == null || PasswordR == null || Name == null)
            {
                MessageBox.Show("Введите данные");
                return;
            }
            if (_proverkaService.ValidateUser(UserNameR))
            {
                MessageBox.Show("Такой пользователь уже существует");
            }
            else
            {
                _registService.RegistUser(UserNameR, PasswordR, Name);
                MessageBox.Show("Регистрация успешна!");
                Login login = new Login();
                login.Show();
                Application.Current.Windows[0].Close();
            }
        }
        private void CloseW(object obj)
        {
            Application.Current.Windows[0].Close();
        }
        private void Minimize(object obj)
        {
            Application.Current.Windows[0].WindowState = WindowState.Minimized;
        }
        private void Insert(object obj) 
        {
            foreach (Control control in Controls)
            {
                int min = 1, hud = 1, ad = 1, cost = 1;
                if ((bool)control.min.IsChecked)
                    min = 2;
                if ((bool)control.hud.IsChecked)
                    hud = 2;
                if ((bool)control.ad.IsChecked)
                    ad = 2;
                if ((bool)control.cost.IsChecked)
                    cost = 2;
                string name = control.FileNameLabel.Content.ToString();
                int form = (int)control.ComboForms.SelectedValue;
                if ((bool)control.good.IsChecked)
                {
                    name += " (хор)";
                }
                if ((bool)control.num.IsChecked)
                {
                    name += " (числ)";
                }
                DateTime Date = new DateTime(Year, _selectedMonth, Day);
                _insertService.InsertUser(name, Date, form, MainWindowViewModel.ID_author, min, hud, ad, cost);
            }
            MessageBox.Show("Ввод данных прошёл успешно!");
        }
        private void OpenReportUser(object obj)
        {
            ReportUserWindow reportUserWindow = new ReportUserWindow();
            reportUserWindow.Show();
            Application.Current.Windows[0].Close();
        }
        private void ReportUser(object obj) 
        {
            DateTime start = new DateTime(StartYear, _selectedStartMonth, FinDay);
            DateTime end = new DateTime(FinYear, _selectedFinMonth, FinDay);
            _reportService.ReportUser(ID_author, start, end);
            MessageBox.Show("Данные сохранены!");
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.Windows[0].Close();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
