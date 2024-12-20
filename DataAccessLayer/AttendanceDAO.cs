﻿using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    CheckIn = TimeOnly.FromDateTime(DateTime.Now),
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

        public List<EmployeeAttendance> GetAllEmployeesWithAttendance(DateOnly date)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var result = context.Employees
                    .GroupJoin(
                        context.Attendance.Where(o => o.Date==date),
                        employee => employee.EmployeeID,
                        attendance => attendance.EmployeeID,
                        (employee, attendance) => new { employee, attendance }
                    )
                    .SelectMany(
                        x => x.attendance.DefaultIfEmpty(),
                        (x, attendance) => new EmployeeAttendance
                        {
                            EmployeeId = x.employee.EmployeeID,
                            EmployeeName = x.employee.FullName,
                            CheckIn = attendance != null ? attendance.CheckIn : null,
                            CheckOut = attendance != null ? attendance.CheckOut : null,
                            Date = DateOnly.FromDateTime(DateTime.Now),
                            Status = attendance != null ? attendance.Status : "Absent"
                        }
                    )
                    .ToList();

                return result;
            }
        }

        public List<EmployeeAttendance> GetCheckInLateEmployees(DateOnly date)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var result = context.Attendance
                    .Where(a => a.CheckIn != null && a.CheckIn.Value.Hour > 8)
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

        public List<EmployeeAttendance> GetCheckoutSoonEmployee(DateOnly date)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var result = context.Attendance
                    .Where(a => a.CheckOut != null && a.CheckOut.Value.Hour < 17)
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

        public List<EmployeeAttendance> GetCheckoutLateEmployee(DateOnly date)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var result = context.Attendance
                    .Where(a => a.CheckOut != null && a.CheckOut.Value.Hour > 17)
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

        public List<EmployeeAttendance> GetAbsentEmployee(DateOnly date)
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
                            Date = date,
                            Status = attendance != null ? attendance.Status : "Absent"
                        }
                    ).Where(a => a.Status == "Absent" && a.Date == date)
                    .ToList();

                return result;
            }
        }


        public List<EmployeeAttendance> GetEmployeeAttendancesInADay(DateOnly day)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var result = context.Employees
                    .GroupJoin(
                        context.Attendance.Where(a => a.Date == day),
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
                            Date = DateOnly.FromDateTime(DateTime.Now),
                            Status = attendance != null ? attendance.Status : "Absent"
                        }
                    )
                    
                    .ToList();

                return result;
            }
        }

        public bool CheckOut(int employeeId)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var attendance = context.Attendance
                    .Where(a => a.EmployeeID == employeeId && a.Date == DateOnly.FromDateTime(DateTime.Now))
                    .FirstOrDefault();

                if (attendance == null)
                {
                    return false;
                }
                else
                {
                    attendance.CheckOut = TimeOnly.FromDateTime(DateTime.Now);
                }
                int check = context.SaveChanges();
                return true;
            }
        }

        public bool CheckOutNotInTime(int employeeId)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                CheckOut(employeeId);
                SalaryModification salaryModification = new SalaryModification();
                TimeOnly checkout = new TimeOnly(17, 0);
                if (TimeOnly.FromDateTime(DateTime.Now).Hour < 17)
                {
                    int differences = (int)(checkout - TimeOnly.FromDateTime(DateTime.Now)).TotalMinutes;
                    String late = differences.ToString();
                    salaryModification.EmployeeId = employeeId;
                    salaryModification.Date = DateOnly.FromDateTime(DateTime.Now);
                    salaryModification.Amount = double.Parse(late);
                    salaryModification.Status = "deduction";
                    salaryModification.Description = "Checkout soon " + late + " minutes";
                    context.SalaryModifications.Add(salaryModification);
                    int check = context.SaveChanges();
                    if (check > 0)
                    {
                        return true;
                    }
                }

                if (TimeOnly.FromDateTime(DateTime.Now).Hour > 17)
                {
                    int differences = (int)(TimeOnly.FromDateTime(DateTime.Now)-checkout).TotalMinutes;
                    String late = differences.ToString();
                    salaryModification.EmployeeId = employeeId;
                    salaryModification.Date = DateOnly.FromDateTime(DateTime.Now);
                    salaryModification.Amount = double.Parse(late);
                    salaryModification.Status = "bonus";
                    salaryModification.Description = "Checkout late " + late + " minutes";
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
                CheckIn(employeeId);
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
                    salaryModification.Description = "Checkin late " + late + " minutes";
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
                    salaryModification.Description = "Checkin soon " + late + " minutes";
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

        public List<DateOnly> GetAllAbsenceOfAnEmployee(int employeeId)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var result = context.Attendance
                    .Where(a => a.EmployeeID == employeeId && a.Status == "Absent")
                    .Select(a => a.Date)
                    .ToList();

                return result;
            }
        }

        public bool AddPermissionAttendance(int employeeId, DateOnly date)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var attendance = new Attendances
                {
                    EmployeeID = employeeId,
                    Date = date,
                    Status = "Permission"
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
    }
}
