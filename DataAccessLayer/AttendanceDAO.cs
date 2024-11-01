using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AttendanceDAO
    {
        public bool CheckIn(int employeeId)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var attendance = new Attendances
                {
                    EmployeeID = employeeId,
                    Date = DateTime.Now,
                    CheckIn = DateTime.Now.TimeOfDay,
                    Status = "Present"
                };

                context.Attendance.Add(attendance);
                int check = context.SaveChanges();
                if (check > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public List<EmployeeAttendance> GetAllEmployeesWithAttendance(DateTime date)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var result = context.Employees
                    .GroupJoin(
                        context.Attendance,
                        employee => employee.EmployeeID,
                        attendance => attendance.EmployeeID,
                        (employee, attendances) => new { employee, attendances }
                    )
                    .SelectMany(
                        x => x.attendances.DefaultIfEmpty(),
                        (x, attendance) => new EmployeeAttendance
                        {
                            EmployeeId = x.employee.EmployeeID,
                            EmployeeName = x.employee.FullName,
                            CheckIn = attendance != null ? attendance.CheckIn : null,
                            CheckOut = attendance != null ? attendance.CheckOut : null,
                            Date = attendance != null ? attendance.Date : DateTime.Now,
                            Status = attendance != null ? attendance.Status : "Absent"
                        }
                    )
                    .ToList();

                return result;
            }
        }

        public List<EmployeeAttendance> GetCheckInLateEmployees(DateTime date)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var result = context.Attendance
                    .Where(a => a.CheckIn != null && a.CheckIn.Value.Hours > 8)
                    .Select(a => new EmployeeAttendance
                    {
                        EmployeeId = (int)a.EmployeeID,
                        EmployeeName = a.Employees.FullName,
                        CheckIn = a.CheckIn,
                        CheckOut = a.CheckOut,
                        Date = a.Date,
                        Status = a.Status
                    }).Where(a => a.Date == date)
                    .ToList();

                return result;
            }
        }

        public List<EmployeeAttendance> GetCheckoutSoonEmployee(DateTime date)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var result = context.Attendance
                    .Where(a => a.CheckOut != null && a.CheckOut.Value.Hours < 17)
                    .Select(a => new EmployeeAttendance
                    {
                        EmployeeId = (int)a.EmployeeID,
                        EmployeeName = a.Employees.FullName,
                        CheckIn = a.CheckIn,
                        CheckOut = a.CheckOut,
                        Date = a.Date,
                        Status = a.Status
                    }).Where(a => a.Date == date)
                    .ToList();

                return result;
            }
        }

        public List<EmployeeAttendance> GetCheckoutLateEmployee(DateTime date)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var result = context.Attendance
                    .Where(a => a.CheckOut != null && a.CheckOut.Value.Hours > 17)
                    .Select(a => new EmployeeAttendance
                    {
                        EmployeeId = (int)a.EmployeeID,
                        EmployeeName = a.Employees.FullName,
                        CheckIn = a.CheckIn,
                        CheckOut = a.CheckOut,
                        Date = a.Date,
                        Status = a.Status
                    }).Where(a => a.Date == date)
                    .ToList();

                return result;
            }
        }

        public List<EmployeeAttendance> GetAbsentEmployee(DateTime date)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var result = context.Employees
                    .GroupJoin(
                        context.Attendance,
                        employee => employee.EmployeeID,
                        attendance => attendance.EmployeeID,
                        (employee, attendances) => new { employee, attendances }
                    )
                    .SelectMany(
                        x => x.attendances.DefaultIfEmpty(),
                        (x, attendance) => new EmployeeAttendance
                        {
                            EmployeeId = x.employee.EmployeeID,
                            EmployeeName = x.employee.FullName,
                            CheckIn = attendance != null ? attendance.CheckIn : null,
                            CheckOut = attendance != null ? attendance.CheckOut : null,
                            Date = attendance != null ? attendance.Date : DateTime.Now,
                            Status = attendance != null ? attendance.Status : "Absent"
                        }
                    ).Where(a => a.Status == "Absent" && a.Date == date)
                    .ToList();

                return result;
            }
        }

        public List<EmployeeAttendance> GetEmployeeWithAttendanceInAday(DateTime day)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var result = context.Employees
                    .GroupJoin(
                        context.Attendance,
                        employee => employee.EmployeeID,
                        attendance => attendance.EmployeeID,
                        (employee, attendances) => new { employee, attendances }
                    )
                    .SelectMany(
                        x => x.attendances.DefaultIfEmpty(),
                        (x, attendance) => new EmployeeAttendance
                        {
                            EmployeeId = x.employee.EmployeeID,
                            EmployeeName = x.employee.FullName,
                            CheckIn = attendance != null ? attendance.CheckIn : null,
                            CheckOut = attendance != null ? attendance.CheckOut : null,
                            Date = attendance != null ? attendance.Date : DateTime.Now,
                            Status = attendance != null ? attendance.Status : "Absent"
                        }
                    ).Where(DateOnly => DateOnly.Date == day)
                    .ToList();

                return result;
            }
        }

        public List<EmployeeAttendance> GetEmployeeAttendancesInADay(DateTime day)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var result = context.Employees
                    .GroupJoin(
                        context.Attendance,
                        employee => employee.EmployeeID,
                        attendance => attendance.EmployeeID,
                        (employee, attendances) => new { employee, attendances }
                    )
                    .SelectMany(
                        x => x.attendances.DefaultIfEmpty(),
                        (x, attendance) => new EmployeeAttendance
                        {
                            EmployeeId = x.employee.EmployeeID,
                            EmployeeName = x.employee.FullName,
                            CheckIn = attendance != null ? attendance.CheckIn : null,
                            CheckOut = attendance != null ? attendance.CheckOut : null,
                            Date = attendance != null ? attendance.Date : DateTime.Now,
                            Status = attendance != null ? attendance.Status : "Absent"
                        }
                    )
                    .Where(a => a.Date == day)
                    .ToList();

                return result;
            }
        }

        public bool CheckOut(int employeeId)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var attendance = context.Attendance
                    .Where(a => a.EmployeeID == employeeId && a.Date == DateTime.Now)
                    .FirstOrDefault();

                if (attendance == null)
                {
                    return false;
                }
                else
                {
                    attendance.CheckOut = DateTime.Now.TimeOfDay;
                }
                int check = context.SaveChanges();
                return true;
            }
        }

        public bool CheckOutNotInTime(int employeeId)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                SalaryModification salaryModification = new SalaryModification();
                TimeOnly checkout = new TimeOnly(17, 0);
                if (TimeOnly.FromDateTime(DateTime.Now).Hour > 17)
                {
                    int differences = (int)(TimeOnly.FromDateTime(DateTime.Now) - checkout).TotalMinutes;
                    String late = differences.ToString();
                    salaryModification.EmployeeId = employeeId;
                    salaryModification.Date = DateOnly.FromDateTime(DateTime.Now);
                    salaryModification.Amount = double.Parse(late);
                    salaryModification.Status = "deduction";
                    salaryModification.Description = "CheckOut soon " + late + " minutes";
                    context.SalaryModifications.Add(salaryModification);
                    int check = context.SaveChanges();
                    if (check > 0)
                    {
                        return true;
                    }
                }

                if (TimeOnly.FromDateTime(DateTime.Now).Hour < 17)
                {
                    int differences = (int)(checkout - TimeOnly.FromDateTime(DateTime.Now)).TotalMinutes;
                    String late = differences.ToString();
                    salaryModification.EmployeeId = employeeId;
                    salaryModification.Date = DateOnly.FromDateTime(DateTime.Now);
                    salaryModification.Amount = double.Parse(late);
                    salaryModification.Status = "bonus";
                    salaryModification.Description = "CheckOut late " + late + " minutes";
                    context.SalaryModifications.Add(salaryModification);
                    int check = context.SaveChanges();
                    if (check > 0)
                    {
                        return true;
                    }
                }
                return false;
            }

        }

        public bool CheckInNotInTime(int employeeId)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                SalaryModification salaryModification = new SalaryModification();
                TimeOnly checkin = new TimeOnly(8, 0);
                if (TimeOnly.FromDateTime(DateTime.Now).Hour > 8)
                {
                    int differences = (int)(TimeOnly.FromDateTime(DateTime.Now) - checkin).TotalMinutes;
                    String late = differences.ToString();
                    salaryModification.EmployeeId = employeeId;
                    salaryModification.Date = DateOnly.FromDateTime(DateTime.Now);
                    salaryModification.Amount = double.Parse(late);
                    salaryModification.Status = "deduction";
                    salaryModification.Description = "CheckIn late " + late + " minutes";
                    context.SalaryModifications.Add(salaryModification);
                    int check = context.SaveChanges();
                    if (check > 0)
                    {
                        return true;
                    }
                }

                if (TimeOnly.FromDateTime(DateTime.Now).Hour < 8)
                {
                    int differences = (int)(checkin - TimeOnly.FromDateTime(DateTime.Now)).TotalMinutes;
                    String late = differences.ToString();
                    salaryModification.EmployeeId = employeeId;
                    salaryModification.Date = DateOnly.FromDateTime(DateTime.Now);
                    salaryModification.Amount = double.Parse(late);
                    salaryModification.Status = "bonus";
                    salaryModification.Description = "CheckIn soon " + late + " minutes";
                    context.SalaryModifications.Add(salaryModification);
                    int check = context.SaveChanges();
                    if (check > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
