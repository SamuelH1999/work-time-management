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

namespace Working_time_management
{
    /// <summary>
    /// Interaktionslogik für absence.xaml
    /// </summary>
    public partial class absence : Page
    {
        private List<Worker> listAbsence = new List<Worker>();
        public absence()
        {
            InitializeComponent();
            fillListBox();              //Liste beim Laden der Seite mit den aktuellen Abwesenheiten füllen
        }

        private void clickAddAbsence(object sender, RoutedEventArgs e)  //Öffnen einer neuen Seite um eine neue Abwesenheit zu erstellen
        {
            this.NavigationService.Navigate(new AddAbsence());
        }
        private void fillListBox()          //Die Liste wird mit den aktuellen Abwesenheiten gefüllt
        {
            foreach (string line in File.ReadLines(@"..\..\..\data\admin\absences.csv"))
            {
                string[] data = line.Split(';');
                string Id = data[0];
                string startdate = data[1];
                string enddate = data[2];
                string absenceReason = data[3];
                if (Id != "123123" && Id != null && Id != "ID")                                         // Die ID des Admins darf nicht verwendet werden, Id darf nicht leer sein und die Id darf nicht den ersten Wert in der CSV annehmen, welcher die Überschrift "ID" enthält
                {
                    string workerInformation = ProcessingCSV.GetWorkerInformation(Id);
                    string[] workerInformationSplit = workerInformation.Split(';');
                    string lastName = workerInformationSplit[0];
                    string firstName = workerInformationSplit[1];
                    listAbsence.Add(new Worker { LastName = lastName, FirstName = firstName, ID = Id, StartDate = startdate, EndDate = enddate, AbsenceReason = absenceReason});
                }
                else
                {

                }
            }
            for (int i = 0; i < listAbsence.Count; i++)
            {
                DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);        // Parameter um die Tage vergleichen zu können, ohne Berücksictigung der Stunden, Minuten und Sekunden
                ListBoxItem newWorker = new ListBoxItem();
                newWorker.Content = listAbsence[i].Absence();
                string[] startDate = listAbsence[i].StartDate.Split('.');
                string starteDateDay = startDate[0];
                int startDateDayInt = int.Parse(starteDateDay);                                                         // Muss für den Datentyp "DateTime" in int umgewandelt werden
                string startDateMonth = startDate[1];
                int startDateMonthInt = int.Parse(startDateMonth);
                string startDateYear = startDate[2];
                int startDateYearInt = int.Parse(startDateYear);
                DateTime start = new DateTime(startDateYearInt ,startDateMonthInt, startDateDayInt);
                string[] endDate = listAbsence[i].EndDate.Split('.');
                string endDateDay = endDate[0];
                int endDateDayInt = int.Parse(endDateDay);
                string endDateMonth = endDate[1];
                int endDateMonthInt = int.Parse(endDateMonth);
                string endDateYear = endDate[2];
                int endDateYearInt = int.Parse(endDateYear);
                DateTime end = new DateTime(endDateYearInt, endDateMonthInt, endDateDayInt); 
                if((start == today && end >= today) || (start < today && end >= today))                                 // Es sollen nur die Abwesenheiten angezeigt werden, welche gerade aktiv sind
                {
                        absenceList.Items.Add(newWorker);
                }
            }
        }
    }
}
