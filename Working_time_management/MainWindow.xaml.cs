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
using System.Windows.Threading;

namespace Working_time_management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string logoPath;
        public static int timeRounding;
        public static string fixBreakTime = null;
        public static int breakAfterHours = 0;

        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.Content = new Startpage();
            DispatcherTimer clockTimer = new DispatcherTimer();
            clockTimer.Interval = TimeSpan.FromSeconds(1);
            clockTimer.Tick += clockTick;
            clockTimer.Start();
            getIniVals();
            
        }

        private void getIniVals()
        {
            logoPath = IniHandler.Read("Path", "Logo");
            timeRounding = int.Parse(IniHandler.Read("Rounding", "Time"));
            if (IniHandler.KeyExists("Fix", "Breaks"))
            {
                fixBreakTime = IniHandler.Read("Fix", "Breaks");
            }
            else
            {
                fixBreakTime = null;
            }
            if (IniHandler.KeyExists("AfterHours", "Breaks"))
            {
                breakAfterHours = int.Parse(IniHandler.Read("AfterHours", "Breaks"));
            }
            else
            {
                breakAfterHours = 0;
            }
            imgLogo.Source = new BitmapImage(new Uri(logoPath, UriKind.Relative));
        }
        private void clockTick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            tbClock.Content = " " + currentTime.ToString("HH:mm") + " Uhr\n" + currentTime.ToString("dd.MM.yyyy");
            if (currentTime.ToString("HH:mm") == "00:00")
            {
                getIniVals();
            }
        }
    }
}
