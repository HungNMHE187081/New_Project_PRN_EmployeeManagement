using BusinessObjects.Models;
using DataAccessLayer;
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
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        private List<Employees> employees = new List<Employees>();
        private readonly PRN_EmployeeManagementContext _context;
        private readonly EmployeesService _employeesService;
        private readonly ReportService _reportService;
        public ReportWindow()
        {
            InitializeComponent();
        }
        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    LoadEmployeeStatistics();
        //}
        //private void LoadEmployeeStatistics()
        //{
        //    try
        //    {
        //        dgData.ItemsSource = null;
        //        var reports = _reportService.GetReports();

        //        dgData.ItemsSource = reports;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //}

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            //string currentDate = DateTime.Now.ToString("yyyy_MM_dd");
            //string filePath = @"C:\FPT\GeneralReport_" + currentDate + ".xlsx";
            //try
            //{
            //    // Call the export function
            //    ExportDataGridToExcel(dgData, filePath);

            //    // Optionally, show a confirmation message
            //    MessageBox.Show("Data exported successfully!", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //catch (Exception ex)
            //{
            //    // Display an error message if export fails
            //    MessageBox.Show($"An error occurred: {ex.Message}", "Export Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }
        //public void ExportDataGridToExcel(DataGrid dataGrid, string filePath)
        //{
        //    using (var workbook = new XLWorkbook())
        //    {
        //        var worksheet = workbook.Worksheets.Add("Data");

        //        // Add headers
        //        for (int i = 0; i < dataGrid.Columns.Count; i++)
        //        {
        //            //worksheet.Cell(1, i + 1).Value = dataGrid.Columns[i].Header;
        //            worksheet.Cell(1, i + 1).Value = dataGrid.Columns[i].Header?.ToString() ?? string.Empty;

        //        }

        //        // Add data rows
        //        for (int row = 0; row < dataGrid.Items.Count; row++)
        //        {
        //            for (int col = 0; col < dataGrid.Columns.Count; col++)
        //            {
        //                var cellValue = (dataGrid.Columns[col].GetCellContent(dataGrid.Items[row]) as TextBlock)?.Text;
        //                worksheet.Cell(row + 2, col + 1).Value = cellValue;
        //            }
        //        }

        //        // Save to file
        //        workbook.SaveAs(filePath);
        //    }
        //}
    }
}
