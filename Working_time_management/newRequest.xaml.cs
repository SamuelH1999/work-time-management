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
    /// Interaktionslogik für newRequest.xaml
    /// </summary>
    public partial class newRequest : Page
    {
        public newRequest()
        {
            InitializeComponent();
        }
        public bool holidayChecked;

        private void clickHolReqVerify(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new request(0));

        }

        private void checkedHoliday(object sender, RoutedEventArgs e)
        {
           ParameterStore.holidayChecked = true;
        }
    }
}
