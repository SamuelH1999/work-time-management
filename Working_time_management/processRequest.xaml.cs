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
    /// Interaktionslogik für processRequest.xaml
    /// </summary>
    public partial class processRequest : Page
    {
        public bool holidayChecked;
        public processRequest()
        {
            InitializeComponent();
        }


        private void newHolidayRequest(NavigationEventArgs e)
        {
            bool holidayChecked = ParameterStore.holidayChecked;
            if (holidayChecked == true) 
            {
                newItem.Content = "Urlaub";        
            }
        }
    }
}
