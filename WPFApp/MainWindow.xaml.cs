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
                    Users user = context.Users
                        .FirstOrDefault(u => u.Username == username && u.Password == password);

                    if (user != null)
                    {
                        // Kiểm tra vai trò
                        if (user.RoleID == 1)
                        {
                            ManagementWindow managementWindow = new ManagementWindow();
                            managementWindow.Show();
                            this.Close();
                        }
                        else if (user.RoleID == 2)
                        {
                            int employeeId = user.UserID;
                            CustomerWindow customerWindow = new CustomerWindow(employeeId);
                            customerWindow.Show();
                            this.Close();
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