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
    /// Interaktionslogik für editUser.xaml
    /// </summary>
    public partial class editUser : Page
    {
        public editUser(string[] names)
        {
            InitializeComponent();
            tbFirstName.Text = names[0];
            tbLastName.Text = names[1];
        }

        private void confirmEditClick(object sender, RoutedEventArgs e)
        {
            if (tbFirstName.Text.Length > 0 || tbLastName.Text.Length > 0)
            {
                string fullName = tbFirstName.Text + " " + tbLastName.Text;
                this.NavigationService.Navigate(new userManagement(fullName));
            }
            else
            {
                this.NavigationService.Navigate(new userManagement());
            }
        }
    }
}
