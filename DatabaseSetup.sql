 
-- =============================================
-- Student Management System - Database Setup
-- BIS111 Assessment 3
-- Author: Ankit Kumar
-- Date: June 2026
-- =============================================

CREATE DATABASE StudentManagementDB;
GO
USE StudentManagementDB;
GO

CREATE TABLE Students (
    StudentID    INT IDENTITY(1,1) PRIMARY KEY,
    StudentName  NVARCHAR(100) NOT NULL,
    StudentEmail NVARCHAR(150) NOT NULL UNIQUE
);
GO

CREATE TABLE Courses (
    CourseID          INT IDENTITY(1,1) PRIMARY KEY,
    CourseCode        NVARCHAR(10)  NOT NULL UNIQUE,
    CourseName        NVARCHAR(100) NOT NULL,
    CourseDescription NVARCHAR(500) NULL
);
GO

CREATE TABLE Enrolments (
    EnrolmentID   INT IDENTITY(1,1) PRIMARY KEY,
    StudentID     INT NOT NULL,
    CourseID      INT NOT NULL,
    EnrolmentDate DATE NOT NULL,
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    FOREIGN KEY (CourseID)  REFERENCES Courses(CourseID)
);
GO

INSERT INTO Students (StudentName, StudentEmail) VALUES
('Alice Smith',   'alice@email.com'),
('Bob Jones',     'bob@email.com'),
('Charlie Brown', 'charlie@email.com');

INSERT INTO Courses (CourseCode, CourseName, CourseDescription) VALUES
('BIS111', 'Web Design and Programming', 'Intro to web development'),
('BIS203', 'Database Systems',           'Relational database design'),
('BIS301', 'Networking Fundamentals',    'Basic networking concepts');

INSERT INTO Enrolments (StudentID, CourseID, EnrolmentDate) VALUES
(1, 1, '2026-03-01'),
(1, 2, '2026-03-01'),
(2, 1, '2026-03-01');
GO

PRINT 'Database setup complete!';