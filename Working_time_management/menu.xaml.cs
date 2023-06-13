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
    /// Interaktionslogik für menu.xaml
    /// </summary>
    public partial class menu : Page
    {
        public menu()
        {
            InitializeComponent();
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Startpage());
        }

        private void ClickWorkerInformation(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new workerInformation());
        }

        private void ClickHoliday(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Holidays());
        }

        private void ClickOvertime(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new overtime());
        }

        private void clickRequest(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new request());
        }
    }
}
