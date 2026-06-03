<%--
  DESIGNER: Ankit Kumar
  TASK: Task 7 - Course Management CRUD
  DESCRIPTION: Allows Create, Read, Update, Delete of course records
               using ADO.NET connected mode with SQL Server
--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="StudentManagementSystem.Courses" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Course Management</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 30px; background: #f4f6f8; }
        h1   { color: #2c3e50; }
        .nav { margin-bottom: 20px; }
        .nav a { margin-right: 15px; color: #2980b9; text-decoration: none; font-weight: bold; }
        .box { background: white; padding: 20px; border-radius: 8px;
               box-shadow: 0 2px 6px rgba(0,0,0,0.1); margin-bottom: 20px; }
        input[type=text] {
            width: 300px; padding: 8px; margin: 5px 0;
            border: 1px solid #ccc; border-radius: 4px; }
        textarea {
            width: 300px; padding: 8px; margin: 5px 0;
            border: 1px solid #ccc; border-radius: 4px; }
        input[type=submit], input[type=button] {
            background: #27ae60; color: white; padding: 8px 16px;
            border: none; border-radius: 4px; cursor: pointer; margin: 4px; }
        input[type=submit]:hover { background: #1e8449; }
        .msg-success { color: green; font-weight: bold; }
        .msg-error   { color: red;   font-weight: bold; }
        table { width: 100%; border-collapse: collapse; margin-top: 10px; }
        th { background: #27ae60; color: white; padding: 10px; text-align: left; }
        td { padding: 8px; border-bottom: 1px solid #ddd; }
        tr:hover { background: #eafaf1; }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <h1>📚 Course Management</h1>

        <div class="nav">
            <a href="Default.aspx">🏠 Home</a>
            <a href="Students.aspx">👤 Students</a>
            <a href="Courses.aspx">📚 Courses</a>
            <a href="Enrolments.aspx">📋 Enrolments</a>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="msg-success"></asp:Label>

        <div class="box">
            <h2>Add / Edit Course</h2>
            <asp:HiddenField ID="hdnCourseID" runat="server" Value="0" />

            <label>Course Code (e.g. BIS111):</label><br />
            <asp:TextBox ID="txtCourseCode" runat="server" /><br />

            <label>Course Name:</label><br />
            <asp:TextBox ID="txtCourseName" runat="server" /><br />

            <label>Description (optional):</label><br />
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" /><br /><br />

            <asp:Button ID="btnSave"   runat="server" Text="💾 Save Course" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="✖ Cancel"       OnClick="btnCancel_Click" />
        </div>

        <div class="box">
            <h2>All Courses</h2>
            <asp:GridView ID="gvCourses" runat="server" AutoGenerateColumns="false"
                OnRowCommand="gvCourses_RowCommand" EmptyDataText="No courses found.">
                <Columns>
                    <asp:BoundField DataField="CourseID"          HeaderText="ID"          />
                    <asp:BoundField DataField="CourseCode"        HeaderText="Code"        />
                    <asp:BoundField DataField="CourseName"        HeaderText="Name"        />
                    <asp:BoundField DataField="CourseDescription" HeaderText="Description" />
                    <asp:ButtonField Text="✏ Edit"   CommandName="EditCourse"   ButtonType="Button" />
                    <asp:ButtonField Text="🗑 Delete" CommandName="DeleteCourse" ButtonType="Button" />
                </Columns>
            </asp:GridView>
        </div>

    </form>
</body>
</html>