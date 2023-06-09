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
using System.IO;

namespace Working_time_management
{
    /// <summary>
    /// Interaktionslogik für editUser.xaml
    /// </summary>
    public partial class editUser : Page
    {
        private List<Worker> list = new List<Worker>();
        private string ID;
        private string status;
        public editUser(string[] names)
        {
            InitializeComponent();
            tbLastName.Text = names[0];         //Der Name wird aus userManagement übergeben
            tbFirstName.Text = names[1];
            ID = names[2];
            string Pwd = getIDPwd(ID);      //Passwort wird aus CSV-Datei geholt
            tbPwd.Text = Pwd;
            string workerInformation = ProcessingCSV.GetWorkerInformation(ID);          //Die Informationen des Mitaarbeiter wird aus der CSV gehlt
            string[] workerInformationSplit = workerInformation.Split(";");
            tbDayOfBirth.Text = workerInformationSplit[2];
            tbResidence.Text = workerInformationSplit[3];
            status = workerInformationSplit[4];
        }

        private void confirmEditClick(object sender, RoutedEventArgs e)
        {
            ID = this.ID;
            if (tbFirstName.Text.Length > 0 && tbLastName.Text.Length > 0 && tbPwd.Text.Length > 0 && tbResidence.Text.Length > 0 && tbDayOfBirth.Text.Length >0 && ID != "ID") //Die Einträge dürfen nicht leer sein und die ID darf nicht "ID" sein
            {
                ProcessingCSV.editUserPwdToCSV(ID, tbPwd.Text);
                ProcessingCSV.editUserToWorkerInformationCSV(ID, tbLastName.Text, tbFirstName.Text, tbDayOfBirth.Text, tbResidence.Text, status);   // Einträge aktualisieren
                this.NavigationService.Navigate(new userManagement());
            }
            else
            {
                this.NavigationService.Navigate(new userManagement()); 
            }
        }
        private string getIDPwd(string ID)          //Passwort wird aus CSV-Datei geholt
        {
            string pwd = "";
            foreach (string line in File.ReadLines(ProcessingCSV.idPwdPath))
            {
                string[] data = line.Split(';');
                string id = data[0];
                if (ID == id)
                {
                    pwd = data[1];
                    break;
                }
                else
                {
                } 
            }
            return pwd;
        }
    }
}
