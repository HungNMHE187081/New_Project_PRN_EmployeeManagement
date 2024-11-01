using System;
using System.Linq;
using System.Windows;
using DataAccessLayer; // Thêm namespace cho DbContext
using BusinessObjects.Models; // Thêm namespace cho models
using Microsoft.EntityFrameworkCore;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
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
                        if (user.RoleID == 1)
                        {
                            this.Hide();
                            EmployeesManagementWindow employeesWindow = new EmployeesManagementWindow();
                            employeesWindow.Closed += (s, args) => this.Close();
                            employeesWindow.Show();
                        }
                        else if (user.RoleID == 2)
                        {
                            this.Hide();
                            CustomerWindow customerWindow = new CustomerWindow();
                            customerWindow.Closed += (s, args) => this.Close();
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
