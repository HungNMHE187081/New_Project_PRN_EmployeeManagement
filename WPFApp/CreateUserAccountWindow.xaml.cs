using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BusinessObjects.Models;
using Services;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for CreateUserAccountWindow.xaml
    /// </summary>
    public partial class CreateUserAccountWindow : Window
    {
        private readonly UsersService _usersService;
        private const int EMPLOYEE_ROLE_ID = 2; // ID của role Employee trong database

        public Users? CreatedUser { get; private set; }

        public CreateUserAccountWindow()
        {
            InitializeComponent();
            _usersService = new UsersService();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                var user = new Users
                {
                    Username = TextBoxUsername.Text.Trim(),
                    Password = PasswordBoxPassword.Password,
                    RoleID = EMPLOYEE_ROLE_ID, // Gán cứng role là Employee
                    IsActive = true
                };

                _usersService.AddUser(user);
                CreatedUser = user;
                
                MessageBox.Show("Employee account created successfully!", "Success", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating user account: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(TextBoxUsername.Text))
            {
                MessageBox.Show("Please enter username", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBoxUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(PasswordBoxPassword.Password))
            {
                MessageBox.Show("Please enter password", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                PasswordBoxPassword.Focus();
                return false;
            }

            return true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
