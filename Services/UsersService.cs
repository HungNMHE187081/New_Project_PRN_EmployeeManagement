using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;
namespace Services
{
    public class UsersService
    {
        private readonly UsersDAO _usersDAO;

        public UsersService()
        {
            _usersDAO = new UsersDAO();
        }

        public void AddUser(Users user)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                ValidateUser(user);
                _usersDAO.Add(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in UsersService.AddUser: {ex.Message}", ex);
            }
        }

        public Users? GetUserById(int id)
        {
            return _usersDAO.GetById(id);
        }

        public Users? GetUserByUsername(string username)
        {
            return _usersDAO.GetByUsername(username);
        }

        public List<Users> GetUnassignedUsers()
        {
            return _usersDAO.GetUnassignedUsers();
        }

        private void ValidateUser(Users user)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(user.Username))
                errors.Add("Username is required");

            if (string.IsNullOrWhiteSpace(user.Password))
                errors.Add("Password is required");

            if (user.RoleID <= 0)
                errors.Add("Invalid role");

            if (errors.Any())
                throw new Exception(string.Join("\n", errors));
        }
    }
}
