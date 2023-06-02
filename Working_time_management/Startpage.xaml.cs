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
    /// Interaktionslogik für Startpage.xaml
    /// </summary>
    public partial class Startpage : Page
    {
        public Startpage()
        {
            InitializeComponent();
        }

        private void LogInEvent(object sender, RoutedEventArgs e)
        {
            LogInFrame.Content = new Log_in();
            btnLogIn.Visibility = Visibility.Visible;
        }

        private void btnLogInSuc(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new menu());
        }

        private void TimeDtc(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new ());
        }
    }
}
