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
    /// Interaktionslogik für settings.xaml
    /// </summary>
    public partial class settings : Page
    {
        public settings()
        {
            InitializeComponent();
        }

        private void selectDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(IniHandler.KeyExists("Maximum", "Breaks"))
            {
                IniHandler.DeleteKey("Maximum", "Breaks");
            }
            IniHandler.Write("Maximum", selectDuration.Text, "Breaks");
        }

        private void selectTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IniHandler.KeyExists("Fix", "Breaks"))
            {
                IniHandler.DeleteKey("Fix", "Breaks");
            }
            IniHandler.Write("Fix", selectTime.Text, "Breaks");
        }
    }
}
