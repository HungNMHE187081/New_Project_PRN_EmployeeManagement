using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class LeaveBalances
    {
        [Key]
        public int LeaveID { get; set; }

        public int? EmployeeID { get; set; }

        [Required]
        public int Year { get; set; }

        public int AnnualLeave { get; set; } = 12;

        public int SickLeave { get; set; } = 30;

        public int UnpaidLeave { get; set; } = 0;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int RemainingLeave => AnnualLeave + SickLeave - UnpaidLeave;

        [ForeignKey("EmployeeID")]
        public virtual Employees? Employees { get; set; }
    }

}