using BusinessObjects.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for SalaryDetail.xaml
    /// </summary>
    public partial class SalaryDetail : Window
    {
        private int _employeeId;
        public SalaryDetail(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
            LoadData();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AttendanceList attendanceList = new AttendanceList();
            attendanceList.Show();
            this.Close();
        }

        private void DataGridSalaryModification_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridSalaryModification.SelectedItem != null)
            {
                SalaryModification salaryModification = (SalaryModification)DataGridSalaryModification.SelectedItem;
                int employeeId = salaryModification.EmployeeId;
                ReportDate.Text = salaryModification.Date.ToString();
                String amount = salaryModification.Amount.ToString().TrimEnd();
                string datbase_status = salaryModification.Status;
                if (datbase_status.Equals("bonus"))
                {
                    Amount.Text = "Bonus" + " " + amount + "$";
                }
                else
                {
                    Amount.Text = "Deduct" + " " + amount + "$";
                }
                ReportDescription.Text = salaryModification.Description;
            }
        }

        private void LoadData()
        {
            EmployeesDAO employeeDAO = new EmployeesDAO();
            SalariesDAO salaryDAO = new SalariesDAO();
            List<SalaryModification> salaries = salaryDAO.GetSalaryModificationsOfAnEmployee(_employeeId);
            DataGridSalaryModification.ItemsSource = salaries;
            String report = "Employee ID: " + _employeeId + "\n" +
                "Employee Name: " + employeeDAO.GetNameById(_employeeId) + "\n" +
                "Salary Detail Of: " + DateOnly.FromDateTime(DateTime.Now).ToString("MM/yyyy") + "\n\n"
                ;
            for (int i = 0; i < salaries.Count; i++)
            {
                if (salaries[i].Status.Equals("bonus"))
                {
                    report += salaries[i].Date + " " + "Bonus" + "  " + salaries[i].Amount + "\n"
                        + "------------------------\n";
                }
                else
                {
                    report += salaries[i].Date + " " + "Deduct" + " " + salaries[i].Amount + "\n"
                        + "------------------------\n";
                }
            }
            double deduction = salaryDAO.GetSalaryDeductionOfAnEmployee(_employeeId);
            double bonus = salaryDAO.GetSalaryBonusOfAnEmployee(_employeeId);
            double allowance = salaryDAO.GetSalaryAllowanceOfAnEmployee(_employeeId);
            double salary = bonus + allowance - deduction;
            report += "Total Salary: " + salary + "$";
            ReportEmployeeName.Text = report;
        }


        private void Export_Click(object sender, RoutedEventArgs e)
        {
            string filePath = @"D:\VisualStudio\source\repos\ProjectPRN\salary.txt";

            SalariesDAO salaryDAO = new SalariesDAO();
            EmployeesDAO employeeDAO = new EmployeesDAO();
            List<SalaryModification> salaries = salaryDAO.GetSalaryModificationsOfAnEmployee(_employeeId);
            DataGridSalaryModification.ItemsSource = salaries;
            String report = "Employee ID: " + _employeeId + "\n" +
               "Employee Name: " + employeeDAO.GetNameById(_employeeId) + "\n" +
               "Salary Detail Of: " + DateOnly.FromDateTime(DateTime.Now).ToString("MM/yyyy") + "\n\n";
            for (int i = 0; i < salaries.Count; i++)
            {
                if (salaries[i].Status.Equals("bonus"))
                {
                    report += salaries[i].Date + " " + "Bonus" + "  " + salaries[i].Amount + "\n"
                        + "------------------------\n";
                }
                else
                {
                    report += salaries[i].Date + " " + "Deduct" + " " + salaries[i].Amount + "\n"
                        + "------------------------\n";
                }
            }
            double deduction = salaryDAO.GetSalaryDeductionOfAnEmployee(_employeeId);
            double bonus = salaryDAO.GetSalaryBonusOfAnEmployee(_employeeId);
            double allowance = salaryDAO.GetSalaryAllowanceOfAnEmployee(_employeeId);
            double salary = bonus + allowance - deduction;
            report += "Total Salary: " + salary + "$\n" +
                "------------------------------------------------\n";

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(report);
                }
                System.Windows.MessageBox.Show("Xuất thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
