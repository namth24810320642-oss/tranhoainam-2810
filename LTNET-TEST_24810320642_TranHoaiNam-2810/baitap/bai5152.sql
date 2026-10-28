CREATE DATABASE QuanLySinhVien;
GO
USE QuanLySinhVien;
GO


CREATE TABLE Users (
    Username NVARCHAR(50) PRIMARY KEY,
    Password NVARCHAR(100) NOT NULL
);


INSERT INTO Users (Username, Password) VALUES (N'admin', N'123456');


CREATE TABLE Courses (
    CourseID INT PRIMARY KEY IDENTITY(1,1),
    CourseName NVARCHAR(100) NOT NULL
);

INSERT INTO Courses (CourseName) VALUES 
(N'Lập trình C# WinForms'), 
(N'Lập trình Web ASP.NET'), 
(N'Cơ sở dữ liệu SQL Server');

CREATE TABLE Registrations (
    ID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100),
    Phone VARCHAR(20),
    BirthDate DATE,
    Gender NVARCHAR(10),
    CourseID INT,
    IsAgreed BIT,
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
);
SELECT * FROM Users;

-- Xem danh sách các khóa học
SELECT * FROM Courses;

-- Xem tất cả hồ sơ sinh viên vừa đăng ký qua Form 5.2
SELECT * FROM Registrations;