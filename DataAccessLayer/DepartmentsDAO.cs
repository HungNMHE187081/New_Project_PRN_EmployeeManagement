using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer
{
    public class DepartmentsDAO
    {
        private readonly PRN_EmployeeManagementContext _context;
        private readonly string connectionString;

        public DepartmentsDAO()
        {
            _context = new();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            connectionString = configuration.GetConnectionString("EmployeeManagementDB");
        }

        public List<Departments> GetAll()
        {
            return _context.Departments.ToList();
        }

        public Departments GetByID(int id)
        {
            return _context.Departments.Find(id) ??
                   throw new KeyNotFoundException($"Department with ID {id} not found");
        }


        public List<DepartmentListModel> GetAllDepartments()
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var departments = context.Departments
                    .GroupJoin(
                        context.Employees,
                        d => d.DepartmentID,
                        e => e.DepartmentID,
                        (d, e) => new DepartmentListModel
                        {
                            DepartmentId = d.DepartmentID,
                            DepartmentName = d.DepartmentName,
                            EmployeeCount = e.Count()
                        }
                    )
                    .ToList();

                return departments;
            }
        }

        public Departments GetDepartmentById(int id)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                return context.Departments.Where(d => d.DepartmentID == id).FirstOrDefault();
            }
        }

        public int GetNumberOfEmployeeOfADepartment(int departmentId)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                return context.Employees.Where(e => e.DepartmentID == departmentId).Count();
            }
        }

        public bool AddDepartment(Departments department)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                context.Departments.Add(department);
                int check = context.SaveChanges();
                if (check > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool UpdateDepartment(int departmentID, string departmentName)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var d = context.Departments.Where(d => d.DepartmentID == departmentID).FirstOrDefault();
                if (d != null)
                {
                    d.DepartmentName = departmentName;
                    int check = context.SaveChanges();
                    if (check > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<DepartmentListModel> GetAllDepartmentsByName(string search_departmentName)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var departments = context.Departments
                    .Where(d => d.DepartmentName.Contains(search_departmentName))
                    .GroupJoin(
                        context.Employees,
                        d => d.DepartmentID,
                        e => e.DepartmentID,
                        (d, e) => new DepartmentListModel
                        {
                            DepartmentId = d.DepartmentID,
                            DepartmentName = d.DepartmentName,
                            EmployeeCount = e.Count()
                        }
                    )
                    .ToList();

                return departments;
            }
        }

        public bool UpdateDepartment(Departments department)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var d = context.Departments.Where(d => d.DepartmentID == department.DepartmentID).FirstOrDefault();
                if (d != null)
                {
                    d.DepartmentName = department.DepartmentName;
                    int check = context.SaveChanges();
                    if (check > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool DeleteDepartment(int departmentID)
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                var d = context.Departments.Where(d => d.DepartmentID == departmentID).FirstOrDefault();
                if (d != null)
                {
                    context.Departments.Remove(d);
                    int check = context.SaveChanges();
                    if (check > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int GetMaxDepartmentId()
        {
            using (var context = new PRN_EmployeeManagementContext())
            {
                return context.Departments.Max(d => d.DepartmentID);
            }

        }

        public bool AddDepartment(string departmentName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Departments(DepartmentName) VALUES(@DepartmentName)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@DepartmentName", departmentName);
                int check = command.ExecuteNonQuery();
                if (check > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int GetnumberOfEmployeeOfADepartment(int departmentId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT COUNT(*) FROM Employees WHERE DepartmentID = @DepartmentID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@DepartmentID", departmentId);
                return (int)command.ExecuteScalar();
            }
        }


    }
}
