using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace DataAccessLayer
{
    public class DepartmentsDAO
    {
        private readonly PRN_EmployeeManagementContext _context;

        public DepartmentsDAO()
        {
            _context = new();
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
    }
}
