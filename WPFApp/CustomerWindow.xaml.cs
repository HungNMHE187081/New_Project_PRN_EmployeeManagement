using System;
using System.Windows;
using System.Windows.Forms;
using BusinessObjects.Models;
using DataAccessLayer; // Ensure this is using your actual namespace

namespace WPFApp
{
    public partial class CustomerWindow : Window
    {
        private PRN_EmployeeManagementContext _context;
        private int employeeId;

        public CustomerWindow(int employeeId)
        {
            InitializeComponent();
            _context = new PRN_EmployeeManagementContext();
            this.employeeId = employeeId;
        }

       

        private void CheckInButtonClick(object sender, RoutedEventArgs e)
        {
            AttendanceDAO attendanceDAO = new AttendanceDAO();
            bool checkin = attendanceDAO.CheckInNotInTime(employeeId);
            if (checkin)
            {
                System.Windows.MessageBox.Show("Check in successfully");
            }
            else
            {
                System.Windows.MessageBox.Show("Check in failed");
            }

        }

        private void CheckOutButtonClick(object sender, RoutedEventArgs e)
        {
            AttendanceDAO attendanceDAO = new AttendanceDAO();
            bool checkout = attendanceDAO.CheckOutNotInTime(employeeId);
            if (checkout)
            {
                System.Windows.MessageBox.Show("Check out successfully");
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Check out failed");
            }
        }

        private void SalaryButtonClick(object sender, RoutedEventArgs e)
        {
            CustomerSalaryDetail customerSalaryDetailWindow = new CustomerSalaryDetail(employeeId);
            customerSalaryDetailWindow.Show();
            this.Close();
        }

        private void Notification_Click(object sender, RoutedEventArgs e)
        {
            EmployeeNotificationList employeeNotificationListWindow = new EmployeeNotificationList(employeeId);
            employeeNotificationListWindow.Show();
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            EmployeeAttendanceRequest employeeAttendanceRequestWindow = new EmployeeAttendanceRequest(employeeId);
            employeeAttendanceRequestWindow.Show();
            this.Close();
        }

        private void Attendance_Click(object sender, RoutedEventArgs e)
        {
            EmployeeAttendanceRequest employeeAttendanceRequestWindow = new EmployeeAttendanceRequest(employeeId);
            employeeAttendanceRequestWindow.Show();
            this.Close();
        }
    }
}