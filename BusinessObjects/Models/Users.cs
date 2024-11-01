using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = null!;

        public int? RoleID { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("RoleID")]
        public virtual Roles? Roles { get; set; }

        public virtual Employees? Employees { get; set; }
        public virtual ICollection<ActivityLogs> ActivityLogs { get; set; } = new List<ActivityLogs>();
    }
}