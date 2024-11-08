using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public sealed class AttendanceRequestState
    {
        private static AttendanceRequestState? _instance;
        private static readonly object _lock = new object();

        public DateTime? ExpectedDate { get; set; }
        public string? Message { get; set; }

        // Private constructor to prevent instantiation
        private AttendanceRequestState() { }

        // Public method to get the single instance of the class
        public static AttendanceRequestState Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AttendanceRequestState();
                    }
                    return _instance;
                }
            }
        }
    }
}
