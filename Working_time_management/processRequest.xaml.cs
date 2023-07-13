using Microsoft.VisualBasic;
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
    /// Interaktionslogik für processRequest.xaml
    /// </summary>
    public partial class processRequest : Page
    {
        public processRequest()
        {
            InitializeComponent();
            foreach (string line in File.ReadLines(ProcessingCSV.idPwdPath))        // alle existierenden IDs lesen
            {
                string[] data = line.Split(';');
                string id = data[0];
                if (id != "123123" && id != "ID")       // Überschrift und Admin-ID ignorieren
                {
                    string[] workerInfo = ProcessingCSV.GetWorkerInformation(id).Split(';');        // Zugehörige Infos auslesen
                    string[] requests = ProcessingCSV.getAllWorkerRequests(id);                     // Alle Anträge des Nutzers auflisten
                    foreach (string request in requests)    // alle Anträge auslesen                                         
                    {
                        string[] details = request.Split(';');
                        if(details.Length > 3)
                        {
                            string reason = details[0];
                            string from = details[1];
                            string until = details[2];
                            string status = details[3];

                            if (status == "offen")      // offene Anträge in der UI - Liste sichtbar machen
                            {
                                ListBoxItem requestItem = new ListBoxItem();
                                requestItem.MaxHeight = 30;
                                requestItem.Content = id + ", " + workerInfo[0] + ", " + workerInfo[1] + ", " + reason + ", " + from + " - " + until;
                                openRequests.Items.Add(requestItem);
                            }
                        }
                        
                    }
                }
            }         
        }

        private void acceptOrRecline(string request, bool decision)     // Verarbeitet Annahme oder Ablehnung des Admins
        {
            string[] details = request.Split(", ");                     // Informationen gewinnen
            string id = details[0];
            string reason = details[3];
            string[] timeSpan = details[4].Split(" - ");
            string from = timeSpan[0];
            string until = timeSpan[1];
            string newStatus; 
            

            string[] requests = ProcessingCSV.getAllWorkerRequests(id);     // Anträge des Nutzers auslesen
            for (int i = 0; i < requests.Length; i++)
            {
                string[] csvDetails = requests[i].Split(';');
                if (csvDetails[0] == reason && csvDetails[1] == from && csvDetails[2] == until)     // passenden Eintrag finden
                {
                    if (decision)       // Falls Antrag angenommen
                    {
                        newStatus = "genehmigt";    // neuen Status festlegen
                        string[] fromYMD = from.Split(".");
                        string[] untilYMD = until.Split(".");
                        int dayFrom = int.Parse(fromYMD[0]);
                        int monthFrom = int.Parse(fromYMD[1]);
                        int yearFrom = int.Parse(fromYMD[2]);
                        int dayUntil = int.Parse(untilYMD[0]);
                        int monthUntil = int.Parse(untilYMD[1]);
                        int yearUntil = int.Parse(untilYMD[2]);

                        DateTime fromDate = new DateTime(yearFrom, monthFrom, dayFrom);
                        DateTime untilDate = new DateTime(yearUntil, monthUntil, dayUntil);
                        int absenceDays = 0; 
                        for (DateTime dt = fromDate; dt <= untilDate; dt = dt.AddDays(1))   // Anzahl Tage zwischen From und Until, ausgenommen Wochenenden
                        {
                            if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
                            {
                                absenceDays++;      // Tage erhöhen, wenn Wochentag
                            }
                        }

                        if(reason == "Urlaub")
                        {
                            string[] workTimeInformation = File.ReadAllLines(ProcessingCSV.getWorkingTimeInformationPath(id));
                            string[] values = workTimeInformation[1].Split(";");
                            int currentHolidays = int.Parse(values[2]) - absenceDays;   // Von Urlaubstagen abziehen
                            workTimeInformation[1] = values[0] + ";" + values[1] + ";" + currentHolidays;
                            File.WriteAllLines(ProcessingCSV.getWorkingTimeInformationPath(id),workTimeInformation, Encoding.UTF8);     // Infos neu abspeichern
                        }
                        else if(reason == "Überstundenabbau")
                        {
                            string[] workTimeInformation = File.ReadAllLines(ProcessingCSV.getWorkingTimeInformationPath(id));
                            string[] values = workTimeInformation[1].Split(";");
                            string[] overtime = values[1].Split(":");
                            int overTimeHours = int.Parse(overtime[0]);
                            int overTimeMinutes = int.Parse(overtime[1]);
                            if (overtime[0].StartsWith('-'))
                            {
                                overTimeMinutes *= -1;  // Überstunden negativ? --> Minuten negieren für TimeSpan Konstruktor
                            }
                            TimeSpan overtimeSpan = new TimeSpan(overTimeHours, overTimeMinutes, 0).Subtract(new TimeSpan(8*absenceDays, 0, 0));    // Von Überstunden abziehen (8 Std pro Tag)
                            string[] newovertime = overtimeSpan.ToString().Split(':');
                            if (newovertime[0].Contains('.'))       // Wenn "overtime" die Zeit in Tagen und nicht mehr in Stunden gespeichert hat, wird diese hier wieder in Stunden umgewandelt
                            {
                                int fullHours = int.Parse(newovertime[0].Split('.')[0]) * 24;
                                if (newovertime[0].StartsWith('-'))
                                {
                                    fullHours -= int.Parse(newovertime[0].Split('.')[1]);
                                }
                                else
                                {
                                    fullHours += int.Parse(newovertime[0].Split('.')[1]);
                                }
                                newovertime[0] = fullHours.ToString();
                            }
                            values[1] = newovertime[0] + ":" + newovertime[1];
                            workTimeInformation[1] = values[0] + ";" + values[1] + ";" + values[2];
                            File.WriteAllLines(ProcessingCSV.getWorkingTimeInformationPath(id), workTimeInformation, Encoding.UTF8);    // Zeitinformationen aktualisieren
                        }
                        ProcessingCSV.addAbsenceToAbsenceCSV(id, from, until, reason);      // Abwesenheit eintragen 
                    }
                    else
                    {                               // Ablehnung bearbeiten    
                        newStatus = "abgelehnt";    // neuen Status festlegen
                    }
                    requests[i] = csvDetails[0] + ";" + csvDetails[1] + ";" + csvDetails[2] + ";" + newStatus;      // Enstprechende Zeile bearbeiten
                    break;      // richtigen Antrag gefunden und bearbeitet --> break!
                }
            }
            File.WriteAllLines(ProcessingCSV.getUserRequestPath(id), requests);     // Antragsdetails für Mitarbeiter aktualisieren
        }

        private void acceptClick(object sender, RoutedEventArgs e)      // Button Click Akzeptieren
        {
            if (openRequests.SelectedItem != null)
            {
                ListBoxItem request = openRequests.SelectedItem as ListBoxItem;
                acceptOrRecline(request.Content.ToString(), true);
                openRequests.Items.Remove(request);  
                MessageBox.Show("Antrag akzeptiert!", "Antrag bearbeitet", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);      // Feedback
            }
        }

        private void rejectClick(object sender, RoutedEventArgs e)      // Button Click Ablehnen
        {
            if (openRequests.SelectedItem != null)
            {
                ListBoxItem request = openRequests.SelectedItem as ListBoxItem;     
                acceptOrRecline(request.Content.ToString(), false);         // Ausgewähltes Element verarbeiten
                openRequests.Items.Remove(request);
                MessageBox.Show("Antrag abgelehnt!", "Antrag bearbeitet", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);       // Feedback
            }
        }
    }
}
