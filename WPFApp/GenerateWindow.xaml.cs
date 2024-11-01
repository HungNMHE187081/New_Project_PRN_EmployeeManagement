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
using System.Windows.Shapes;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for GenerateWindow.xaml
    /// </summary>
    public partial class GenerateWindow : Window
    {
        public GenerateWindow()
        {
            InitializeComponent();
        }
        private void btnGeneral_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ReportWindow reportWindow = new ReportWindow();
            reportWindow.Show();
            this.Close();
        }

        private void btnPersonal_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            //PersonalReport personalReport = new PersonalReport();
            //personalReport.Show();
            //this.Close();
        }
    }
}