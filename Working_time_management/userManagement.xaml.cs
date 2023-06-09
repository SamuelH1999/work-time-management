﻿using System;
using System.Collections;
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
    /// Interaktionslogik für userManagement.xaml
    /// </summary>
    public partial class userManagement : Page
    {
        private List<Worker> list = new List<Worker>();
        public userManagement()
        {
            InitializeComponent();
            fillListBox();      //Liste mit den Mitarbeitern befüllen
        }
        private void newUserClick(object sender, RoutedEventArgs e) //Neue Seite um neuen Nutzer anzulegen
        {
            this.NavigationService.Navigate(new addUser());
        }

        private void deleteUserClick(object sender, RoutedEventArgs e)  //Ausgewählter Benutzer wird gelöscht

        {
            if (userList.SelectedItem != null)                                          
            {                                                                         
                ListBoxItem deletedUser = userList.SelectedItem as ListBoxItem;
                string[] name = deletedUser.Content.ToString().Split(", ");
                string id = name[2];
                MessageBoxResult mboxResult = MessageBox.Show("Möchten Sie " + deletedUser.Content.ToString() + " wirklich löschen?", "Nutzer löschen", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (mboxResult == MessageBoxResult.Yes)
                {
                    userList.Items.Remove(deletedUser);
                    ProcessingCSV.deleteUserPwdInCSV(id);       //ID und Passwort wird aus CSV gelöscht       
                    moveFolder(id);                             //Ordener des Benutzers wird in den Ordner "archive" verschoben

                }
            }
        }
        private void editUserClick(object sender, RoutedEventArgs e)      //Neue Seite zum Bearbeiten des ausgewählten Mitarbeiters wird geöffnet      
        {
            if (userList.SelectedItem != null)
            {
                ListBoxItem editUser = userList.SelectedItem as ListBoxItem;
                string[] names = editUser.Content.ToString().Split(", ");
                this.NavigationService.Navigate(new editUser(names));
            }
        }

        private void fillListBox()  //Liste wird mit Mitarbeitern befüllt
        {
            foreach (string line in File.ReadLines(ProcessingCSV.idPwdPath))
            {
                string[] data = line.Split(';');
                string Id = data[0];
                string Pwd = data[1];
                if (Id != "123123" && Id != "ID")               //der Admin und die Überschrift dürfen nicht aufgeführt werden
                {
                    string workerInformation = ProcessingCSV.GetWorkerInformation(Id);
                    string[] workerInformationSplit = workerInformation.Split(';');
                    string lastName = workerInformationSplit[0];
                    string firstName = workerInformationSplit[1];
                    string dateOfBirth = workerInformationSplit[2];
                    string residence = workerInformationSplit[3];  
                    list.Add(new Worker { LastName = lastName, FirstName = firstName, DateOfBirth = dateOfBirth, Residence = residence, Password = Pwd, ID = Id });
                }
                else
                {

                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                ListBoxItem newWorker = new ListBoxItem();
                newWorker.Content = list[i].ToString(); 
                userList.Items.Add(newWorker);
            }
        }
        private void moveFolder(string ID)
        {
            string sPath = @"..\..\..\data\worker_information\" + ID;   //Quellpfad
            string dPath = @"..\..\..\data\archive\" + ID;              //Zielpfad
            string aPath = @"..\..\..\data\archive\";                   //Pfad zum Archiv-Ordner
            if (Directory.Exists(aPath) == false)                       //Erstellt einen Ordner für den Fall, dass noch keiner existiert
            {
                Directory.CreateDirectory(aPath);
            }

            if (Directory.Exists(sPath) == true)
            {
                Directory.Move(sPath, dPath);
            }
            else
            {
                MessageBoxResult mboxResult = MessageBox.Show("Dieser Benutzer wurde nicht gefunden");
            }
        }
    }
}
