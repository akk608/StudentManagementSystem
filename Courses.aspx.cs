/*
 * DESIGNER: Ankit Kumar
 * TASK:     Task 7 - Course Management Code-Behind
 * DESCRIPTION: Handles all CRUD operations for Courses using
 *              ADO.NET in connected mode with SQL Server.
 *              Follows the pseudo code written by Charles (Task 3).
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace StudentManagementSystem
{
    public partial class Courses : System.Web.UI.Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadCourses();
        }

        private void LoadCourses()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "SELECT CourseID, CourseCode, CourseName, CourseDescription " +
                                   "FROM Courses ORDER BY CourseCode ASC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    gvCourses.DataSource = dt;
                    gvCourses.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError("Could not load courses: " + ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string code = txtCourseCode.Text.Trim();
            string name = txtCourseName.Text.Trim();
            string description = txtDescription.Text.Trim();
            int id = int.Parse(hdnCourseID.Value);

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name))
            {
                ShowError("Error: Course code and name are required.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    if (id == 0)
                    {
                        SqlCommand checkCmd = new SqlCommand(
                            "SELECT COUNT(*) FROM Courses WHERE CourseCode = @Code", conn);
                        checkCmd.Parameters.AddWithValue("@Code", code);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            ShowError("Error: Course code already exists.");
                            return;
                        }

                        SqlCommand insertCmd = new SqlCommand(
                            "INSERT INTO Courses (CourseCode, CourseName, CourseDescription) " +
                            "VALUES (@Code, @Name, @Description)", conn);
                        insertCmd.Parameters.AddWithValue("@Code", code);
                        insertCmd.Parameters.AddWithValue("@Name", name);
                        insertCmd.Parameters.AddWithValue("@Description", description);
                        insertCmd.ExecuteNonQuery();
                        ShowSuccess("Course added successfully!");
                    }
                    else
                    {
                        SqlCommand updateCmd = new SqlCommand(
                            "UPDATE Courses SET CourseName = @Name, CourseDescription = @Description " +
                            "WHERE CourseID = @ID", conn);
                        updateCmd.Parameters.AddWithValue("@Name", name);
                        updateCmd.Parameters.AddWithValue("@Description", description);
                        updateCmd.Parameters.AddWithValue("@ID", id);
                        updateCmd.ExecuteNonQuery();
                        ShowSuccess("Course updated successfully!");
                    }
                }
                ClearForm();
                LoadCourses();
            }
            catch (Exception ex)
            {
                ShowError("Error saving course: " + ex.Message);
            }
        }

        protected void gvCourses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int courseID = Convert.ToInt32(gvCourses.Rows[rowIndex].Cells[0].Text);

            if (e.CommandName == "EditCourse")
            {
                hdnCourseID.Value = courseID.ToString();
                txtCourseCode.Text = gvCourses.Rows[rowIndex].Cells[1].Text;
                txtCourseName.Text = gvCourses.Rows[rowIndex].Cells[2].Text;
                txtDescription.Text = gvCourses.Rows[rowIndex].Cells[3].Text;
            }
            else if (e.CommandName == "DeleteCourse")
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();

                        SqlCommand delEnrol = new SqlCommand(
                            "DELETE FROM Enrolments WHERE CourseID = @ID", conn);
                        delEnrol.Parameters.AddWithValue("@ID", courseID);
                        delEnrol.ExecuteNonQuery();

                        SqlCommand delCourse = new SqlCommand(
                            "DELETE FROM Courses WHERE CourseID = @ID", conn);
                        delCourse.Parameters.AddWithValue("@ID", courseID);
                        delCourse.ExecuteNonQuery();

                        ShowSuccess("Course and related enrolments deleted.");
                    }
                    LoadCourses();
                }
                catch (Exception ex)
                {
                    ShowError("Error deleting course: " + ex.Message);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            hdnCourseID.Value = "0";
            txtCourseCode.Text = "";
            txtCourseName.Text = "";
            txtDescription.Text = "";
            lblMessage.Text = "";
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