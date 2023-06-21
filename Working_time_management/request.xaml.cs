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
    /// Interaktionslogik für request.xaml
    /// </summary>
    public partial class request : Page
    {
        public request()
        {
            InitializeComponent();
        }

        public request(int i)
        {
            InitializeComponent();
            ListBoxItem newRequest = new ListBoxItem();
            newRequest.Content = "Urlaub            30.06.2023 - 14.07.2023             Status: offen";
            newRequest.MinHeight = 30;
            allRequest.Items.Add(newRequest);
        }

        private void clickNewRequest(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new newRequest());
        }
    }
}
