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
            foreach (string line in File.ReadLines(CSVpath))        // Alle existiernden Anträge des Users auslesen
            {
                string[] data = line.Split(';');                    // CSV-Spalten splitten
                if (data[0] != "Abwesenheitsgrund")                 // Überschrift-Zeile ignorieren
                {
                    string[] requestInformation = line.Split(";");
                    ListBoxItem newRequest = new ListBoxItem();
                    newRequest.Content = requestInformation[0] + ", " + requestInformation[1] + " - " + requestInformation[2] + ", " + requestInformation[3];
                    allRequest.Items.Add(newRequest);               // Alle Anträge auf der Oberfläche anzeigen

                }
            }
        }

        private void clickNewRequest(object sender, RoutedEventArgs e)      // Seite für neuen Antrag laden
        {
            this.NavigationService.Navigate(new newRequest(userID));
        }

        private void clickDeleteRequest(object sender, RoutedEventArgs e)   // Löscht ausgewählte Anträge
        {
            ListBoxItem deleteItem = allRequest.SelectedItem as ListBoxItem;    // Checkt, ob ein Element in der Liste ausgewählt wurde
            if(deleteItem != null )
            {
                string[] details = deleteItem.Content.ToString().Split(", ");   // Content des Eintrags in Bestandteile splitten und Informationen speichern
                string reason = details[0];
                string[] timeSpan = details[1].Split(" - ");
                string from = timeSpan[0];
                string until = timeSpan[1];
                string status = details[2];
                if(status != "genehmigt")                                       // Nur offene oder abgelehnte Anträge können aus der Übersicht entfernt werden
                {
                    bool wasFound = false;
                    string[] requestCSV = File.ReadAllLines(CSVpath);           // Alle Anträge einlesen
                    string[] newCSVstring = new string[requestCSV.Length-1];    // Array für neue Liste ohne den gelöschten Antrag (1 Entrag weniger --> Length - 1)
                    for( int i = 0; i < requestCSV.Length; i++)
                    {
                        string[] requestInformation = requestCSV[i].Split(";");
                        if (!wasFound && requestInformation[0] == reason && requestInformation[1] == from && requestInformation[2] == until && requestInformation[3] == status)     // Informationen der CSV mit den gespeicherten Informationen abgleichen
                        {
                            wasFound = true;                                    // Flag wenn Eintrag gefunden und kein Eintrag in newString[i]
                        }
                        else
                        {
                            if(!wasFound) {                                     // Solange Eintrag nicht gefunden: Zeilen kopieren
                                if(i < newCSVstring.Length)                     // Fehler vermeiden falls bis zur letzten Zeile kein Eintrag gefunden wird
                                {
                                    newCSVstring[i] = requestCSV[i];
                                }
                            }
                            else 
                            {
                                newCSVstring[i-1] = requestCSV[i];              // Nachdem Eintrag gefunden: Kopie nun verschoben, wegen gelöschter Zeile
                            }
                        }
                    }
                    if (wasFound)                                               // Wenn Durchlauf erfolgreich:
                    {
                        allRequest.Items.Remove(deleteItem);                    // Eintrag aus UI-Liste löschen
                        File.WriteAllLines(CSVpath, newCSVstring);              // CSV mit Kopie überschreiben
                    }
                }
                else
                {
                    MessageBox.Show("Antrag bereits bewilligt!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);      // Fehler, falls Antrag bewilligt
                }
            }
            
        }
    }
}
