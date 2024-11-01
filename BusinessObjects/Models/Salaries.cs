using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Salaries
    {
        [Key]
        public int SalaryID { get; set; }

        public int? EmployeeID { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        public double Allowance { get; set; } = 0;

        public double Bonus { get; set; } = 0;

        public double Deduction { get; set; } = 0;

        public DateOnly? PaymentDate { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employees? Employees { get; set; }
    }
}
