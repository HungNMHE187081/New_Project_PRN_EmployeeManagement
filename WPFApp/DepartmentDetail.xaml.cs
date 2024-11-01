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
    /// Interaction logic for DepartmentDetail.xaml
    /// </summary>
    public partial class DepartmentDetail : Window
    {
        private int departmentID;
        public DepartmentDetail(int departmentID)
        {
            InitializeComponent();
            this.departmentID = departmentID;
            LoadData();
        }
        public void LoadData()
        {
            EmployeesDAO employeeDAO = new EmployeesDAO();
            DepartmentsDAO departmentDAO = new DepartmentsDAO();

            Departments department = departmentDAO.GetDepartmentById(departmentID);
            DepartmentName.Text = department.DepartmentName;
            int numberOfEmployee = departmentDAO.GetNumberOfEmployeeOfADepartment(departmentID);
            NumberOfEmployee.Text = numberOfEmployee.ToString();
            List<Employees> employees = employeeDAO.GetEmployeesByDepartmentId(departmentID);
            DataGridEmployee.ItemsSource = employees;
            List<Employees> unassignedEmployees = employeeDAO.GetUnassignedEmployees();
            DataGridUnasignedEmployee.ItemsSource = unassignedEmployees;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DepartmentList departmentList = new DepartmentList();
            departmentList.Show();
            this.Close();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeesDAO employeeDAO = new EmployeesDAO();
            Employees employee = (Employees)DataGridEmployee.SelectedItem;
            bool checkk = employeeDAO.RemoveEmployeeFromDepartment(employee.EmployeeID);
            if (checkk)
            {
                MessageBox.Show("Remove successfully");
                LoadData();
            }
            else
            {
                MessageBox.Show("Remove failed");
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AssignButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeesDAO employeeDAO = new EmployeesDAO();
            Employees employee = (Employees)DataGridUnasignedEmployee.SelectedItem;
            bool checkk = employeeDAO.AssignEmployeeToDepartment(employee.EmployeeID, departmentID);
            if (checkk)
            {
                MessageBox.Show("Assign successfully");
                LoadData();
            }
            else
            {
                MessageBox.Show("Assign failed");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AssignEmployee.Visibility = Visibility.Visible;
            DepartmentNameonCanvas.Text = DepartmentName.Text;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            AssignEmployee.Visibility = Visibility.Hidden;
        }

        private void DataGridUnasignedEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EditDepartment_Click(object sender, RoutedEventArgs e)
        {
            DepartmentsDAO departmentDAO = new DepartmentsDAO();
            Departments department = new Departments();
            department.DepartmentID = departmentDAO.GetMaxDepartmentId();
            department.DepartmentName = DepartmentName.Text;
            bool check = departmentDAO.UpdateDepartment(department);
            if (check)
            {
                MessageBox.Show("Update successfully");

            }
            else
            {
                MessageBox.Show("Update failed");
            }
        }
    }
}
