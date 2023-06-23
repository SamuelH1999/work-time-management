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
            
            
        }

        private void btnCaG(object sender, RoutedEventArgs e)
        {
            if (!isActive)
            {
                lblStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("Green");
                lblStatus.Content = "Angemeldet";
                isActive = true;
            }
            else
            {
                lblStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("Red");
                lblStatus.Content = "Abgemeldet";
                isActive = false;
            }
            
        }

        private void btnBackTDEvt(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Startpage());
        }
    }
}
