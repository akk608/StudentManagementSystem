/*
 * DESIGNER: Ankit Kumar
 * TASK:     Task 7 - Grades Management Code-Behind
 * DESCRIPTION: Handles all CRUD operations for Grades using
 *              ADO.NET in connected mode with SQL Server.
 *              Grades are linked to Enrolments.
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace StudentManagementSystem
{
    public partial class Grades : System.Web.UI.Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString;

        // ── Page Load ─────────────────────────────────────────────────────────
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdowns();
                LoadAllGrades();
                // Set default date to today
                txtGradeDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        // ── Load dropdowns ────────────────────────────────────────────────────
        private void LoadDropdowns()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Load enrolments as "StudentName — CourseCode"
                    string enrolQuery =
                        "SELECT e.EnrolmentID, s.StudentName, c.CourseCode, c.CourseName " +
                        "FROM Enrolments e " +
                        "INNER JOIN Students s ON e.StudentID = s.StudentID " +
                        "INNER JOIN Courses  c ON e.CourseID  = c.CourseID " +
                        "ORDER BY s.StudentName ASC";

                    SqlCommand enrolCmd = new SqlCommand(enrolQuery, conn);
                    SqlDataReader enrolReader = enrolCmd.ExecuteReader();
                    DataTable enrolDT = new DataTable();
                    enrolDT.Load(enrolReader);

                    // Format display as "Alice Smith — BIS111"
                    enrolDT.Columns.Add("DisplayText", typeof(string));
                    foreach (DataRow row in enrolDT.Rows)
                        row["DisplayText"] = row["StudentName"] + " — " + row["CourseCode"];

                    ddlEnrolments.DataSource = enrolDT;
                    ddlEnrolments.DataTextField = "DisplayText";
                    ddlEnrolments.DataValueField = "EnrolmentID";
                    ddlEnrolments.DataBind();
                    ddlEnrolments.Items.Insert(0, new ListItem("-- Select Enrolment --", "0"));

                    // Load students for filter dropdown
                    string studentQuery = "SELECT StudentID, StudentName FROM Students ORDER BY StudentName ASC";
                    SqlCommand studentCmd = new SqlCommand(studentQuery, conn);
                    SqlDataReader studentReader = studentCmd.ExecuteReader();
                    DataTable studentDT = new DataTable();
                    studentDT.Load(studentReader);

                    ddlFilterStudent.DataSource = studentDT;
                    ddlFilterStudent.DataTextField = "StudentName";
                    ddlFilterStudent.DataValueField = "StudentID";
                    ddlFilterStudent.DataBind();
                    ddlFilterStudent.Items.Insert(0, new ListItem("-- All Students --", "0"));
                }
            }
            catch (Exception ex)
            {
                ShowError("Could not load dropdowns: " + ex.Message);
            }
        }

        // ── Load ALL grades ───────────────────────────────────────────────────
        private void LoadAllGrades()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    // JOIN 4 tables to show useful info
                    string query =
                        "SELECT g.GradeID, s.StudentName, c.CourseCode, " +
                        "       g.GradeValue, g.GradeDate, " +
                        "       ISNULL(g.Comments, '') AS Comments " +
                        "FROM Grades g " +
                        "INNER JOIN Enrolments e ON g.EnrolmentID = e.EnrolmentID " +
                        "INNER JOIN Students   s ON e.StudentID   = s.StudentID " +
                        "INNER JOIN Courses    c ON e.CourseID    = c.CourseID " +
                        "ORDER BY s.StudentName ASC, c.CourseCode ASC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    gvGrades.DataSource = dt;
                    gvGrades.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError("Could not load grades: " + ex.Message);
            }
        }

        // ── Load grades for ONE student ───────────────────────────────────────
        private void LoadStudentGrades(int studentID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query =
                        "SELECT g.GradeID, s.StudentName, c.CourseCode, " +
                        "       g.GradeValue, g.GradeDate, " +
                        "       ISNULL(g.Comments, '') AS Comments " +
                        "FROM Grades g " +
                        "INNER JOIN Enrolments e ON g.EnrolmentID = e.EnrolmentID " +
                        "INNER JOIN Students   s ON e.StudentID   = s.StudentID " +
                        "INNER JOIN Courses    c ON e.CourseID    = c.CourseID " +
                        "WHERE s.StudentID = @SID " +
                        "ORDER BY c.CourseCode ASC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SID", studentID);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    gvGrades.DataSource = dt;
                    gvGrades.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError("Could not load grades: " + ex.Message);
            }
        }

        // ── Save button (ADD and UPDATE) ──────────────────────────────────────
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int enrolmentID = int.Parse(ddlEnrolments.SelectedValue);
            string gradeValue = txtGradeValue.Text.Trim();
            string gradeDateTxt = txtGradeDate.Text.Trim();
            string comments = txtComments.Text.Trim();
            int gradeID = int.Parse(hdnGradeID.Value);

            // Validate inputs
            if (enrolmentID == 0)
            {
                ShowError("Error: Please select an enrolment.");
                return;
            }
            if (string.IsNullOrEmpty(gradeValue))
            {
                ShowError("Error: Grade value is required.");
                return;
            }
            if (string.IsNullOrEmpty(gradeDateTxt))
            {
                ShowError("Error: Grade date is required.");
                return;
            }

            DateTime gradeDate = DateTime.Parse(gradeDateTxt);

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    if (gradeID == 0)
                    {
                        // ── CREATE ────────────────────────────────────────────
                        // Check if grade already exists for this enrolment
                        SqlCommand checkCmd = new SqlCommand(
                            "SELECT COUNT(*) FROM Grades WHERE EnrolmentID = @EID", conn);
                        checkCmd.Parameters.AddWithValue("@EID", enrolmentID);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            ShowError("Error: A grade already exists for this enrolment. Use Edit to update it.");
                            return;
                        }

                        SqlCommand insertCmd = new SqlCommand(
                            "INSERT INTO Grades (EnrolmentID, GradeValue, GradeDate, Comments) " +
                            "VALUES (@EID, @Grade, @Date, @Comments)", conn);
                        insertCmd.Parameters.AddWithValue("@EID", enrolmentID);
                        insertCmd.Parameters.AddWithValue("@Grade", gradeValue);
                        insertCmd.Parameters.AddWithValue("@Date", gradeDate);
                        insertCmd.Parameters.AddWithValue("@Comments", comments);
                        insertCmd.ExecuteNonQuery();
                        ShowSuccess("Grade added successfully!");
                    }
                    else
                    {
                        // ── UPDATE ────────────────────────────────────────────
                        SqlCommand updateCmd = new SqlCommand(
                            "UPDATE Grades SET GradeValue = @Grade, GradeDate = @Date, " +
                            "Comments = @Comments WHERE GradeID = @ID", conn);
                        updateCmd.Parameters.AddWithValue("@Grade", gradeValue);
                        updateCmd.Parameters.AddWithValue("@Date", gradeDate);
                        updateCmd.Parameters.AddWithValue("@Comments", comments);
                        updateCmd.Parameters.AddWithValue("@ID", gradeID);
                        updateCmd.ExecuteNonQuery();
                        ShowSuccess("Grade updated successfully!");
                    }
                }

                ClearForm();
                LoadAllGrades();
            }
            catch (Exception ex)
            {
                ShowError("Error saving grade: " + ex.Message);
            }
        }

        // ── Grid buttons (Edit / Delete) ──────────────────────────────────────
        protected void gvGrades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int gradeID = Convert.ToInt32(gvGrades.Rows[rowIndex].Cells[0].Text);

            if (e.CommandName == "EditGrade")
            {
                // Load grade details into form for editing
                try
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        string query =
                            "SELECT g.GradeID, g.EnrolmentID, g.GradeValue, " +
                            "       g.GradeDate, ISNULL(g.Comments,'') AS Comments " +
                            "FROM Grades g WHERE g.GradeID = @ID";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ID", gradeID);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            hdnGradeID.Value = gradeID.ToString();
                            txtGradeValue.Text = reader["GradeValue"].ToString();
                            txtGradeDate.Text = Convert.ToDateTime(reader["GradeDate"]).ToString("yyyy-MM-dd");
                            txtComments.Text = reader["Comments"].ToString();

                            // Select correct enrolment in dropdown
                            string enrolID = reader["EnrolmentID"].ToString();
                            if (ddlEnrolments.Items.FindByValue(enrolID) != null)
                                ddlEnrolments.SelectedValue = enrolID;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowError("Error loading grade: " + ex.Message);
                }
            }
            else if (e.CommandName == "DeleteGrade")
            {
                // ── DELETE ────────────────────────────────────────────────────
                try
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        SqlCommand delCmd = new SqlCommand(
                            "DELETE FROM Grades WHERE GradeID = @ID", conn);
                        delCmd.Parameters.AddWithValue("@ID", gradeID);
                        delCmd.ExecuteNonQuery();
                        ShowSuccess("Grade deleted successfully.");
                    }
                    LoadAllGrades();
                }
                catch (Exception ex)
                {
                    ShowError("Error deleting grade: " + ex.Message);
                }
            }
        }

        // ── Filter button ─────────────────────────────────────────────────────
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            int studentID = int.Parse(ddlFilterStudent.SelectedValue);

            if (studentID == 0)
            {
                ShowError("Error: Please select a student to filter.");
                return;
            }
            LoadStudentGrades(studentID);
        }

        // ── Show All button ───────────────────────────────────────────────────
        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            LoadAllGrades();
        }

        // ── Cancel button ─────────────────────────────────────────────────────
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        // ── Helper: clear the form ────────────────────────────────────────────
        private void ClearForm()
        {
            hdnGradeID.Value = "0";
            txtGradeValue.Text = "";
            txtComments.Text = "";
            txtGradeDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            ddlEnrolments.SelectedIndex = 0;
            lblMessage.Text = "";
        }

        // ── Helper: green success message ─────────────────────────────────────
        private void ShowSuccess(string msg)
        {
            lblMessage.Text = msg;
            lblMessage.CssClass = "msg-success";
        }

        // ── Helper: red error message ─────────────────────────────────────────
        private void ShowError(string msg)
        {
            lblMessage.Text = msg;
            lblMessage.CssClass = "msg-error";
        }
    }
}