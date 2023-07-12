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
using System.IO;
using System.Windows.Markup;
using System.Reflection;

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
            string userID = tbId.Text;
            string userPWD = tbPwd.Password;
            int inputCorrect = ProcessingCSV.checkLogIn(userID, userPWD, isLogIn);
            switch (inputCorrect)
            {
                case (int)ProcessingCSV.LogInResult.IDNotFound:
                    MessageBox.Show("ID nicht gefunden!", "Anmeldefehler", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    tbId.Text = "";
                    tbPwd.Password = "";
                    break;
                case (int)ProcessingCSV.LogInResult.PwdIncorrect:
                    MessageBox.Show("Passwort nicht korrekt!", "Anmeldefehler", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    tbPwd.Password = "";
                    break;
                case (int)ProcessingCSV.LogInResult.UserCorrect:
                    this.NavigationService.Navigate(new menu(userID));
                    break;
                case (int)ProcessingCSV.LogInResult.AdminCorrect:
                    this.NavigationService.Navigate(new menuAdmin());
                    break;
                case (int)ProcessingCSV.LogInResult.TimeDetectionIdFound:
                    this.NavigationService.Navigate(new TimeDetection(userID));
                    break;
                case (int)ProcessingCSV.LogInResult.TimeDetectionIdNotFound:
                    MessageBox.Show("ID nicht gefunden!", "Anmeldefehler", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    tbId.Text = "";
                    tbPwd.Password = "";
                    break;
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
