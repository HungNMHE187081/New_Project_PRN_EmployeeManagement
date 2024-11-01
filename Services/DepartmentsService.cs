using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessLayer;
namespace Services
{
    public class DepartmentsService
    {
        private DepartmentsDAO _departmentsDAO = new();

        public List<Departments> GetAllDepartments() {
            return _departmentsDAO.GetAll();
        }
        public Departments GetDepartmentsByID(int id) {
            return _departmentsDAO.GetByID(id);
        }
    }
}
