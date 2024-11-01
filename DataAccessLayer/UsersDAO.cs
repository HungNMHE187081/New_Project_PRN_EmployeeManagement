using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessLayer;

namespace DataAccessLayer
{
    public class UsersDAO
    {
        private readonly PRN_EmployeeManagementContext _context;

        public UsersDAO()
        {
            _context = new PRN_EmployeeManagementContext();
        }

        public void Add(Users user)
        {
            try
            {
                // Kiểm tra username đã tồn tại chưa
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    throw new Exception("Username already exists");
                }

                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding user: {ex.Message}", ex);
            }
        }

        public Users? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public Users? GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public List<Users> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public List<Users> GetUnassignedUsers()
        {
            // Lấy danh sách user chưa được gán cho employee nào
            var assignedUserIds = _context.Employees
                .Where(e => e.UserID != null)
                .Select(e => e.UserID)
                .ToList();

            return _context.Users
                .Where(u => !assignedUserIds.Contains(u.UserID))
                .ToList();
        }
    }
}
