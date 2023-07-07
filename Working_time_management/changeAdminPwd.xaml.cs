using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaktionslogik für changeAdminPwd.xaml
    /// </summary>
    public partial class changeAdminPwd : Page
    {
        public changeAdminPwd()
        {
            InitializeComponent();
        }

        private void btnAdminPwdConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(ProcessingCSV.checkLogIn("123123", tbOldPwd.Text, true) == (int) ProcessingCSV.LogInResult.AdminCorrect && tbNewPwd.Text.Length > 0)
            {
                ProcessingCSV.editUserPwdToCSV("123123", tbNewPwd.Text);
                this.NavigationService.GoBack();
            }
        }
    }
}
