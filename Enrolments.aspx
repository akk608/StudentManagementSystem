<%--
  DESIGNER: Ankit Kumar
  TASK: Task 7 - Enrolment Management
  DESCRIPTION: Allows enrolling students into courses, viewing enrolments,
               and removing enrolments using ADO.NET connected mode
--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Enrolments.aspx.cs" Inherits="StudentManagementSystem.Enrolments" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Enrolment Management</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 30px; background: #f4f6f8; }
        h1   { color: #2c3e50; }
        .nav { margin-bottom: 20px; }
        .nav a { margin-right: 15px; color: #2980b9; text-decoration: none; font-weight: bold; }
        .box { background: white; padding: 20px; border-radius: 8px;
               box-shadow: 0 2px 6px rgba(0,0,0,0.1); margin-bottom: 20px; }
        select {
            width: 316px; padding: 8px; margin: 5px 0;
            border: 1px solid #ccc; border-radius: 4px; }
        input[type=submit], input[type=button] {
            background: #8e44ad; color: white; padding: 8px 16px;
            border: none; border-radius: 4px; cursor: pointer; margin: 4px; }
        input[type=submit]:hover { background: #6c3483; }
        .msg-success { color: green; font-weight: bold; }
        .msg-error   { color: red;   font-weight: bold; }
        table { width: 100%; border-collapse: collapse; margin-top: 10px; }
        th { background: #8e44ad; color: white; padding: 10px; text-align: left; }
        td { padding: 8px; border-bottom: 1px solid #ddd; }
        tr:hover { background: #f5eef8; }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <h1>📋 Enrolment Management</h1>

        <div class="nav">
            <a href="Default.aspx">🏠 Home</a>
            <a href="Students.aspx">👤 Students</a>
            <a href="Courses.aspx">📚 Courses</a>
            <a href="Enrolments.aspx">📋 Enrolments</a>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="msg-success"></asp:Label>

        <div class="box">
            <h2>Enrol a Student into a Course</h2>

            <label>Select Student:</label><br />
            <asp:DropDownList ID="ddlStudents" runat="server"></asp:DropDownList><br />

            <label>Select Course:</label><br />
            <asp:DropDownList ID="ddlCourses" runat="server"></asp:DropDownList><br />

            <label>Enrolment Date (leave blank for today):</label><br />
            <asp:TextBox ID="txtEnrolDate" runat="server" TextMode="Date" /><br /><br />

            <asp:Button ID="btnEnrol" runat="server" Text="✅ Enrol Student" OnClick="btnEnrol_Click" />
        </div>

        <div class="box">
            <h2>View Enrolments for a Student</h2>

            <asp:DropDownList ID="ddlViewStudent" runat="server"></asp:DropDownList>
            <asp:Button ID="btnView"    runat="server" Text="🔍 View"              OnClick="btnView_Click" />
            <asp:Button ID="btnViewAll" runat="server" Text="Show All Enrolments"  OnClick="btnViewAll_Click" />

            <asp:GridView ID="gvEnrolments" runat="server" AutoGenerateColumns="false"
                OnRowCommand="gvEnrolments_RowCommand" EmptyDataText="No enrolments found.">
                <Columns>
                    <asp:BoundField DataField="EnrolmentID"   HeaderText="ID"      />
                    <asp:BoundField DataField="StudentName"   HeaderText="Student" />
                    <asp:BoundField DataField="CourseCode"    HeaderText="Code"    />
                    <asp:BoundField DataField="CourseName"    HeaderText="Course"  />
                    <asp:BoundField DataField="EnrolmentDate" HeaderText="Date"    />
                    <asp:ButtonField Text="🗑 Remove" CommandName="RemoveEnrolment" ButtonType="Button" />
                </Columns>
            </asp:GridView>
        </div>

    </form>
</body>
</html>