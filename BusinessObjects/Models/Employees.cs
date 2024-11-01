using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Employees
    {
        [Key]
        public int EmployeeID { get; set; }

        public int? UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        public DateTime BirthDate { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        public int? DepartmentID { get; set; }

        [StringLength(50)]
        public string? Position { get; set; }

        public double BaseSalary { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [StringLength(255)]
        public string? AvatarPath { get; set; }

        [ForeignKey("UserID")]
        public virtual Users? Users { get; set; }

        [ForeignKey("DepartmentID")]
        public virtual Departments? Departments { get; set; }

        public virtual ICollection<Salaries> Salaries { get; set; } = new List<Salaries>();
        public virtual ICollection<Attendances> Attendances { get; set; } = new List<Attendances>();
        public virtual ICollection<LeaveBalances> LeaveBalances { get; set; } = new List<LeaveBalances>();
    }
}