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
            string fullName = tbFirstName.Text + " " + tbLastName.Text;
            this.NavigationService.Navigate(new userManagement(fullName));
        }
    }
}
