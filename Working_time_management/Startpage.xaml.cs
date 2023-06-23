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

        enum LogInResult
        {
            IDNotFound,
            PwdIncorrect,
            UserCorrect,
            AdminCorrect
        }

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
        {       // This will get the current WORKING directory (i.e. \bin\Debug)
                string workingDirectory = Environment.CurrentDirectory;
                // or: Directory.GetCurrentDirectory() gives the same result
                // This will get the current PROJECT directory
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
                string[] pwdIdCSV = File.ReadAllLines(projectDirectory + @"\data\id_pwd\id_pwd.csv");
                string userID = tbId.Text;
            // Anmeldung für Menü
            if (isLogIn)
            {
                LogInResult inputCorrect = LogInResult.IDNotFound;
                foreach (string line in pwdIdCSV)
                {
                    string[] data = line.Split(';');
                    string ID = data[0];
                    string pwd = data[1];
                    if (ID == userID)
                    {
                        if (string.Compare(pwd, tbPwd.Text) == 0)
                        {
                            if (string.Compare(userID, "123123") == 0)
                            {
                                inputCorrect = LogInResult.AdminCorrect;
                                break;
                            }
                            else
                            {
                                inputCorrect = LogInResult.UserCorrect;
                                break;
                            }
                            
                        }
                        else
                        {
                            inputCorrect = LogInResult.PwdIncorrect;
                            break;
                        }
                    }
                }
                switch(inputCorrect)
                {
                    case LogInResult.IDNotFound:
                        MessageBox.Show("ID nicht gefunden!", "Anmeldefehler", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                        tbId.Text = "";
                        tbPwd.Text = "";
                        break;
                    case LogInResult.PwdIncorrect:
                        MessageBox.Show("Passwort nicht korrekt!", "Anmeldefehler", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                        tbPwd.Text = "";
                        break;
                    case LogInResult.UserCorrect:
                        this.NavigationService.Navigate(new menu(userID));
                        break;
                    case LogInResult.AdminCorrect:
                        this.NavigationService.Navigate(new menuAdmin());
                        break;
                }
            }
            // Anmeldung für Zeiterfassung
            else 
            {   
                bool idFound = false;
                foreach (string line in pwdIdCSV)
                {
                    string[] data = line.Split(';');
                    string ID = data[0];
                    if (ID == userID)
                    {
                        idFound = true;
                        break;
                    }
                }
                if (idFound)
                {
                    this.NavigationService.Navigate(new TimeDetection());
                }
                else 
                {
                    MessageBox.Show("ID nicht gefunden!", "Anmeldefehler", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    tbId.Text = "";
                }
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
