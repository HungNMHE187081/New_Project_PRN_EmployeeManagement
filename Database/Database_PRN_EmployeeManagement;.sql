CREATE DATABASE PRN_EmployeeManagement;
GO

USE PRN_EmployeeManagement;
GO

-- Bảng lưu trữ thông tin về các vai trò của người dùng (Admin hoặc Employee)
CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL  -- Admin hoặc Employee
);

-- Bảng lưu trữ thông tin người dùng
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,  -- Tên đăng nhập
    Password NVARCHAR(255) NOT NULL,        -- Mật khẩu
    RoleID INT,                             -- Vai trò của người dùng
    IsActive BIT DEFAULT 1,                 -- Trạng thái hoạt động của tài khoản
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID) ON DELETE CASCADE
);

-- Bảng lưu trữ thông tin về các phòng ban
CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY IDENTITY(1,1),
    DepartmentName NVARCHAR(100) NOT NULL  -- Tên phòng ban
);

-- Bảng lưu trữ thông tin nhân viên
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT UNIQUE,                     -- ID người dùng liên kết
    FullName NVARCHAR(100) NOT NULL,       -- Họ và tên
    BirthDate DATE NOT NULL,               -- Ngày sinh
    Gender NVARCHAR(10),                   -- Giới tính
    Address NVARCHAR(255),                 -- Địa chỉ
    Phone NVARCHAR(20),                    -- Số điện thoại
    DepartmentID INT,                      -- ID phòng ban
    Position NVARCHAR(50),                 -- Chức vụ
    BaseSalary FLOAT,                      -- Lương cơ bản
    StartDate DATE NOT NULL,               -- Ngày bắt đầu làm việc
    AvatarPath NVARCHAR(255),              -- Đường dẫn ảnh đại diện
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
);

-- Bảng lưu trữ thông tin lương của nhân viên
CREATE TABLE Salaries (
    SalaryID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT,                        -- ID nhân viên
    Month INT NOT NULL,                    -- Tháng
    Year INT NOT NULL,                     -- Năm
    Allowance FLOAT DEFAULT 0,             -- Phụ cấp
    Bonus FLOAT DEFAULT 0,                 -- Thưởng
    Deduction FLOAT DEFAULT 0,             -- Khấu trừ
    PaymentDate DATE,                      -- Ngày thanh toán
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE CASCADE
);

-- Bảng lưu trữ thông tin chấm công của nhân viên
CREATE TABLE Attendance (
    AttendanceID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT,                        -- ID nhân viên
    Date DATE NOT NULL,                    -- Ngày chấm công
    CheckIn TIME,                          -- Giờ vào
    CheckOut TIME,                         -- Giờ ra
    Status NVARCHAR(20),                   -- Trạng thái (Present, Absent, Late)
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE CASCADE
);

-- Bảng lưu trữ thông tin số ngày nghỉ phép của nhân viên
CREATE TABLE LeaveBalances (
    LeaveID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT,                        -- ID nhân viên
    Year INT NOT NULL,                     -- Năm
    AnnualLeave INT DEFAULT 12,            -- Số ngày nghỉ phép hàng năm
    SickLeave INT DEFAULT 30,              -- Số ngày nghỉ bệnh
    UnpaidLeave INT DEFAULT 0,             -- Số ngày nghỉ không lương
    RemainingLeave AS (AnnualLeave + SickLeave - UnpaidLeave),  -- Số ngày nghỉ còn lại
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE CASCADE
);

-- Bảng lưu trữ thông tin thông báo
CREATE TABLE Notifications (
    NotificationID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL,          -- Tiêu đề thông báo
    Message NVARCHAR(500),                 -- Nội dung thông báo
    DepartmentID INT,                      -- ID phòng ban nhận thông báo
    CreatedDate DATETIME DEFAULT GETDATE(),-- Ngày tạo thông báo
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID) ON DELETE CASCADE
);

-- Bảng lưu trữ lịch sử hoạt động của người dùng
CREATE TABLE ActivityLogs (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,                            -- ID người dùng
    Action NVARCHAR(100),                  -- Hành động
    Description NVARCHAR(255),             -- Mô tả chi tiết
    LogDate DATETIME DEFAULT GETDATE(),    -- Ngày ghi nhận hành động
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);

INSERT INTO Departments (DepartmentName)
VALUES 
('Human Resources'), 
('Finance'), 
('IT'), 
('Marketing');

INSERT INTO Roles (RoleName)
VALUES 
('Admin'), 
('Employee');

INSERT INTO Users (Username, Password, RoleID, IsActive)
VALUES 
('admin', '123', 1, 1),  -- Admin
('employee1', '123', 2, 1),  -- Employee
('employee2', '123', 2, 1);  -- Employee

INSERT INTO Employees (UserID, FullName, BirthDate, Gender, Address, Phone, DepartmentID, Position, BaseSalary, StartDate, AvatarPath)
VALUES 
(1, 'Nguyen Van A', '1990-01-01', 'Male', '123 Main St', '0123456789', 1, 'Manager', 1000.0, '2022-01-01', 'path/to/avatar1.jpg'),
(2, 'Tran Thi B', '1992-02-02', 'Female', '456 Elm St', '0987654321', 2, 'Accountant', 800.0, '2022-02-01', 'path/to/avatar2.jpg'),
(3, 'Le Van C', '1985-03-03', 'Male', '789 Oak St', '0112233445', 3, 'Developer', 1200.0, '2022-03-01', 'path/to/avatar3.jpg');