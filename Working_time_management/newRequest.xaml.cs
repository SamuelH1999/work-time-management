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
    /// Interaktionslogik für newRequest.xaml
    /// </summary>
    public partial class newRequest : Page
    {
        private string userID;
        private string CSVpath;
        public newRequest(string id)
        {
            InitializeComponent();
            this.userID = id;
            this.CSVpath = ProcessingCSV.getUserRequestPath(userID);
        }

        private void clickHolReqVerify(object sender, RoutedEventArgs e)
        {
            string type = "";
            if (checkboxHoliday.IsChecked == true)
            {
                type = "Urlaub";
            }
            else if(checkboxOvertime.IsChecked == true) 
            {
                type = "Überstundenabbau";
            }
            else
            {
                return;
            }

            if(startDate.SelectedDate == null)
            {
                return;
            }

            if (endDate.SelectedDate == null)
            {
                return;
            }
            string from = startDate.SelectedDate.Value.ToString("dd.MM.yyyy");
            string until = endDate.SelectedDate.Value.ToString("dd.MM.yyyy");
            string requestData = "\n" + type + ";" + from + ";" + until + ";" + "offen";
            File.AppendAllText(CSVpath, requestData);
            this.NavigationService.Navigate(new request(userID));
        }
   
    }
}
