using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaktionslogik für settings.xaml
    /// </summary>
    public partial class settings : Page
    {
        private bool initialCheck = true;
        public settings()
        {
            InitializeComponent();

            tbLogoPath.Text = IniHandler.Read("Path", "Logo");      // Logo Pfad einlesen

            if (IniHandler.KeyExists("Rounding", "Time"))           // Rundung einlesen, falls vorhanden
            {
                string keyValue = IniHandler.Read("Rounding", "Time");
                for (int i = 0; i < selectRounding.Items.Count; i++)
                {
                    ComboBoxItem item = (ComboBoxItem)selectRounding.Items[i];
                    if (item != null)
                    {
                        if (string.Compare(item.Content.ToString(), keyValue) == 0)
                        {
                            selectRounding.SelectedItem = item;                         // richtiges UI-Element auswählen, dass zu key passt
                            break;
                        }
                    }
                }
            }

            if (IniHandler.KeyExists("AfterHours", "Breaks"))       // Pause nach Stunden einlesen, falls vorhanden
            {
                string keyValue = IniHandler.Read("AfterHours", "Breaks");
                radbtnDuration.IsChecked = true;
                for (int i = 0; i < selectDuration.Items.Count; i++)
                {
                    ComboBoxItem item = (ComboBoxItem)selectDuration.Items[i];
                    if (item != null)
                    {
                        if (string.Compare(item.Content.ToString(), keyValue) == 0)
                        {
                            selectDuration.SelectedItem = item;                         // richtiges UI-Element auswählen, dass zu key passt
                            break;
                        }
                    }
                }
            }

            if (IniHandler.KeyExists("Fix", "Breaks"))          // Pause zu fester Uhrzeit einlesen, falls vorhanden
            {
                string keyValue = IniHandler.Read("Fix", "Breaks");
                radbtnTime.IsChecked = true;
                for (int i = 0; i < selectTime.Items.Count; i++)
                {
                    ComboBoxItem item = (ComboBoxItem)selectTime.Items[i];
                    if (item != null)
                    {
                        if (string.Compare(item.Content.ToString(), keyValue) == 0)
                        {
                            selectTime.SelectedItem = item;                         // richtiges UI-Element auswählen, dass zu key passt
                            break;
                        }
                    }
                }
            }
        }
        private void selectDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)        // Änderungen der ComboBox verarbeiten
        {
            ComboBoxItem selected = selectDuration.SelectedItem as ComboBoxItem;
            if (selectDuration.SelectedItem != null)
            {
                IniHandler.Write("AfterHours", selected.Content.ToString(), "Breaks");                  // Ausgewählten Wert in Ini-File schreiben
            }    
        }

        private void selectTime_SelectionChanged(object sender, SelectionChangedEventArgs e)            // Änderungen der ComboBox verarbeiten
        {
            ComboBoxItem selected = selectTime.SelectedItem as ComboBoxItem;
            if(selectTime.SelectedItem != null)
            {
                IniHandler.Write("Fix", selected.Content.ToString(), "Breaks");                         // Ausgewählten Wert in Ini-File schreiben
            }
        }

        private void selectRounding_SelectionChanged(object sender, SelectionChangedEventArgs e)        // Änderungen der ComboBox verarbeiten
        {
            ComboBoxItem selected = selectRounding.SelectedItem as ComboBoxItem;
            if (selected != null)
            {
                IniHandler.Write("Rounding", selected.Content.ToString(), "Time");                      // Ausgewählten Wert in Ini-File schreiben
            }
        }

        private void btnPicPath_Click(object sender, RoutedEventArgs e)     // Logo-Pfad ändern
        {
            string newLogoPath = tbLogoPath.Text;
            IniHandler.Write("Path", newLogoPath, "Logo");                  // Ausgewählten Wert in Ini-File schreiben
        }

        private void btnNewPwd_Click(object sender, RoutedEventArgs e)      // Passwort ändern
        {
            this.NavigationService.Navigate(new changeAdminPwd());          // Navigiere zu Page
        }

        private void radbtnDuration_Checked(object sender, RoutedEventArgs e)       // Änderungen der RadioButtons verarbeiten
        {
            selectDuration.IsEnabled = true;
            selectTime.IsEnabled = false;
            selectTime.SelectedItem = null;
            if (initialCheck == false)          // fängt unerwünschten Funktionsaufruf ab, der passiert wenn Seite neu geladen wird
            {
                ComboBoxItem selected = (ComboBoxItem)selectDuration.Items[2];
                selectDuration.SelectedItem = selected;         // Default-Item auswählen --> triggert Selection_Canged Funktion
            }
            if (IniHandler.KeyExists("Fix", "Breaks"))
            {
                IniHandler.DeleteKey("Fix", "Breaks");          // Key des anderen Modus löschen, falls vorhanden
            }
            initialCheck = false;
        }

        private void radbtnTime_Checked(object sender, RoutedEventArgs e)
        {
            selectDuration.IsEnabled = false;   // fängt unerwünschten Funktionsaufruf ab, der passiert wenn Seite neu geladen wird
            selectTime.IsEnabled = true;
            selectDuration.SelectedItem = null;
            if (initialCheck == false)
            {
                ComboBoxItem selected = (ComboBoxItem)selectTime.Items[4];
                selectTime.SelectedItem = selected;             // Default-Item auswählen --> triggert Selection_Canged Funktion
            }
            if (IniHandler.KeyExists("AfterHours", "Breaks"))   // Key des anderen Modus löschen, falls vorhanden
            {
                IniHandler.DeleteKey("AfterHours", "Breaks");
            }
            initialCheck = false;
        }
    }
}
