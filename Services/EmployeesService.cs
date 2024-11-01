using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessLayer;
namespace Services
{
    public class EmployeesService
    {
        private readonly EmployeesDAO _employeesDAO;
        public EmployeesService()
        {
            _employeesDAO = new EmployeesDAO();
        }
        public List<Employees> GetAllEmployees()
        {
            return _employeesDAO.GetAll();
        }
        public Employees GetEmployeeById(int id)
        {
            return _employeesDAO.GetById(id);
        }
        public void AddEmployee(Employees employee)
        {
            try
            {
                if (employee == null)
                    throw new ArgumentNullException(nameof(employee));

                // Validate dữ liệu trước khi thêm
                ValidateEmployee(employee);

                // Đảm bảo EmployeeID = 0 khi thêm mới
                employee.EmployeeID = 0;

                // Kiểm tra và gán giá trị mặc định cho các trường có thể null
                employee.Address ??= string.Empty;
                employee.Phone ??= string.Empty;
                employee.Position ??= string.Empty;
                employee.AvatarPath ??= string.Empty;

                _employeesDAO.Add(employee);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in EmployeesService.AddEmployee: {ex.Message}", ex);
            }
        }

        private void ValidateEmployee(Employees employee)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(employee.FullName))
                errors.Add("Full name is required");

            if (employee.BirthDate == default)
                errors.Add("Birth date is required");

            if (string.IsNullOrWhiteSpace(employee.Gender))
                errors.Add("Gender is required");

            if (employee.DepartmentID <= 0)
                errors.Add("Invalid department");

            if (employee.StartDate == default)
                errors.Add("Start date is required");

            if (employee.BaseSalary < 0)
                errors.Add("Salary cannot be negative");

            if (errors.Any())
                throw new Exception(string.Join("\n", errors));
        }

        public void UpdateEmployee(Employees employee)
        {
            try
            {
                if (employee == null)
                    throw new ArgumentNullException(nameof(employee));

                _employeesDAO.Update(employee);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in EmployeesService.UpdateEmployee: {ex.Message}", ex);
            }
        }
        public void DeleteEmployee(Employees employee)
        {
            _employeesDAO.Delete(employee);
        }
        public List<Employees> SearchEmployees(string searchText, string searchBy)
        {
            return _employeesDAO.Search(searchText, searchBy);
        }
        public List<Employees> FilterEmployees(int? departmentId, string gender, double? minSalary, double? maxSalary)
        {
            return _employeesDAO.Filter(departmentId, gender, minSalary, maxSalary);
        }
    }
}
