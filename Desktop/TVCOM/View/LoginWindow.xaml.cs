﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TVCOM.Model;
using TVCOM.ViewModel;

namespace TVCOM.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(new LoginService(), new RegistService(), new ProverkaService(), new InsertService(), new ReportService(), new ReportAllService(), new ReportDoljService());
        }
    }
}
