using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Interaktionslogik für TimeDetection.xaml
    /// </summary>
    public partial class TimeDetection : Page
    {
        private bool isActive = false;
        private string id;
        public TimeDetection(string id_str)
        {
            InitializeComponent();
            this.id = id_str;
            string workerInformation = ProcessingCSV.GetWorkerInformation(id);
            string[] workerInformationSplit = workerInformation.Split(';');
            string status = workerInformationSplit[4];
            if (status == "Angemeldet")
            {
                isActive = true;
                lblStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("Green");
                lblStatus.Content = "Angemeldet";
            }
            else
            {
                isActive = false;
                lblStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("Red");
                lblStatus.Content = "Abgemeldet";
            }
        }

        private void btnCaG(object sender, RoutedEventArgs e)
        {
            if (!isActive)
            {
                lblStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("Green");
                lblStatus.Content = "Angemeldet";
                string workerInformation = ProcessingCSV.GetWorkerInformation(id);
                string[] workerInformationSplit = workerInformation.Split(';');
                string lastName = workerInformationSplit[0];
                string firstName = workerInformationSplit[1];
                string dateOfBirth = workerInformationSplit[2];
                string residence = workerInformationSplit[3];
                string status = "Angemeldet";
                ProcessingCSV.editUserToWorkerInformationCSV(id, lastName, firstName, dateOfBirth, residence, status);
                isActive = true;
            }
            else
            {
                lblStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("Red");
                lblStatus.Content = "Abgemeldet";
                string workerInformation = ProcessingCSV.GetWorkerInformation(id);
                string[] workerInformationSplit = workerInformation.Split(';');
                string lastName = workerInformationSplit[0];
                string firstName = workerInformationSplit[1];
                string dateOfBirth = workerInformationSplit[2];
                string residence = workerInformationSplit[3];
                string status = "Abgemeldet";
                ProcessingCSV.editUserToWorkerInformationCSV(id, lastName, firstName, dateOfBirth, residence, status);
                isActive = false;
            }
            
        }

        private void btnBackTDEvt(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Startpage());
        }
    }
}
