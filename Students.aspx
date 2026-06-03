<%--
  DESIGNER: Ankit Kumar
  TASK: Task 7 - Student Management CRUD
  DESCRIPTION: Allows Create, Read, Update, Delete and Search of student records
               using ADO.NET connected mode with SQL Server
--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Students.aspx.cs" Inherits="StudentManagementSystem.Students" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Student Management</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 30px; background: #f4f6f8; }
        h1   { color: #2c3e50; }
        .nav { margin-bottom: 20px; }
        .nav a { margin-right: 15px; color: #2980b9; text-decoration: none; font-weight: bold; }
        .box { background: white; padding: 20px; border-radius: 8px;
               box-shadow: 0 2px 6px rgba(0,0,0,0.1); margin-bottom: 20px; }
        input[type=text], input[type=email] {
            width: 300px; padding: 8px; margin: 5px 0; border: 1px solid #ccc; border-radius: 4px; }
        input[type=submit], input[type=button] {
            background: #2980b9; color: white; padding: 8px 16px;
            border: none; border-radius: 4px; cursor: pointer; margin: 4px; }
        input[type=submit]:hover { background: #1a5276; }
        .msg-success { color: green; font-weight: bold; }
        .msg-error   { color: red;   font-weight: bold; }
        table { width: 100%; border-collapse: collapse; margin-top: 10px; }
        th { background: #2980b9; color: white; padding: 10px; text-align: left; }
        td { padding: 8px; border-bottom: 1px solid #ddd; }
        tr:hover { background: #eaf4fb; }
    </style>

    <script type="text/javascript">
        function lettersOnly(event) {
            var char = String.fromCharCode(event.which || event.keyCode);
            // Allow letters, spaces, hyphens and apostrophes only
            if (/^[a-zA-Z\s\-']+$/.test(char)) {
                return true;
            }
            return false;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">

        <h1>🎓 Student Management System</h1>

        <div class="nav">
            <a href="Default.aspx">🏠 Home</a>
            <a href="Students.aspx">👤 Students</a>
            <a href="Courses.aspx">📚 Courses</a>
            <a href="Enrolments.aspx">📋 Enrolments</a>
            <a href="Grades.aspx">📊 Grades</a>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="msg-success"></asp:Label>

        <div class="box">
            <h2>Add / Edit Student</h2>
            <asp:HiddenField ID="hdnStudentID" runat="server" Value="0" />

            <label>Name:</label><br />
            <asp:TextBox ID="txtName" runat="server"
                onkeypress="return lettersOnly(event)"
                placeholder="Enter full name (letters only)" /><br />

            <label>Email:</label><br />
            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" /><br /><br />

            <asp:Button ID="btnSave"   runat="server" Text="💾 Save Student" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="✖ Cancel"        OnClick="btnCancel_Click" />
        </div>

        <div class="box">
            <h2>Search Students</h2>
            <asp:TextBox ID="txtSearch" runat="server" placeholder="Enter name or ID..." />
            <asp:Button  ID="btnSearch"  runat="server" Text="🔍 Search"  OnClick="btnSearch_Click" />
            <asp:Button  ID="btnShowAll" runat="server" Text="Show All"   OnClick="btnShowAll_Click" />
        </div>

        <div class="box">
            <h2>All Students</h2>
            <asp:GridView ID="gvStudents" runat="server" AutoGenerateColumns="false"
                OnRowCommand="gvStudents_RowCommand" EmptyDataText="No students found.">
                <Columns>
                    <asp:BoundField DataField="StudentID"    HeaderText="ID"    />
                    <asp:BoundField DataField="StudentName"  HeaderText="Name"  />
                    <asp:BoundField DataField="StudentEmail" HeaderText="Email" />
                    <asp:ButtonField Text="✏ Edit"   CommandName="EditStudent"   ButtonType="Button" />
                    <asp:ButtonField Text="🗑 Delete" CommandName="DeleteStudent" ButtonType="Button" />
                </Columns>
            </asp:GridView>
        </div>

    </form>
</body>
</html>