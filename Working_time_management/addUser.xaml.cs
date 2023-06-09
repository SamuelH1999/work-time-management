﻿ using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaktionslogik für addUser.xaml
    /// </summary>
    public partial class addUser : Page
    {
        public addUser()
        {
            InitializeComponent();
        }

        private void confirmClick(object sender, RoutedEventArgs e) 
        {
            if(tbFirstName.Text.Length > 0 && tbLastName.Text.Length > 0 && tbPwd.Text.Length > 0)  //Felder dürfen nicht leer bleiben
            {
                string pwd = tbPwd.Text;
                string id = tbID.Text;
                bool IDAlreadyExists = false;
                foreach (string line in File.ReadLines(ProcessingCSV.idPwdPath))
                {
                    string[] data = line.Split(';');
                    string ID = data[0];
                    string Password = data[1];   
                    if (id == ID)                    // Es wird überprüft, ob es diese ID schon gibt
                    {
                        IDAlreadyExists = true;
                    }
                }
                if (IDAlreadyExists == false)               //Neuer Mitarbeiter wird angelegt
                {
                    string[] pwdID = { id + ";" + pwd };
                    string firstName = tbFirstName.Text;
                    string lastName = tbLastName.Text;
                    string dateOfBirth = tbDateOfBirth.Text;
                    string residence = tbResidence.Text;
                    string fullName = firstName + " " + lastName;
                    ProcessingCSV.addUserToID_PWDCSV(pwdID);
                    newFolder(id);
                    ProcessingCSV.addUserToWorkerInformationCSV(id, lastName, firstName, dateOfBirth, residence);
                    ProcessingCSV.addWorkingTimeCSV(id);
                    this.NavigationService.Navigate(new userManagement());              //Bei erfolgreicher Eingabe gelangt man wieder zurück in das userManagement
                }
                else
                {
                    MessageBox.Show("ID existiert bereits!", "Eingabefehler", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                }
            }
            else
            {
                MessageBox.Show("Eingaben fehlerhaft!", "Eingabefehler", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                this.NavigationService.Navigate(new userManagement());
            }
            
        }
        private void newFolder(string newID)
        {
            String sPath = @"..\..\..\data\worker_information\" + newID; // newID ist die neue ID des Mitarbeiters welcher angelegt werden soll
            if (Directory.Exists(sPath) == false)
            {
                Directory.CreateDirectory(sPath);
            }
        }

    }
}
