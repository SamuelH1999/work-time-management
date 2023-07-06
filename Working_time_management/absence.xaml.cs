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
            fillListBox();
        }

        private void clickAddAbsence(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddAbsence());
        }
        private void fillListBox()
        {
            foreach (string line in File.ReadLines(@"..\..\..\data\admin\absences.csv"))
            {
                string[] data = line.Split(';');
                string Id = data[0];
                string startdate = data[1];
                string enddate = data[2];
                string absenceReason = data[3];
                if (Id != "123123" && Id != null && Id != "ID")
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
                DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                ListBoxItem newWorker = new ListBoxItem();
                newWorker.Content = listAbsence[i].Absence();
                string[] startDate = listAbsence[i].StartDate.Split('.');
                string starteDateDay = startDate[0];
                int startDateDayInt = int.Parse(starteDateDay);
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
                DateTime end = new DateTime(endDateYearInt, endDateMonthInt, endDateDayInt); //DateTime erstellen und dann Start und Enddatum mit dem aktuellen Wert vergleichen
                if((start == today && end >= today) || (start < today && end >= today)) 
                {
                        absenceList.Items.Add(newWorker);
                }
            }
        }
    }
}
