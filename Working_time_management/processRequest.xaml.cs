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
            foreach (string line in File.ReadLines(ProcessingCSV.idPwdPath))
            {
                string[] data = line.Split(';');
                string id = data[0];
                if (id != "123123" && id != "ID")
                {
                    string[] workerInfo = ProcessingCSV.GetWorkerInformation(id).Split(';');
                    string[] requests = ProcessingCSV.getAllWorkerRequests(id);
                    foreach (string request in requests)
                    {
                        string[] details = request.Split(';');
                        if(details.Length > 3)
                        {
                            string reason = details[0];
                            string from = details[1];
                            string until = details[2];
                            string status = details[3];

                            if (status == "offen")
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

        private void showDetails(object sender, RoutedEventArgs e)
        {
            if (openRequests.SelectedItem != null)
            {
                ListBoxItem details = openRequests.SelectedItem as ListBoxItem;
                string[] names = details.Content.ToString().Split();
                MessageBox.Show("Antragsteller: " + names[0] + " " + names[1] + "\nZeitraum: 30.06.2023 - 14.07.2023", "Antragsdetails", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }

        private void acceptOrRecline(string request, bool decision)
        {
            string[] details = request.Split(", ");
            string id = details[0];
            string reason = details[3];
            string[] timeSpan = details[4].Split(" - ");
            string from = timeSpan[0];
            string until = timeSpan[1];
            string newStatus; 
            

            string[] requests = ProcessingCSV.getAllWorkerRequests(id);
            for (int i = 0; i < requests.Length; i++)
            {
                string[] csvDetails = requests[i].Split(';');
                if (csvDetails[0] == reason && csvDetails[1] == from && csvDetails[2] == until)
                {
                    if (decision)
                    {
                        newStatus = "genehmigt";
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
                        for (DateTime dt = fromDate; dt <= untilDate; dt = dt.AddDays(1))
                        {
                            if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
                            {
                                absenceDays++;
                            }
                        }

                        if(reason == "Urlaub")
                        {
                            string[] workTimeInformation = File.ReadAllLines(ProcessingCSV.getWorkingTimeInformationPath(id));
                            string[] values = workTimeInformation[1].Split(";");
                            int currentHolidays = int.Parse(values[2]) - absenceDays;
                            workTimeInformation[1] = values[0] + ";" + values[1] + ";" + currentHolidays;
                            File.WriteAllLines(ProcessingCSV.getWorkingTimeInformationPath(id),workTimeInformation, Encoding.UTF8);
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
                                overTimeMinutes *= -1;
                            }
                            TimeSpan overtimeSpan = new TimeSpan(overTimeHours, overTimeMinutes, 0).Subtract(new TimeSpan(8*absenceDays, 0, 0));
                            string[] newovertime = overtimeSpan.ToString().Split(':');
                            if (newovertime[0].Contains('.'))
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
                            File.WriteAllLines(ProcessingCSV.getWorkingTimeInformationPath(id), workTimeInformation, Encoding.UTF8);
                        }
                        ProcessingCSV.addAbsenceToAbsenceCSV(id, from, until, reason);
                    }
                    else
                    {
                        newStatus = "abgelehnt";
                    }
                    requests[i] = csvDetails[0] + ";" + csvDetails[1] + ";" + csvDetails[2] + ";" + newStatus;
                    break;
                }
            }
            File.WriteAllLines(ProcessingCSV.getUserRequestPath(id), requests);
        }

        private void acceptClick(object sender, RoutedEventArgs e)
        {
            if (openRequests.SelectedItem != null)
            {
                ListBoxItem request = openRequests.SelectedItem as ListBoxItem;
                acceptOrRecline(request.Content.ToString(), true);
                openRequests.Items.Remove(request);  
                MessageBox.Show("Antrag akzeptiert!", "Antrag bearbeitet", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }

        private void rejectClick(object sender, RoutedEventArgs e)
        {
            if (openRequests.SelectedItem != null)
            {
                ListBoxItem request = openRequests.SelectedItem as ListBoxItem;
                acceptOrRecline(request.Content.ToString(), false);
                openRequests.Items.Remove(request);
                MessageBox.Show("Antrag abgelehnt!", "Antrag bearbeitet", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }
    }
}
