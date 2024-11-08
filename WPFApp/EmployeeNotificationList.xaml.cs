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
    /// Interaction logic for EmployeeNotificationList.xaml
    /// </summary>
    public partial class EmployeeNotificationList : Window
    {
        private int employee_id;
        private int delete_id;
        public EmployeeNotificationList(int empoyee_id)
        {
            InitializeComponent();
            this.employee_id = empoyee_id;
            LoadData();
        }
        private void LoadData()
        {
            NotificationsDAO notificationsDAO = new NotificationsDAO();
            List<Notifications> approved_notifications = notificationsDAO.GetAllApprovedNotificationsOfAnEmployee(employee_id);
            DataGridApprovedNotifications.ItemsSource = approved_notifications;
            List<Notifications> unapproved_notifications = notificationsDAO.GetAllUnApprovedNotificationsOfAnEmployee(employee_id);
            DataGridUnApprovedNotifications.ItemsSource = unapproved_notifications;

        }

        public void HandledNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            NotificationsDAO notificationsDAO = new NotificationsDAO();
            notificationsDAO.RemoveNotification(delete_id);
            LoadData();
        }

        public void NotHandledNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            NotificationsDAO notificationsDAO = new NotificationsDAO();
            notificationsDAO.RemoveNotification(delete_id);
            LoadData();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow customerWindow = new CustomerWindow(employee_id);
            customerWindow.Show();
            this.Close();
        }

        private void ApproveConfirmation(object sender, RoutedEventArgs e)
        {
            ConfirmBox.Visibility = Visibility.Hidden;
            if (MessageConfirm.Text == "Are you sure to proceed?")
            {
                HandledNotificationButton_Click(sender, e);
            }
            else
            {
                NotHandledNotificationButton_Click(sender, e);
            }
        }

        private void DisapproveConfirmation(object sender, RoutedEventArgs e)
        {
            ConfirmBox.Visibility = Visibility.Hidden;
        }

        private void OpenBoxForNotHandel(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Notifications notification = (Notifications)button.DataContext;
            delete_id = notification.NotificationId;
            MessageConfirm.Text = "Are you sure. This is not handled yet?";
            ConfirmBox.Visibility = Visibility.Visible;
        }
        private void OpenBoxForHandle(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Notifications notification = (Notifications)button.DataContext;
            delete_id = notification.NotificationId;
            MessageConfirm.Text = "Are you sure to proceed?";
            ConfirmBox.Visibility = Visibility.Visible;
        }
    }
}
