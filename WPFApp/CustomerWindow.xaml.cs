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

        public CustomerWindow()
        {
            InitializeComponent();
            _context = new PRN_EmployeeManagementContext();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            dgEmployees.ItemsSource = _context.Employees.ToList();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var employee = new Employees
            {
                FullName = txtFullName.Text,
                BirthDate = dpBirthDate.SelectedDate.Value
                // Initialize other properties as needed
            };

            _context.Employees.Add(employee);
            _context.SaveChanges();
            LoadEmployees();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (dgEmployees.SelectedItem is Employees selectedEmployee)
            {
                selectedEmployee.FullName = txtFullName.Text;
                selectedEmployee.BirthDate = dpBirthDate.SelectedDate.Value;
                // Update other properties as needed

                _context.SaveChanges();
                LoadEmployees();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgEmployees.SelectedItem is Employees selectedEmployee)
            {
                _context.Employees.Remove(selectedEmployee);
                _context.SaveChanges();
                LoadEmployees();
            }
        }
    }
}