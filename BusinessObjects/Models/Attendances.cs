using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Attendances
    {
        [Key]
        public int AttendanceID { get; set; }

        public int? EmployeeID { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        public TimeOnly? CheckIn { get; set; }

        public TimeOnly? CheckOut { get; set; }

        [StringLength(20)]
        public string? Status { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employees? Employees { get; set; }
    }

}
