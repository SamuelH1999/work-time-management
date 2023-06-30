using System;
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
        private List<Worker> list;
        public userManagement()
        {
            InitializeComponent();
            fillListBox();
        }

        /*
        public userManagement(string addName)
        {
            InitializeComponent();
            ListBoxItem newUser = new ListBoxItem();
            newUser.Content = addName;
            userList.Items.Add(newUser);
        }
        */
        private void newUserClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new addUser());
        }

        private void deleteUserClick(object sender, RoutedEventArgs e)

        {
            if (userList.SelectedItem != null)
            {
                ListBoxItem deletedUser = userList.SelectedItem as ListBoxItem;
                MessageBoxResult mboxResult = MessageBox.Show("Möchten Sie " + deletedUser.Content.ToString() + " wirklich löschen?", "Nutzer löschen", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (mboxResult == MessageBoxResult.Yes)
                {
                    userList.Items.Remove(deletedUser);
                }
            }
        }
        private void editUserClick(object sender, RoutedEventArgs e)            //TODO Samuel
        {
            if (userList.SelectedItem != null)
            {
                ListBoxItem editUser = userList.SelectedItem as ListBoxItem;
                string[] names = editUser.Content.ToString().Split(',');
                this.NavigationService.Navigate(new editUser(names));
            }
        }

        private void fillListBox()
        {
            list = new List<Worker>();
            foreach (string line in File.ReadLines(@"..\..\..\data\id_pwd\id_pwd.csv"))
            {
                string[] data = line.Split(';');
                string Id = data[0];
                string Pwd = data[1];
                if (Id != "123123")
                {
                    string workerInformation = ProcessingCSV.GetWorkerInformation(Id);
                    string[] workerInformationSplit = workerInformation.Split(';');
                    string lastName = workerInformationSplit[0];
                    string firstName = workerInformationSplit[1];
                    string dateOfBirth = workerInformationSplit[2];
                    string residence = workerInformationSplit[3];  
                    list.Add(new Worker { LastName = lastName, FirstName = firstName, DateOfBirth = dateOfBirth, Residence = residence, Password = Pwd, ID = Id});
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
    }
}
