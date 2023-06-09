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
    /// Interaktionslogik für workerInformation.xaml
    /// </summary>
    public partial class workerInformation : Page
    {
        private string userID;
        public workerInformation(string id)
        {
            InitializeComponent();
            this.userID = id;
            if (userID != "123123") {
                string[] information = ProcessingCSV.GetWorkerInformation(userID).Split(';');       // Infos auslesen
                lblUserName.Content = information[1] + " " + information[0];                        // UI entsprechend anpassen
                lblUserDoB.Content = information[2];
                lblResidence.Content = information[3];
                lblStatus.Content = information[4];
            }

        }
    }
}
