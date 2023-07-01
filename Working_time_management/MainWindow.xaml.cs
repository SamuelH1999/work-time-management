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
        public string logoPath;
        public int timeRounding;
        public string fixBreakTime;
        public int breakAfterHours;

        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.Content = new Startpage();
            DispatcherTimer clockTimer = new DispatcherTimer();
            clockTimer.Interval = TimeSpan.FromSeconds(1);
            clockTimer.Tick += clockTick;
            clockTimer.Start();

            logoPath = IniHandler.Read("Path", "Logo");
            timeRounding = int.Parse(IniHandler.Read("Rounding", "Time"));
            fixBreakTime = IniHandler.Read("Fix", "Breaks");
            breakAfterHours = int.Parse(IniHandler.Read("AfterHours", "Breaks"));
            imgLogo.Source = new BitmapImage(new Uri(logoPath, UriKind.Relative));
            
        }

        void clockTick(object sender, EventArgs e)
        {
            tbClock.Content = " " + DateTime.Now.ToString("HH:mm") + " Uhr\n" + DateTime.Now.ToString("dd.MM.yyyy");
        }
    }
}
