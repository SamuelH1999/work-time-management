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
    /// Interaktionslogik für request.xaml
    /// </summary>
    public partial class request : Page
    {
        private string userID;
        private string CSVpath;
        public request(string id)
        {
            InitializeComponent();
            this.userID = id;
            this.CSVpath = ProcessingCSV.getUserRequestPath(userID);
            foreach (string line in File.ReadLines(CSVpath))
            {
                string[] data = line.Split(';');
                string header = data[0];
                if (data[0] != "Abwesenheitsgrund")
                {
                    string[] requestInformation = line.Split(";");
                    ListBoxItem newRequest = new ListBoxItem();
                    newRequest.Content = requestInformation[0] + ", " + requestInformation[1] + " - " + requestInformation[2] + ", " + requestInformation[3];
                    allRequest.Items.Add(newRequest);
                }
            }
        }

        private void clickNewRequest(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new newRequest(userID));
        }

        private void clickDeleteRequest(object sender, RoutedEventArgs e)
        {
            ListBoxItem deleteItem = allRequest.SelectedItem as ListBoxItem;
            if(deleteItem != null )
            {
                string[] details = deleteItem.Content.ToString().Split(", ");
                string reason = details[0];
                string[] timeSpan = details[1].Split(" - ");
                string from = timeSpan[0];
                string until = timeSpan[1];
                string status = details[2];
                if(status != "genehmigt")
                {
                    bool wasFound = false;
                    string[] requestCSV = File.ReadAllLines(CSVpath);
                    string[] newCSVstring = new string[requestCSV.Length-1];
                    for( int i = 0; i < requestCSV.Length; i++)
                    {
                        string[] requestInformation = requestCSV[i].Split(";");
                        if (!wasFound && requestInformation[0] == reason && requestInformation[1] == from && requestInformation[2] == until && requestInformation[3] == status)
                        {
                            wasFound = true;
                        }
                        else
                        {
                            if(!wasFound) {
                                if(i < newCSVstring.Length)
                                {
                                    newCSVstring[i] = requestCSV[i];
                                }
                            }
                            else 
                            {
                                newCSVstring[i-1] = requestCSV[i];
                            }
                        }
                    }
                    if (wasFound)
                    {
                        allRequest.Items.Remove(deleteItem);
                        File.WriteAllLines(CSVpath, newCSVstring);
                    }
                }
                else
                {
                    MessageBox.Show("Antrag bereits bewilligt!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                }
            }
            
        }
    }
}
