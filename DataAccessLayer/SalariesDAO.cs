using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SalariesDAO
    {
        public List<Salaries> GetSalaries()
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                return context.Salaries.Include(o => o.Employees).ToList();
            }
        }

        public List<SalaryModification> GetSalaryModificationsOfAnEmployee(int employeeId)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                return context.SalaryModifications.Where(o => o.EmployeeId == employeeId).ToList();
            }
        }

        public double GetSalaryBonusOfAnEmployee(int employeeId)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var a = context.SalaryModifications.Where(o => o.EmployeeId == employeeId && o.Status.Equals("bonus"));
                if (a == null)
                {
                    return 0;
                }
                return context.SalaryModifications.Where(o => o.EmployeeId == employeeId && o.Status.Equals("bonus")).Sum(o => o.Amount);
            }
        }

        public double GetSalaryDeductionOfAnEmployee(int employeeId)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var a = context.SalaryModifications.Where(o => o.EmployeeId == employeeId && o.Status.Equals("deduction"));
                if (a == null)
                {
                    return 0;
                }
                return context.SalaryModifications.Where(o => o.EmployeeId == employeeId && o.Status.Equals("deduction")).Sum(o => o.Amount);
            }
        }

        public double GetSalaryAllowanceOfAnEmployee(int employeeId)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                return context.Salaries.Where(o => o.EmployeeID == employeeId).Sum(o => (double?)o.Allowance) ?? 0;
            }
        }

        public bool UpdateSalary(Salaries salary)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var s = context.Salaries.Find(salary.SalaryID);
                if (s == null)
                {
                    return false;
                }
                s.Allowance = salary.Allowance;
                s.Bonus = salary.Bonus;
                s.Deduction = salary.Deduction;
                s.PaymentDate = salary.PaymentDate;
                context.SaveChanges();
                return true;
            }
        }
    }
}
