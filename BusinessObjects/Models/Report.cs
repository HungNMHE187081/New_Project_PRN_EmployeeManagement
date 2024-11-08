using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Report
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string DepartmentName { get; set; }
        //public double BaseSalary { get; set; }
        //public double Allowance { get; set; }
        public double MonthlySalary { get; set; }
        //public double QuarterlySalary { get; set; }
        public int AnnualLeave { get; set; } = 12;
        public int SickLeave { get; set; } = 30;
        public int UnpaidLeave { get; set; } = 0;
        public int RemainingLeave { get; set; }
    }
}
