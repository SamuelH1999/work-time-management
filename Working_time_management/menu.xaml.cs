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
    public partial class menu : Page
    {
        private string userID;

        public menu(string id)      //ID des angemeldeten Users wird übergeben
        {
            InitializeComponent();
            this.userID = id;
        }

        private void LogOut(object sender, RoutedEventArgs e)       //Nachfolgend wird nur zu den einzelnen Seiten navigiert
        {
            this.NavigationService.Navigate(new Startpage());
        }

        private void ClickWorkerInformation(object sender, RoutedEventArgs e)
        {
            contentFrameMenu.Content = new workerInformation(userID);
        }

        private void ClickHoliday(object sender, RoutedEventArgs e)
        {
            contentFrameMenu.Content = new Holidays(userID);
        }

        private void clickRequest(object sender, RoutedEventArgs e)
        {
            contentFrameMenu.Content = new request(userID);
        }
    }
}
