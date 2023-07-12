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
    /// Interaktionslogik für Holidays.xaml
    /// </summary>
    public partial class Holidays : Page
    {
        public Holidays(string id)
        {
            InitializeComponent();
            string[] information = File.ReadAllLines(ProcessingCSV.getWorkingTimeInformationPath(id))[1].Split(';');
            string holidaysRemaining = information[2];
            string overtime = information[1];
            lblHolidays.Content = holidaysRemaining;
            lblOvertime.Content = overtime; 
        }
    }
}
