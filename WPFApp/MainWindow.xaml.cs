using System;
using System.Linq;
using System.Windows;
using DataAccessLayer; // Thêm namespace cho DbContext
using BusinessObjects.Models; // Thêm namespace cho models

namespace WPFApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = txtUserName.Text;
                string password = txtPassWord.Password;

                using (var context = new PRN_EmployeeManagementContext())
                {
                    var user = context.Users
                        .FirstOrDefault(u => u.Username == username && u.Password == password);

                    if (user != null)
                    {
                        // Kiểm tra vai trò
                        if (user.RoleID == 1)
                        {
                            this.Hide();
                            EmployeesManagementWindow managementWindow = new EmployeesManagementWindow();
                            managementWindow.Show();
                        }
                        else if (user.RoleID == 2)
                        {
                            this.Hide();
                            CustomerWindow customerWindow = new CustomerWindow(user.Employees.EmployeeID);
                            customerWindow.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Thông tin tài khoản không chính xác. Vui lòng thử lại.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}