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
    /// Interaction logic for SalaryList.xaml
    /// </summary>
    public partial class SalaryList : Window
    {
        public SalaryList()
        {
            InitializeComponent();
            LoadData();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ManagementWindow managementWindow = new ManagementWindow();
            managementWindow.Show();
            this.Close();

        }

        private void LoadData()
        {
            SalariesDAO salaryDAO = new SalariesDAO();
            List<Salaries> salaries = salaryDAO.GetSalaries();
            DataGridSalary.ItemsSource = salaries;
        }

        private void SalaryDetailButton_Click(object sender, RoutedEventArgs e)
        {

            int employeeId = (int)(DataGridSalary.SelectedItem as Salaries).EmployeeID;
            SalaryDetail salaryDetail = new SalaryDetail(employeeId);
            salaryDetail.Show();
            this.Close();

        }

        private void DataGridSalary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridSalary.SelectedItem != null)
            {
                Salaries salary = DataGridSalary.SelectedItem as Salaries;
                PaymentDate.Text = salary.PaymentDate.ToString();
                EmployeeName.Text = salary.Employees.FullName;
                BaseSalary.Text = salary.Allowance.ToString();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Salaries updated_salary = DataGridSalary.SelectedItem as Salaries;
            SalariesDAO salaryDAO = new SalariesDAO();
            Salaries salary = new Salaries();
            salary.EmployeeID = updated_salary.EmployeeID;
            salary.PaymentDate = DateOnly.Parse(PaymentDate.Text);
            salary.Allowance = double.Parse(BaseSalary.Text);
            salary.Bonus = updated_salary.Bonus;
            salary.Deduction = updated_salary.Deduction;
            salary.Year = updated_salary.Year;
            salary.Month = updated_salary.Month;
            salary.SalaryID = updated_salary.SalaryID;
            bool check = salaryDAO.UpdateSalary(salary);
            if (check)
            {
                MessageBox.Show("Update successfully");
                LoadData();
            }
            else
            {
                MessageBox.Show("Update failed");
            }
        }
    }
}

