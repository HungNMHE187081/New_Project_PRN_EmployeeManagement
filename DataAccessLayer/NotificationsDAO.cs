using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class NotificationsDAO
    {
        public bool AddNotification(Notifications notification)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                context.Notifications.Add(notification);
                return context.SaveChanges() > 0;
            }
        }

        public List<Notifications> GetAllApprovedNotificationsOfAnEmployee(int employee_id) { 
            using(var context = new PRN_EmployeeManagementContext())
            {
                return context.Notifications.Where(n => n.EmployeeId == employee_id && (n.IsApproved == 1||n.IsApproved==2)).ToList();
            }
        }

        public List<Notifications> GetAllUnApprovedNotificationsOfAnEmployee(int employee_id)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                return context.Notifications.Where(n => n.EmployeeId == employee_id && n.IsApproved == 0).ToList();
            }
        }

        public List<Notifications> GetAllNotifications()
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                return context.Notifications.Where(o => o.IsApproved==0).ToList();
            }
        }

        public List<Notifications> GetAllNotificationsByTitle(string title)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                return context.Notifications.Where(o => o.Title.Contains(title)&&o.IsApproved==0).ToList();
            }
        }

        public bool ApproveNotification(int notification_id, string message)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var notification = context.Notifications.Find(notification_id);
                if (notification == null)
                {
                    return false;
                }
                notification.IsApproved = 1;
                notification.Message = message;
                context.SaveChanges();
                return true;
            }
        }

        public bool RejectNotification(int notification_id, string message)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var notification = context.Notifications.Find(notification_id);
                if (notification == null)
                {
                    return false;
                }
                notification.IsApproved = 2;
                notification.Message = message;
                context.SaveChanges();
                return true;
            }
        }

        public bool RemoveNotification(int notification_id)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var notification = context.Notifications.Find(notification_id);
                if (notification == null)
                {
                    return false;
                }
                context.Notifications.Remove(notification);
                context.SaveChanges();
                return true;
            }
        }
    }
}
