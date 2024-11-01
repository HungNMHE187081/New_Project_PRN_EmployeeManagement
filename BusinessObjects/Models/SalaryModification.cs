using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class SalaryModification
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public string Status { get; set; } = null!;

        public double Amount { get; set; }

        public int EmployeeId { get; set; }

        public string? Description { get; set; }
    }
}
