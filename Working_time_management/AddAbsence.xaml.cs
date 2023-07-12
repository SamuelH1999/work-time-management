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
    /// Interaktionslogik für AddAbsence.xaml
    /// </summary>
    public partial class AddAbsence : Page
    {
        public AddAbsence()
        {
            InitializeComponent();
        }

        private void clickConfirm(object sender, RoutedEventArgs e)
        {
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);        // Mit dieser Variable werden die Abwesenheitstage verglichen
            if (dpFrom.SelectedDate == null)                                                                        // Es muss ein Datum asugewählt werden
            {
                return;
            }

            if (dpTo.SelectedDate == null)                                                                          // Es muss ein Datum asugewählt werden
            {
                return;
            }
            if (dpFrom.SelectedDate.Value > dpTo.SelectedDate.Value || dpFrom.SelectedDate.Value < today || dpTo.SelectedDate.Value < today)  // Enddatum darf nicht vor Stardatum liegen
            {                                                                                                                                 // End- und Startdatum dürfen nicht voe dem heutigen Datum liegen
                MessageBox.Show("Ungültige Eingabe", "Eingabefehler", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }
            string from = dpFrom.SelectedDate.Value.ToString("dd.MM.yyyy");
            string until = dpTo.SelectedDate.Value.ToString("dd.MM.yyyy");
            bool IDFound = ProcessingCSV.addAbsenceToAbsenceCSV(tbID.Text, from, until, "Krankheit");
            if (!IDFound)                                                                                                                     // Fehlermeldung, wenn falsche ID eingegeben wird
            {
                MessageBox.Show("ID nicht gefunden!", "Eingabefehler", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
            }
            this.NavigationService.Navigate(new absence());
        }
    }
}
