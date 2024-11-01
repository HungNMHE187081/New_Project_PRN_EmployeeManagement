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
        [Key]
        public int NotificationID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? Message { get; set; }

        public int? DepartmentID { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("DepartmentID")]
        public virtual Departments? Departments { get; set; }
    }

}