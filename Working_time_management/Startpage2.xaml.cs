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
    /// Interaktionslogik für Startpage2.xaml
    /// </summary>
    public partial class Startpage2 : Page
    {
        public Startpage2()
        {
            InitializeComponent();
        }
        private bool isLogIn = false;

        private void LogInEvent(object sender, RoutedEventArgs e)
        {
            isLogIn = true;
            lblAnm.Visibility = Visibility.Visible;
            lblPwd.Visibility = Visibility.Visible;
            tbPwd.Visibility = Visibility.Visible;
            Grid.SetRow(btnLogIn, 6);
            Anmeldung.FontSize = 18;
            Anmeldung.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFAFAFAF");
            btnTimeDtc.FontSize = 15;
            btnTimeDtc.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFDDDDDD");
        }

        private void btnLogInSuc(object sender, RoutedEventArgs e)
        {
            if (isLogIn)
            {
                string userPwd1 = "123456";
                string adminPwd = "654321";
                int userID;
                if (tbId.Text.Length < 1)
                {
                    userID = 0;
                }
                else
                {

                    userID = int.Parse(tbId.Text);
                }

                if (string.Compare(userPwd1, tbPwd.Text) == 0)
                {
                    this.NavigationService.Navigate(new menu(userID));
                }
                else if (string.Compare(adminPwd, tbPwd.Text) == 0)
                {
                    this.NavigationService.Navigate(new menuAdmin());
                }
                else
                {

                }
            }
            else
            {
                this.NavigationService.Navigate(new TimeDetection(""));
            }
        }

        private void TimeDtc(object sender, RoutedEventArgs e)
        {
            isLogIn = false;
            lblAnm.Visibility = Visibility.Hidden;
            lblPwd.Visibility = Visibility.Hidden;
            tbPwd.Visibility = Visibility.Hidden;
            Grid.SetRow(btnLogIn, 4);
            Anmeldung.FontSize = 15;
            Anmeldung.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFDDDDDD");
            btnTimeDtc.FontSize = 18;
            btnTimeDtc.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFAFAFAF");
        }
    }
}
