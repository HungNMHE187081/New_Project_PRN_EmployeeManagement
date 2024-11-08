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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for SalaryDetail.xaml
    /// </summary>
    public partial class SalaryDetail : Window
    {
        private int _employeeId;
        private DateOnly _dateOnly;
        private string _state;
        public SalaryDetail(int employeeId, DateOnly date, string state)
        {
            InitializeComponent();
            _employeeId = employeeId;
            _dateOnly = date;
            _state = state;
            LoadData();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if(_state == "attendance_list")
            {
                AttendanceList attendanceList = new AttendanceList();
                attendanceList.Show();
                this.Close();
            }
            else
            {
                SalaryList salaryList = new SalaryList();
                salaryList.Show();
                this.Close();
            }
        }

        private void DataGridSalaryModification_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void LoadData()
        {
            EmployeesDAO employeeDAO = new EmployeesDAO();
            SalariesDAO salaryDAO = new SalariesDAO();
            List<SalaryModification> salaries = salaryDAO.GetSalaryModificationsOfAnEmployee(_employeeId, _dateOnly);
            DataGridSalaryModification.ItemsSource = salaries;
            String report = "Employee ID: " + _employeeId + "\n" +
                "Employee Name: " + employeeDAO.GetNameById(_employeeId) + "\n" +
                "Salary Detail Of: " + _dateOnly.ToString("MM/yyyy") + "\n\n"
                ;
            for (int i = 0; i < salaries.Count; i++)
            {
                if (salaries[i].Status.Trim()=="bonus")
                {
                    report += salaries[i].Date + " " + "Bonus" + "  " + salaries[i].Amount + "\n"
                        + "------------------------\n";
                }
                else
                {
                    report += salaries[i].Date + " " + "Deduct" + "  " + salaries[i].Amount + "\n"
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
            List<SalaryModification> salaries = salaryDAO.GetSalaryModificationsOfAnEmployee(_employeeId,_dateOnly);
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
