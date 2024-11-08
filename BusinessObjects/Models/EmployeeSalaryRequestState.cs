using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public sealed class EmployeeSalaryRequestState
    {
        private static EmployeeSalaryRequestState? _instance;
        private static readonly object _lock = new object();

        public DateTime? ExpectedDate { get; set; }
        public string? Message { get; set; }

        // Private constructor to prevent instantiation
        private EmployeeSalaryRequestState() { }

        // Public method to get the single instance of the class
        public static EmployeeSalaryRequestState Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new EmployeeSalaryRequestState();
                    }
                    return _instance;
                }
            }
        }
    }
}
