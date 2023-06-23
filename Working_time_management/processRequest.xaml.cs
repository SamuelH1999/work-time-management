using System;
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
    /// Interaktionslogik für processRequest.xaml
    /// </summary>
    public partial class processRequest : Page
    {
        public processRequest()
        {
            InitializeComponent();
        }

        private void showDetails(object sender, RoutedEventArgs e)
        {
            if (openRequests.SelectedItem != null)
            {
                ListBoxItem details = openRequests.SelectedItem as ListBoxItem;
                string[] names = details.Content.ToString().Split();
                MessageBox.Show("Antragsteller: " + names[0] + " " + names[1] + "\nZeitraum: 30.06.2023 - 14.07.2023", "Antragsdetails", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }

        private void acceptClick(object sender, RoutedEventArgs e)
        {
            if (openRequests.SelectedItem != null)
            {
                ListBoxItem deletedUser = openRequests.SelectedItem as ListBoxItem;
                openRequests.Items.Remove(deletedUser);
                MessageBox.Show("Antrag akzeptiert!", "Antrag bearbeitet", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }

        private void rejectClick(object sender, RoutedEventArgs e)
        {
            if (openRequests.SelectedItem != null)
            {
                ListBoxItem deletedUser = openRequests.SelectedItem as ListBoxItem;
                openRequests.Items.Remove(deletedUser);
                MessageBox.Show("Antrag abgelehnt!", "Antrag bearbeitet", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }
    }
}
