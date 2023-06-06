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

namespace Working_time_management
{
    /// <summary>
    /// Interaktionslogik für menu.xaml
    /// </summary>
    public partial class menuAdmin : Page
    {
        public menuAdmin()
        {
            InitializeComponent();
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Startpage());
        }

        private void UserM(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new userManagement());
        }
    }
}
