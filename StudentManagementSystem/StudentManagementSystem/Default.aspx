<%--
  DESIGNER: Ankit Kumar
  TASK: Task 7 - Home Dashboard
  DESCRIPTION: Welcome page for the Student Management System.
               Provides navigation to all modules.
--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="StudentManagementSystem._Default" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Student Management System</title>
    <style>
        * { box-sizing: border-box; margin: 0; padding: 0; }
        body { font-family: Arial, sans-serif; background: #f4f6f8; min-height: 100vh; }
        .header { background: #2c3e50; color: white; padding: 20px 40px; }
        .header h1 { font-size: 26px; }
        .header p  { font-size: 13px; color: #aab7c4; margin-top: 4px; }
        .content { padding: 40px; max-width: 900px; margin: 0 auto; }
        .welcome { background: white; border-radius: 8px; padding: 30px;
                   box-shadow: 0 2px 6px rgba(0,0,0,0.1); margin-bottom: 30px; text-align: center; }
        .welcome h2 { color: #2c3e50; margin-bottom: 10px; }
        .welcome p  { color: #666; font-size: 15px; line-height: 1.6; }
        .cards { display: flex; gap: 20px; flex-wrap: wrap; }
        .card { background: white; border-radius: 8px; padding: 30px 20px;
                box-shadow: 0 2px 6px rgba(0,0,0,0.1); flex: 1; min-width: 200px;
                text-align: center; text-decoration: none; color: #2c3e50;
                transition: transform 0.2s, box-shadow 0.2s; display: block; }
        .card:hover { transform: translateY(-4px); box-shadow: 0 6px 16px rgba(0,0,0,0.15); }
        .card .icon { font-size: 48px; margin-bottom: 15px; }
        .card h3    { font-size: 18px; margin-bottom: 8px; }
        .card p     { font-size: 13px; color: #888; line-height: 1.5; }
        .card-blue   { border-top: 4px solid #2980b9; }
        .card-green  { border-top: 4px solid #27ae60; }
        .card-purple { border-top: 4px solid #8e44ad; }
        .footer { text-align: center; padding: 30px; color: #aaa; font-size: 13px; }
    </style>
</head>
<body>
    <div class="header">
        <h1>🎓 Student Management System</h1>
        <p>BIS111 Web Design and Programming — Assessment 2 | Ankit Kumar</p>
    </div>

    <div class="content">
        <div class="welcome">
            <h2>Welcome! 👋</h2>
            <p>This system allows you to manage student records, course information,
               and enrolments. Use the cards below to navigate to each section.</p>
        </div>

        <div class="cards">
            <a href="Students.aspx" class="card card-blue">
                <div class="icon">👤</div>
                <h3>Students</h3>
                <p>Add, view, update, delete and search student records.</p>
            </a>

            <a href="Courses.aspx" class="card card-green">
                <div class="icon">📚</div>
                <h3>Courses</h3>
                <p>Manage course codes, names and descriptions.</p>
            </a>

            <a href="Enrolments.aspx" class="card card-purple">
                <div class="icon">📋</div>
                <h3>Enrolments</h3>
                <p>Enrol students into courses and manage enrolment records.</p>
            </a>
        </div>
    </div>

    <div class="footer">
        Student Management System &copy; 2026 — Built with ASP.NET &amp; SQL Server
    </div>
</body>
</html>