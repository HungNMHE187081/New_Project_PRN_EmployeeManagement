using BusinessObjects.Models;
using ClosedXML.Excel;
using DataAccessLayer;
using DocumentFormat.OpenXml.Bibliography;
using Services;
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
    /// Interaction logic for PersonalReport.xaml
    /// </summary>
    public partial class PersonalReport : Window
    {
        private List<Employees> employees = new List<Employees>();
        private readonly PRN_EmployeeManagementContext _context;
        private readonly EmployeesService _employeesService;
        private readonly ReportService _reportService;
        public PersonalReport()
        {
            InitializeComponent();
            _context = new PRN_EmployeeManagementContext();
            _employeesService = new EmployeesService();
            _reportService = new ReportService();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadEmployeeLeaveData();
        }
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string currentDate = DateTime.Now.ToString("yyyy_MM_dd");
            string filePath = @"C:\FPT\PersonalReport_" + currentDate + ".xlsx";
            try
            {
                // Call the export function
                ExportDataGridToExcel(dgData, filePath);

                // Optionally, show a confirmation message
                MessageBox.Show("Data exported successfully!", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Display an error message if export fails
                MessageBox.Show($"An error occurred: {ex.Message}", "Export Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void ExportDataGridToExcel(DataGrid dataGrid, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Data");

                // Add headers
                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    //worksheet.Cell(1, i + 1).Value = dataGrid.Columns[i].Header;
                    worksheet.Cell(1, i + 1).Value = dataGrid.Columns[i].Header?.ToString() ?? string.Empty;

                }

                // Add data rows
                for (int row = 0; row < dataGrid.Items.Count; row++)
                {
                    for (int col = 0; col < dataGrid.Columns.Count; col++)
                    {
                        var cellValue = (dataGrid.Columns[col].GetCellContent(dataGrid.Items[row]) as TextBlock)?.Text;
                        worksheet.Cell(row + 2, col + 1).Value = cellValue;
                    }
                }

                // Save to file
                workbook.SaveAs(filePath);
            }
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void LoadEmployeeLeaveData()
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var employeeLeaveBalances = from e in context.Employees
                                            join lb in context.LeaveBalances on e.EmployeeID equals lb.EmployeeID
                                            join s in context.Salaries on lb.EmployeeID equals s.EmployeeID
                                            select new
                                            {
                                                e.Phone,
                                                e.Departments.DepartmentName,
                                                e.EmployeeID,
                                                e.FullName,
                                                e.BaseSalary,
                                                s.Allowance,
                                                MonthlySalary = e.BaseSalary + s.Allowance + s.Bonus - s.Deduction, // Example calculation
                                                lb.AnnualLeave,
                                                lb.SickLeave,
                                                lb.UnpaidLeave,
                                                RemainingLeave = lb.AnnualLeave + lb.SickLeave - lb.UnpaidLeave
                                            };

                var monthlySalaries = employeeLeaveBalances.ToList();

                var report = monthlySalaries
                    .GroupBy(emp => emp.FullName)
                    .Select(g => new Report
                    {
                        ID = g.FirstOrDefault().EmployeeID,
                        Name = g.Key,
                        Phone = g.FirstOrDefault().Phone,
                        DepartmentName = g.FirstOrDefault().DepartmentName,
                        //BaseSalary = g.First().BaseSalary,
                        //Allowance = g.FirstOrDefault().Allowance,
                        MonthlySalary = g.Sum(x => x.MonthlySalary), // Total of individual monthly salaries
                        AnnualLeave = g.First().AnnualLeave,
                        SickLeave = g.First().SickLeave,
                        UnpaidLeave = g.First().UnpaidLeave,
                        RemainingLeave = g.First().RemainingLeave
                    })
                    .ToList();
                var dataList = employeeLeaveBalances.ToList();
                dgData.ItemsSource = report;
                //MessageBox.Show("Data retrieved: " + dataList.Count);
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            ManagementWindow managementWindow = new ManagementWindow();
            managementWindow.Show();
            this.Close();
        }

        private void ReportButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void Notification_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckOutButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
