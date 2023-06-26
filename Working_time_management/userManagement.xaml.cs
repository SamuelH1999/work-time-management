using System;
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
        public userManagement()
        {
            InitializeComponent();
        }

        public userManagement(string addName)
        {
            InitializeComponent();
            ListBoxItem newUser = new ListBoxItem();
            newUser.Content = addName;
            userList.Items.Add(newUser);
        }

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
        private void editUserClick(object sender, RoutedEventArgs e)
        {
            if (userList.SelectedItem != null)
            {
                ListBoxItem editUser = userList.SelectedItem as ListBoxItem;
                string[] names = editUser.Content.ToString().Split();
                this.NavigationService.Navigate(new editUser(names));
            }
        }
    }
}
