using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Drawing.Chart.ChartEx;

namespace DataAccessLayer
{
    public class EmployeesDAO
    {
        private static readonly PRN_EmployeeManagementContext _context;

        static EmployeesDAO()
        {
            _context = new PRN_EmployeeManagementContext();
        }

        public List<Employees> GetAll()
        {
            return _context.Employees.Include(e => e.Departments).ToList();
        }
        //Get By Id
        public Employees GetById(int id)
        {
            return _context.Employees.Include(e => e.Departments).FirstOrDefault(e => e.EmployeeID == id) ??
                throw new KeyNotFoundException($"Employee with ID {id} not found");
        }
        //Create
        public void Add(Employees employee)
        {
            try
            {
                // Log thông tin employee trước khi thêm
                Console.WriteLine($"Attempting to add employee: {employee.FullName}");
                Console.WriteLine($"Department ID: {employee.DepartmentID}");
                Console.WriteLine($"Birth Date: {employee.BirthDate}");
                // ... log các thông tin khác

                _context.Employees.Add(employee);
                
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateException dbEx)
                {
                    // Log inner exception chi tiết
                    var innerException = dbEx.InnerException;
                    while (innerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {innerException.Message}");
                        Console.WriteLine($"Stack Trace: {innerException.StackTrace}");
                        innerException = innerException.InnerException;
                    }
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Chi tiết lỗi khi thêm nhân viên:\n" +
                    $"Message: {ex.Message}\n" +
                    $"Stack Trace: {ex.StackTrace}\n" +
                    $"Inner Exception: {ex.InnerException?.Message}", ex);
            }
        }
        //Update
        public void Update(Employees employees)
        {
            try
            {
                var existingEmployee = _context.Employees.Find(employees.EmployeeID);
                if (existingEmployee == null)
                {
                    throw new Exception($"Employee with ID {employees.EmployeeID} not found");
                }

                // Cập nhật từng trường thay vì dùng SetValues
                existingEmployee.FullName = employees.FullName;
                existingEmployee.BirthDate = employees.BirthDate;
                existingEmployee.Gender = employees.Gender;
                existingEmployee.Address = employees.Address;
                existingEmployee.Phone = employees.Phone;
                existingEmployee.DepartmentID = employees.DepartmentID;
                existingEmployee.Position = employees.Position;
                existingEmployee.BaseSalary = employees.BaseSalary;
                existingEmployee.StartDate = employees.StartDate;
                existingEmployee.AvatarPath = employees.AvatarPath;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating employee: {ex.Message}", ex);
            }
        }
        //Delete
        public void Delete(Employees employees)
        {
            _context.Employees.Remove(employees);
            _context.SaveChanges();
        }
        //Search
        public List<Employees> Search(string searchText, string searchBy)
        {
            var query = _context.Employees.Include(e => e.Departments).AsQueryable();
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToLower();
                switch (searchBy)
                {
                    case "Full Name":
                        query = query.Where(e => e.FullName.ToLower().Contains(searchText));
                        break;
                    case "Department":
                        query = query.Where(e => e.Departments.DepartmentName.ToLower().Contains(searchText));
                        break;
                    case "Position":
                        query = query.Where(e => e.Position.ToLower().Contains(searchText));
                        break;
                }
            }
            return query.ToList();
        }
        //Filter
        public List<Employees> Filter(int? departmentId, string gender, double? minSalary, double? maxSalary)
        {
            var query = _context.Employees.Include(e => e.Departments).AsQueryable();

            if (departmentId.HasValue && departmentId > 0)
            {
                query = query.Where(e => e.DepartmentID == departmentId);
            }

            if (!string.IsNullOrEmpty(gender) && gender != "All")
            {
                query = query.Where(e => e.Gender == gender);
            }

            if (minSalary.HasValue)
            {
                query = query.Where(e => e.BaseSalary >= minSalary);
            }

            if (maxSalary.HasValue)
            {
                query = query.Where(e => e.BaseSalary <= maxSalary);
            }

            return query.ToList();
        }
    }
}
