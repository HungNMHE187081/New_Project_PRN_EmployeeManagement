using System;
using System.Collections.Generic;
using System.IO;
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
using BusinessObjects.Models;
using Microsoft.Win32;
using Services;
using Path = System.Windows.Shapes.Path;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for EmployeeDetailWindow.xaml
    /// </summary>
    public partial class EmployeeDetailWindow : Window
    {
        private readonly EmployeesService _employeesService;
        private readonly DepartmentsService _departmentsService;
        private readonly UsersService _usersService;
        private readonly Employees? _employee;
        private string? _selectedImagePath;
        private bool _isEditMode;
        public EmployeeDetailWindow(Employees? employee = null)
        {
            InitializeComponent();
            _employeesService = new EmployeesService();
            _departmentsService = new DepartmentsService();
            _usersService = new UsersService();
            _employee = employee;
            _isEditMode = employee != null;

            LoadInitialData();
        }

        private void LoadInitialData()
        {
            LoadDepartments();
            LoadUnassignedUsers();

            if (_isEditMode && _employee != null)
            {
                LoadEmployeeData();
            }
            else
            {
                DatePickerBirthDate.SelectedDate = DateTime.Now.AddYears(-20);
                DatePickerStartDate.SelectedDate = DateTime.Now;
            }
        }
        private void LoadDepartments()
        {
            try
            {
                var departments = _departmentsService.GetAllDepartments();
                ComboBoxDepartment.ItemsSource = departments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách phòng ban: {ex.Message}");
            }
        }
        private void LoadEmployeeData()
        {
            if (_employee == null) return;

            TextBoxFullName.Text = _employee.FullName;
            DatePickerBirthDate.SelectedDate = _employee.BirthDate;

            if (!string.IsNullOrEmpty(_employee.Gender))
            {
                ComboBoxGender.SelectedItem = ComboBoxGender.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == _employee.Gender);
            }

            TextBoxAddress.Text = _employee.Address;
            TextBoxPhone.Text = _employee.Phone;
            ComboBoxDepartment.SelectedValue = _employee.DepartmentID;
            TextBoxPosition.Text = _employee.Position;
            TextBoxBaseSalary.Text = _employee.BaseSalary.ToString("N0");
            DatePickerStartDate.SelectedDate = _employee.StartDate;

            if (!string.IsNullOrEmpty(_employee.AvatarPath) && File.Exists(_employee.AvatarPath))
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(_employee.AvatarPath);
                    bitmap.EndInit();
                    ImgAvatar.Source = bitmap;
                    _selectedImagePath = _employee.AvatarPath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể tải ảnh: {ex.Message}", "Cảnh báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void LoadUnassignedUsers()
        {
            try
            {
                var users = _usersService.GetUnassignedUsers();
                ComboBoxUser.ItemsSource = users;
                ComboBoxUser.DisplayMemberPath = "Username";
                ComboBoxUser.SelectedValuePath = "UserID";

                if (_isEditMode && _employee?.UserID != null)
                {
                    var currentUser = _usersService.GetUserById(_employee.UserID.Value);
                    if (currentUser != null)
                    {
                        users.Add(currentUser);
                        ComboBoxUser.SelectedValue = currentUser.UserID;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}");
            }
        }

        private void ButtonUploadImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                    ImgAvatar.Source = bitmap;
                    _selectedImagePath = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải ảnh: {ex.Message}");
                }
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var employee = _isEditMode ? 
                    new Employees { EmployeeID = _employee!.EmployeeID } : 
                    new Employees();

                employee.FullName = TextBoxFullName.Text.Trim();
                employee.BirthDate = DatePickerBirthDate.SelectedDate!.Value;
                employee.Gender = (ComboBoxGender.SelectedItem as ComboBoxItem)?.Content.ToString();
                employee.Address = TextBoxAddress.Text?.Trim();
                employee.Phone = TextBoxPhone.Text?.Trim();
                employee.DepartmentID = (int)ComboBoxDepartment.SelectedValue;
                employee.Position = TextBoxPosition.Text?.Trim();
                employee.BaseSalary = double.Parse(TextBoxBaseSalary.Text.Replace(",", ""));
                employee.StartDate = DatePickerStartDate.SelectedDate!.Value;
                employee.UserID = (int?)ComboBoxUser.SelectedValue;

                if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    try
                    {
                        var fileName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(_selectedImagePath)}";
                        var imagesFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

                        if (!Directory.Exists(imagesFolder))
                        {
                            Directory.CreateDirectory(imagesFolder);
                        }

                        var destinationPath = System.IO.Path.Combine(imagesFolder, fileName);
                        File.Copy(_selectedImagePath, destinationPath, true);
                        employee.AvatarPath = destinationPath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi lưu ảnh: {ex.Message}. Tiếp tục lưu thông tin nhân viên.");
                        employee.AvatarPath = string.Empty;
                    }
                }
                else
                {
                    employee.AvatarPath = string.Empty;
                }

                if (_isEditMode)
                {
                    _employeesService.UpdateEmployee(employee);
                    MessageBox.Show("Employee updated successfully!");
                }
                else
                {
                    _employeesService.AddEmployee(employee);
                    MessageBox.Show("Employee added successfully!");
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving employee: {ex.Message}");
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(TextBoxFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBoxFullName.Focus();
                return false;
            }

            if (!DatePickerBirthDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn ngày sinh!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                DatePickerBirthDate.Focus();
                return false;
            }

            if (ComboBoxGender.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn giới tính!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                ComboBoxGender.Focus();
                return false;
            }

            if (ComboBoxDepartment.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn phòng ban!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                ComboBoxDepartment.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TextBoxBaseSalary.Text) ||
                !double.TryParse(TextBoxBaseSalary.Text.Replace(",", ""), out _))
            {
                MessageBox.Show("Vui lòng nhập mức lương hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBoxBaseSalary.Focus();
                return false;
            }

            if (!DatePickerStartDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                DatePickerStartDate.Focus();
                return false;
            }

            return true;
        }
    }
}
