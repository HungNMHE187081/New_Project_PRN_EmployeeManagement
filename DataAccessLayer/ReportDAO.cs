using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ReportDAO
    {
        List<Employees> list = new List<Employees>();

        public static List<dynamic> GetReports()
        {

            using (var db = new PRN_EmployeeManagementContext())
            {
                //employees = db.Employees.ToList();
                var combinestats = db.Employees
                .GroupBy(e => new { e.Position, e.Gender, e.Departments.DepartmentName })
                .Select(g => new
                {
                    Departments = g.Key.DepartmentName,
                    Position = g.Key.Position,
                    Gender = g.Key.Gender,
                    Count = g.Count()
                })
                .ToList<dynamic>();

                return combinestats;
            }
        }
    }
}
