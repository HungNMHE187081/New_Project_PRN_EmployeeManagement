using BusinessObjects.Models;
using DataAccessLayer;
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
    /// Interaction logic for EmployeeAttendanceRequest.xaml
    /// </summary>
    public partial class EmployeeAttendanceRequest : Window
    {
        private int employee_id;
        public EmployeeAttendanceRequest(int employee_id)
        {
            InitializeComponent();
            this.employee_id = employee_id;
            var state = AttendanceRequestState.Instance;
            if (state != null)
            {
                ExpectedDate.SelectedDate = state.ExpectedDate;
                Message.Text = state.Message;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            NotificationsDAO notificationsDAO = new NotificationsDAO();
            Notifications notification = new Notifications();
            notification.EmployeeId = employee_id;
            notification.Title = "Attendance Request";
            notification.Message = Message.Text;
            notification.CreatedDate = DateTime.Parse(ExpectedDate.Text);
            notification.IsApproved = 0;
            bool check = notificationsDAO.AddNotification(notification);
            if (check)
            {
                var state = AttendanceRequestState.Instance;
                state.ExpectedDate = null;
                state.Message = null;
                ExpectedDate.Text = "";
                Message.Text = "";
                MessageBox.Show("Request successfully");
            }
            else
            {
                MessageBox.Show("Request failed");
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var state = AttendanceRequestState.Instance;
            state.ExpectedDate = ExpectedDate.SelectedDate;
            state.Message = Message.Text;
            CustomerWindow customerWindow = new CustomerWindow(employee_id);
            customerWindow.Show();
            this.Close();
        }

        private void ValidDate(object sender, SelectionChangedEventArgs e)
        {
            if (ExpectedDate.SelectedDate < DateTime.Now.AddDays(1))
            {
                MessageBox.Show("Absence request should be 1 day ahead.");
            }
        }
    }
}
