/*
 * DESIGNER: Ankit Kumar
 * TASK:     Task 7 - Enrolment Management Code-Behind
 * DESCRIPTION: Handles enrolling students into courses, viewing enrolments,
 *              and removing enrolments using ADO.NET connected mode.
 *              Follows the pseudo code written by Charles (Task 3).
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace StudentManagementSystem
{
    public partial class Enrolments : System.Web.UI.Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdowns();
                LoadAllEnrolments();
            }
        }

        private void LoadDropdowns()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string studentQuery = "SELECT StudentID, StudentName FROM Students ORDER BY StudentName ASC";
                    SqlCommand studentCmd = new SqlCommand(studentQuery, conn);
                    SqlDataReader studentReader = studentCmd.ExecuteReader();
                    DataTable studentDT = new DataTable();
                    studentDT.Load(studentReader);

                    ddlStudents.DataSource = studentDT;
                    ddlStudents.DataTextField = "StudentName";
                    ddlStudents.DataValueField = "StudentID";
                    ddlStudents.DataBind();
                    ddlStudents.Items.Insert(0, new ListItem("-- Select Student --", "0"));

                    ddlViewStudent.DataSource = studentDT;
                    ddlViewStudent.DataTextField = "StudentName";
                    ddlViewStudent.DataValueField = "StudentID";
                    ddlViewStudent.DataBind();
                    ddlViewStudent.Items.Insert(0, new ListItem("-- Select Student --", "0"));

                    string courseQuery = "SELECT CourseID, CourseCode, CourseName FROM Courses ORDER BY CourseCode ASC";
                    SqlCommand courseCmd = new SqlCommand(courseQuery, conn);
                    SqlDataReader courseReader = courseCmd.ExecuteReader();
                    DataTable courseDT = new DataTable();
                    courseDT.Load(courseReader);

                    ddlCourses.DataSource = courseDT;
                    ddlCourses.DataTextField = "CourseName";
                    ddlCourses.DataValueField = "CourseID";
                    ddlCourses.DataBind();
                    ddlCourses.Items.Insert(0, new ListItem("-- Select Course --", "0"));
                }
            }
            catch (Exception ex)
            {
                ShowError("Could not load dropdowns: " + ex.Message);
            }
        }

        private void LoadAllEnrolments()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query =
                        "SELECT e.EnrolmentID, s.StudentName, c.CourseCode, " +
                        "       c.CourseName, e.EnrolmentDate " +
                        "FROM Enrolments e " +
                        "INNER JOIN Students s ON e.StudentID = s.StudentID " +
                        "INNER JOIN Courses  c ON e.CourseID  = c.CourseID " +
                        "ORDER BY s.StudentName ASC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    gvEnrolments.DataSource = dt;
                    gvEnrolments.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError("Could not load enrolments: " + ex.Message);
            }
        }

        private void LoadStudentEnrolments(int studentID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query =
                        "SELECT e.EnrolmentID, s.StudentName, c.CourseCode, " +
                        "       c.CourseName, e.EnrolmentDate " +
                        "FROM Enrolments e " +
                        "INNER JOIN Students s ON e.StudentID = s.StudentID " +
                        "INNER JOIN Courses  c ON e.CourseID  = c.CourseID " +
                        "WHERE e.StudentID = @SID " +
                        "ORDER BY e.EnrolmentDate ASC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SID", studentID);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    gvEnrolments.DataSource = dt;
                    gvEnrolments.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError("Could not load enrolments: " + ex.Message);
            }
        }

        protected void btnEnrol_Click(object sender, EventArgs e)
        {
            int studentID = int.Parse(ddlStudents.SelectedValue);
            int courseID = int.Parse(ddlCourses.SelectedValue);

            if (studentID == 0)
            {
                ShowError("Error: Please select a student.");
                return;
            }
            if (courseID == 0)
            {
                ShowError("Error: Please select a course.");
                return;
            }

            DateTime enrolDate = string.IsNullOrEmpty(txtEnrolDate.Text)
                ? DateTime.Today
                : DateTime.Parse(txtEnrolDate.Text);

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand checkCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Enrolments " +
                        "WHERE StudentID = @SID AND CourseID = @CID", conn);
                    checkCmd.Parameters.AddWithValue("@SID", studentID);
                    checkCmd.Parameters.AddWithValue("@CID", courseID);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        ShowError("Error: Student is already enrolled in this course.");
                        return;
                    }

                    SqlCommand insertCmd = new SqlCommand(
                        "INSERT INTO Enrolments (StudentID, CourseID, EnrolmentDate) " +
                        "VALUES (@SID, @CID, @Date)", conn);
                    insertCmd.Parameters.AddWithValue("@SID", studentID);
                    insertCmd.Parameters.AddWithValue("@CID", courseID);
                    insertCmd.Parameters.AddWithValue("@Date", enrolDate);
                    insertCmd.ExecuteNonQuery();

                    ShowSuccess("Student enrolled successfully!");
                }
                LoadAllEnrolments();
            }
            catch (Exception ex)
            {
                ShowError("Error enrolling student: " + ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            int studentID = int.Parse(ddlViewStudent.SelectedValue);

            if (studentID == 0)
            {
                ShowError("Error: Please select a student to view.");
                return;
            }
            LoadStudentEnrolments(studentID);
        }

        protected void btnViewAll_Click(object sender, EventArgs e)
        {
            LoadAllEnrolments();
        }

        protected void gvEnrolments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveEnrolment")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int enrolmentID = Convert.ToInt32(gvEnrolments.Rows[rowIndex].Cells[0].Text);

                try
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();

                        SqlCommand delCmd = new SqlCommand(
                            "DELETE FROM Enrolments WHERE EnrolmentID = @ID", conn);
                        delCmd.Parameters.AddWithValue("@ID", enrolmentID);
                        delCmd.ExecuteNonQuery();

                        ShowSuccess("Enrolment removed successfully.");
                    }
                    LoadAllEnrolments();
                }
                catch (Exception ex)
                {
                    ShowError("Error removing enrolment: " + ex.Message);
                }
            }
        }

        private void ShowSuccess(string msg)
        {
            lblMessage.Text = msg;
            lblMessage.CssClass = "msg-success";
        }

        private void ShowError(string msg)
        {
            lblMessage.Text = msg;
            lblMessage.CssClass = "msg-error";
        }
    }
}