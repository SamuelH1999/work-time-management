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
        public settings()
        {
            InitializeComponent();

            for (int i = 0; i < selectRounding.Items.Count; i++)
            {
                ComboBoxItem item = (ComboBoxItem)selectRounding.Items[i];
                if (item != null)
                {
                    if (string.Compare(item.Content.ToString(), IniHandler.Read("Rounding", "Time")) == 0)
                    {
                        selectRounding.SelectedItem = item;
                        break;
                    }
                }
            }

            for (int i = 0; i < selectDuration.Items.Count; i++)
            {
                ComboBoxItem item = (ComboBoxItem) selectDuration.Items[i];
                if (item != null)
                {
                    if (string.Compare(item.Content.ToString(), IniHandler.Read("AfterHours", "Breaks")) == 0)
                    {
                        selectDuration.SelectedItem = item;
                        break;
                    }                
                }
            }

            for (int i = 0; i < selectTime.Items.Count; i++)
            {
                ComboBoxItem item = (ComboBoxItem)selectTime.Items[i];
                if (item != null)
                {
                    if (string.Compare(item.Content.ToString(), IniHandler.Read("Fix", "Breaks")) == 0)
                    {
                        selectTime.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void selectDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selected = selectDuration.SelectedItem as ComboBoxItem;
            if (selected != null)
            {
                IniHandler.Write("AfterHours", selected.Content.ToString(), "Breaks");
            }
            
        }

        private void selectTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selected = selectTime.SelectedItem as ComboBoxItem;
            if(selected != null)
            {
                IniHandler.Write("Fix", selected.Content.ToString(), "Breaks");
            }
            
        }

        private void selectRounding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selected = selectRounding.SelectedItem as ComboBoxItem;
            if (selected != null)
            {
                IniHandler.Write("Rounding", selected.Content.ToString(), "Time");
            }
        }

        private void btnPicPath_Click(object sender, RoutedEventArgs e)
        {
            string newLogoPath = tbLogoPath.Text;
            IniHandler.Write("Path", newLogoPath, "Logo");
        }

        private void btnNewPwd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
