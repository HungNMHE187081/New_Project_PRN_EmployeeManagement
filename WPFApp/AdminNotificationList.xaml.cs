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
    /// Interaction logic for AdminNotificationList.xaml
    /// </summary>
    public partial class AdminNotificationList : Window
    {
        public AdminNotificationList()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            NotificationsDAO notificationsDAO = new NotificationsDAO();
            List<Notifications> notifications = notificationsDAO.GetAllNotifications();
            DataGridNotifications.ItemsSource = notifications;
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            NotificationsDAO notificationsDAO = new NotificationsDAO();
            string search = Filter.Text;
            List<Notifications> notifications = notificationsDAO.GetAllNotificationsByTitle(search);
            DataGridNotifications.ItemsSource = notifications;
        }

        private void DataGridNotifications_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridNotifications.SelectedItem != null)
            {
                Notifications notification = (Notifications)DataGridNotifications.SelectedItem;
                EmployeesDAO employeesDAO = new EmployeesDAO();
                if (notification.Title.Contains("Salary"))
                {
                    SalaryRequest.Visibility = Visibility.Visible;
                    AttendanceRequest.Visibility = Visibility.Hidden;
                    string employee_name = employeesDAO.GetNameById((int)notification.EmployeeId);
                    SEmployeeName.Text = employee_name;
                    SDepartment_Name.Text = employeesDAO.GetDepartmentNameById((int)notification.EmployeeId);
                }
                else
                {
                    SalaryRequest.Visibility = Visibility.Hidden;
                    AttendanceRequest.Visibility = Visibility.Visible;
                    string employee_name = employeesDAO.GetNameById((int)notification.EmployeeId);
                    AEmployeeName.Text = employee_name;
                    ADepartment_Name.Text = employeesDAO.GetDepartmentNameById((int)notification.EmployeeId);

                }
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            ManagementWindow managementWindow = new ManagementWindow();
            managementWindow.Show();
            this.Close();
        }

        private void SalaryApprove_Click(object sender, RoutedEventArgs e)
        {
            string message=SMessage.Text;
            NotificationsDAO notificationsDAO = new NotificationsDAO();
            Notifications notification = (Notifications)DataGridNotifications.SelectedItem;

            bool check = notificationsDAO.ApproveNotification((int)notification.NotificationId, message);
            if (check)
            {
                MessageBox.Show("Approve successfully");
                SalaryRequest.Visibility = Visibility.Hidden;
                LoadData();
            }
            else
            {
                MessageBox.Show("Approve failed");
            }
        }

        private void AttendanceApprove_Click(object sender, RoutedEventArgs e)
        {
            string message = AMessage.Text;
            NotificationsDAO notificationsDAO = new NotificationsDAO();
            Notifications notification = (Notifications)DataGridNotifications.SelectedItem;
            DateTime expected_date= (DateTime)notification.CreatedDate;
            DateOnly dateOnly = DateOnly.FromDateTime(expected_date);
            bool check = notificationsDAO.ApproveNotification((int)notification.NotificationId, message);
            if (check)
            {       
                AttendanceDAO attendanceDAO = new AttendanceDAO();
                bool checkin = attendanceDAO.AddPermissionAttendance((int)notification.EmployeeId, dateOnly);
                if (checkin) {
                    MessageBox.Show("Approve successfully");
                    AttendanceRequest.Visibility = Visibility.Hidden;
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Updated failed");

                }
            }
            else
            {
                MessageBox.Show("Approve failed");
            }

        }

        private void AttendaceDisApprove_Click(object sender, RoutedEventArgs e)
        {
            string message = AMessage.Text;
            NotificationsDAO notificationsDAO = new NotificationsDAO();
            Notifications notification = (Notifications)DataGridNotifications.SelectedItem;
            bool check = notificationsDAO.RejectNotification((int)notification.NotificationId, message);
            if (check)
            {
                MessageBox.Show("Reject successfully");
                AttendanceRequest.Visibility = Visibility.Hidden;
                LoadData();
            }
            else
            {
                MessageBox.Show("Reject failed");
            }
        }

        private void SalaryDisApprove_Click(object sender, RoutedEventArgs e)
        {
            string message = SMessage.Text;
            NotificationsDAO notificationsDAO = new NotificationsDAO();
            Notifications notification = (Notifications)DataGridNotifications.SelectedItem;

            bool check = notificationsDAO.RejectNotification((int)notification.NotificationId, message);
            if (check)
            {
                MessageBox.Show("Reject successfully");
                SalaryRequest.Visibility = Visibility.Hidden;
                LoadData();
            }
            else
            {
                MessageBox.Show("Reject failed");
            }
        }


        private void SalaryApproveOpenBox(object sender, RoutedEventArgs e)
        {
            ConfirmBox.Visibility = Visibility.Visible;
            MessageConfirm.Text = "Do you want to approve this Salary request?";
        }

        private void SalaryDisapproveOPpenBox(object sender, RoutedEventArgs e)
        {
            ConfirmBox.Visibility = Visibility.Visible;
            MessageConfirm.Text = "Do you want to disapprove this Salary request?";
        }

        private void ApproveConfirmation(object sender, RoutedEventArgs e)
        {
            if(MessageConfirm.Text.Equals("Do you want to approve this Salary request?"))
            {
                ConfirmBox.Visibility = Visibility.Hidden;
                SalaryApprove_Click(sender, e);
            }
            else if(MessageConfirm.Text.Equals("Do you want to disapprove this Salary request?")) 
            {
                ConfirmBox.Visibility = Visibility.Hidden;
                SalaryDisApprove_Click(sender, e);
            }
            else if (MessageConfirm.Text.Equals("Do you want to approve this Attendance request?"))
            {
                ConfirmBox.Visibility = Visibility.Hidden;
                AttendanceApprove_Click(sender, e);
            }
            else
            {
                ConfirmBox.Visibility = Visibility.Hidden;
                AttendaceDisApprove_Click(sender, e);
            }
        }

        private void DisapproveConfirmation(object sender, RoutedEventArgs e)
        {
            ConfirmBox.Visibility = Visibility.Hidden;
        }

        private void AttendanceApproveOpenBox(object sender, RoutedEventArgs e)
        {
            ConfirmBox.Visibility = Visibility.Visible;
            MessageConfirm.Text = "Do you want to approve this Attendance request?";
        }

        private void AttendanceDisapproveOpenBox(object sender, RoutedEventArgs e)
        {
            ConfirmBox.Visibility = Visibility.Visible;
            MessageConfirm.Text = "Do you want to disapprove this Attendance request?";
        }
    }
}
