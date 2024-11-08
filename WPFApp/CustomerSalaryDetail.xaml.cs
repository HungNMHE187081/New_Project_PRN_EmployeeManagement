using BusinessObjects.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for CustomerSalaryDetail.xaml
    /// </summary>
    public partial class CustomerSalaryDetail : Window
    {
        private int employee_id;
        public CustomerSalaryDetail(int employee_id)
        {
            InitializeComponent();
            this.employee_id = employee_id;
            var state = EmployeeSalaryRequestState.Instance;
            if (state.ExpectedDate != null)
            {
                ExpectedDate.SelectedDate = state.ExpectedDate;
                Message.Text = state.Message;
            }
            LoadData();
        }

        private void LoadData()
        {
            SalariesDAO salariesDAO = new SalariesDAO();
            Salaries salaries = salariesDAO.GetSalariesByEmployeeId( employee_id);
            BaseSalary.Text = salaries.Allowance.ToString();
            Duedate.Text = salaries.PaymentDate.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            
            SalariesDAO salaryDAO = new SalariesDAO();
            Salaries updated_salary = salaryDAO.GetSalariesByEmployeeId(employee_id);
            NotificationsDAO notificationsDAO = new NotificationsDAO();
            Notifications notification = new Notifications();
            notification.EmployeeId = employee_id;
            notification.CreatedDate = DateTime.Parse(ExpectedDate.Text);
            notification.Title = "Update Payment Date for receiving Salary";
            notification.Message = Message.Text;
            notification.IsApproved = 0;
            bool check = notificationsDAO.AddNotification(notification);
            if (check)
            {
                var state = EmployeeSalaryRequestState.Instance;
                state.ExpectedDate = null;
                state.Message = null;
                ExpectedDate.Text = "";
                Message.Text = "";
                MessageBox.Show("Request successfully");
                LoadData();
            }
            else
            {
                MessageBox.Show("Request failed");
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var state = EmployeeSalaryRequestState.Instance;
            state.ExpectedDate = ExpectedDate.SelectedDate;
            state.Message = Message.Text;
            CustomerWindow customerWindow = new CustomerWindow(employee_id);
            customerWindow.Show();
            this.Close();
        }

        private void ApproveConfirmation(object sender, RoutedEventArgs e)
        {
            ConfirmBox.Visibility = Visibility.Hidden;
            Save_Click(sender, e);

        }

        private void DisapproveConfirmation(object sender, RoutedEventArgs e)
        {
            ConfirmBox.Visibility = Visibility.Hidden;
        }

        // ...

        private void Request(object sender, RoutedEventArgs e)
        {
            if (ExpectedDate.Text == "" && Message.Text == "")
            {
                MessageBox.Show("Please fill all the fields");
                return;
            }
            
            MessageConfirm.Text = "Are you sure to request for changing payment information?";
            ConfirmBox.Visibility = Visibility.Visible;
        }
    }
}
