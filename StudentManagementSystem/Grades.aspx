<%--
  DESIGNER: Ankit Kumar
  TASK: Task 7 - Grades Management CRUD
  DESCRIPTION: Allows Create, Read, Update, Delete of student grades
               linked to enrolments using ADO.NET connected mode
--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Grades.aspx.cs" Inherits="StudentManagementSystem.Grades" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Grades Management</title>
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
        select {
            width: 316px; padding: 8px; margin: 5px 0;
            border: 1px solid #ccc; border-radius: 4px; }
        input[type=submit], input[type=button] {
            background: #e67e22; color: white; padding: 8px 16px;
            border: none; border-radius: 4px; cursor: pointer; margin: 4px; }
        input[type=submit]:hover { background: #ca6f1e; }
        .msg-success { color: green; font-weight: bold; }
        .msg-error   { color: red;   font-weight: bold; }
        table { width: 100%; border-collapse: collapse; margin-top: 10px; }
        th { background: #e67e22; color: white; padding: 10px; text-align: left; }
        td { padding: 8px; border-bottom: 1px solid #ddd; }
        tr:hover { background: #fef9f0; }
        .grade-a  { color: green;  font-weight: bold; }
        .grade-b  { color: blue;   font-weight: bold; }
        .grade-c  { color: orange; font-weight: bold; }
        .grade-f  { color: red;    font-weight: bold; }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <h1>📊 Grades Management</h1>

        <!-- Navigation -->
        <div class="nav">
            <a href="Default.aspx">🏠 Home</a>
            <a href="Students.aspx">👤 Students</a>
            <a href="Courses.aspx">📚 Courses</a>
            <a href="Enrolments.aspx">📋 Enrolments</a>
            <a href="Grades.aspx">📊 Grades</a>
        </div>

        <!-- Message area -->
        <asp:Label ID="lblMessage" runat="server" CssClass="msg-success"></asp:Label>

        <!-- ADD / EDIT GRADE FORM -->
        <div class="box">
            <h2>Add / Edit Grade</h2>
            <asp:HiddenField ID="hdnGradeID" runat="server" Value="0" />

            <label>Select Enrolment (Student — Course):</label><br />
            <asp:DropDownList ID="ddlEnrolments" runat="server"></asp:DropDownList><br />

            <label>Grade (e.g. A, B+, C, 85):</label><br />
            <asp:TextBox ID="txtGradeValue" runat="server" placeholder="e.g. A, B+, 85" /><br />

            <label>Grade Date:</label><br />
            <asp:TextBox ID="txtGradeDate" runat="server" TextMode="Date" /><br />

            <label>Comments (optional):</label><br />
            <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="3" /><br /><br />

            <asp:Button ID="btnSave"   runat="server" Text="💾 Save Grade" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="✖ Cancel"      OnClick="btnCancel_Click" />
        </div>

        <!-- FILTER BY STUDENT -->
        <div class="box">
            <h2>Filter Grades</h2>
            <label>Filter by Student:</label><br />
            <asp:DropDownList ID="ddlFilterStudent" runat="server"></asp:DropDownList>
            <asp:Button ID="btnFilter"  runat="server" Text="🔍 Filter"    OnClick="btnFilter_Click" />
            <asp:Button ID="btnShowAll" runat="server" Text="Show All"     OnClick="btnShowAll_Click" />
        </div>

        <!-- GRADES LIST -->
        <div class="box">
            <h2>All Grades</h2>
            <asp:GridView ID="gvGrades" runat="server" AutoGenerateColumns="false"
                OnRowCommand="gvGrades_RowCommand" EmptyDataText="No grades found.">
                <Columns>
                    <asp:BoundField DataField="GradeID"     HeaderText="ID"       />
                    <asp:BoundField DataField="StudentName" HeaderText="Student"  />
                    <asp:BoundField DataField="CourseCode"  HeaderText="Course"   />
                    <asp:BoundField DataField="GradeValue"  HeaderText="Grade"    />
                    <asp:BoundField DataField="GradeDate"   HeaderText="Date"     />
                    <asp:BoundField DataField="Comments"    HeaderText="Comments" />
                    <asp:ButtonField Text="✏ Edit"   CommandName="EditGrade"   ButtonType="Button" />
                    <asp:ButtonField Text="🗑 Delete" CommandName="DeleteGrade" ButtonType="Button" />
                </Columns>
            </asp:GridView>
        </div>

    </form>
</body>
</html>