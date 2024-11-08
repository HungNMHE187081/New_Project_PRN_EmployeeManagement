using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Notifications
    {
        public int NotificationId { get; set; }

        public string Title { get; set; } = null!;

        public string? Message { get; set; }

        public int? DepartmentId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int EmployeeId { get; set; }

        public int IsApproved { get; set; }

        public virtual Departments? Department { get; set; }
    }

}