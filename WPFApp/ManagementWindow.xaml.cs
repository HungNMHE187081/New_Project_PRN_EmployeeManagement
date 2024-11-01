using System;
using System.Windows;
using OfficeOpenXml;
using Services; // Đảm bảo đã thêm namespace cho service
using System.Collections.Generic;
using BusinessObjects.Models;
using DataAccessLayer;

namespace WPFApp
{

    public partial class ManagementWindow : Window
    {
        private PRN_EmployeeManagementContext _context;

        public ManagementWindow()
        {
            InitializeComponent();
            _context = new PRN_EmployeeManagementContext();

        }
        private void ManageUsersAccount_Click(object sender, RoutedEventArgs e)
        {
            UserAccountManagement userAccountManagementWindow = new UserAccountManagement();
            userAccountManagementWindow.Show();
            this.Close();
        }

        private void ManageEmployeesAccount_Click(object sender, RoutedEventArgs e)
        {
            EmployeesManagementWindow employeesManagementWindow = new EmployeesManagementWindow();
            employeesManagementWindow.Show();
            this.Close();
        }

        private void ManageDepartment_Click(object sender, RoutedEventArgs e)
        {
            DepartmentList department = new DepartmentList();
            department.Show();
            this.Close();
        }

        private void ManageSalary_Click(object sender, RoutedEventArgs e)
        {
            SalaryList salary = new SalaryList();
            salary.Show();
            this.Close();
        }

        private void ManageAttendance_Click(object sender, RoutedEventArgs e)
        {
            AttendanceList attendance = new AttendanceList();
            attendance.Show();
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}