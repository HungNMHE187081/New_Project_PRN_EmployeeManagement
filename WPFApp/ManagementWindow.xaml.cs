using System;
using System.Windows;
using OfficeOpenXml;
using Services; // Đảm bảo đã thêm namespace cho service
using System.Collections.Generic;
using BusinessObjects.Models;
using DataAccessLayer;
using LiveCharts;

namespace WPFApp
{

    public partial class ManagementWindow : Window
    {
        
        public ManagementWindow()
        {
            InitializeComponent();
            DepartmentsDAO departmentsDAO = new DepartmentsDAO();
            DataContext = this;
            List<int> number_of_employeeinadepartments = departmentsDAO.GetNumberOfEmployeesInEachDepartment();
            List<double> avg_salries = departmentsDAO.GetSalaryOfEachDepartment();
            EmployeeValues = new ChartValues<int> ( number_of_employeeinadepartments );
            SalaryValues = new ChartValues<double> (avg_salries);
            AbsenceRateValues = new ChartValues<double> { 5, 3, 4, 2 };
            DepartmentLabels = new List<string> { "HR", "IT", "Finance", "Marketing" };
        }
        public ChartValues<int> EmployeeValues { get; set; }
        public ChartValues<double> SalaryValues { get; set; }
        public ChartValues<double> AbsenceRateValues { get; set; }
        public List<string> DepartmentLabels { get; set; }
        private void ManageUsersAccount_Click(object sender, RoutedEventArgs e)
        {
            ManagementWindow managementWindow = new ManagementWindow();
            managementWindow.Show();
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

        private void Notification_Click(object sender, RoutedEventArgs e)
        {
            AdminNotificationList adminNotificationListWindow = new AdminNotificationList();
            adminNotificationListWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           ReportWindow reportWindow = new ReportWindow();
            reportWindow.Show();
            this.Close();
        }

        private void CartesianChart_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}