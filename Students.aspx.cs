/*
 * DESIGNER: Ankit Kumar
 *              Follows the pseudo code written by Charles (Task 3).
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace StudentManagementSystem
{
    public partial class Students : System.Web.UI.Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadStudents();
        }

        private void LoadStudents(string searchTerm = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query;
                    if (string.IsNullOrEmpty(searchTerm))
                    {
                        query = "SELECT StudentID, StudentName, StudentEmail " +
                                "FROM Students ORDER BY StudentName ASC";
                    }
                    else
                    {
                        query = "SELECT StudentID, StudentName, StudentEmail " +
                                "FROM Students " +
                                "WHERE StudentName LIKE @Term OR " +
                                "CAST(StudentID AS NVARCHAR) = @ExactID";
                    }

                    SqlCommand cmd = new SqlCommand(query, conn);
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        cmd.Parameters.AddWithValue("@Term", "%" + searchTerm + "%");
                        cmd.Parameters.AddWithValue("@ExactID", searchTerm);
                    }

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    gvStudents.DataSource = dt;
                    gvStudents.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError("Could not load students: " + ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            int id = int.Parse(hdnStudentID.Value);

            if (string.IsNullOrEmpty(name))
            {
                ShowError("Error: Student name is required.");
                return;
            }
            if (string.IsNullOrEmpty(email))
            {
                ShowError("Error: A valid email address is required.");
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
                            "SELECT COUNT(*) FROM Students WHERE StudentEmail = @Email", conn);
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            ShowError("Error: A student with this email already exists.");
                            return;
                        }

                        SqlCommand insertCmd = new SqlCommand(
                            "INSERT INTO Students (StudentName, StudentEmail) VALUES (@Name, @Email)", conn);
                        insertCmd.Parameters.AddWithValue("@Name", name);
                        insertCmd.Parameters.AddWithValue("@Email", email);
                        insertCmd.ExecuteNonQuery();
                        ShowSuccess("Student added successfully!");
                    }
                    else
                    {
                        SqlCommand updateCmd = new SqlCommand(
                            "UPDATE Students SET StudentName = @Name, StudentEmail = @Email " +
                            "WHERE StudentID = @ID", conn);
                        updateCmd.Parameters.AddWithValue("@Name", name);
                        updateCmd.Parameters.AddWithValue("@Email", email);
                        updateCmd.Parameters.AddWithValue("@ID", id);
                        updateCmd.ExecuteNonQuery();
                        ShowSuccess("Student updated successfully!");
                    }
                }
                ClearForm();
                LoadStudents();
            }
            catch (Exception ex)
            {
                ShowError("Error saving student: " + ex.Message);
            }
        }

        protected void gvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int studentID = Convert.ToInt32(gvStudents.Rows[rowIndex].Cells[0].Text);

            if (e.CommandName == "EditStudent")
            {
                txtName.Text = gvStudents.Rows[rowIndex].Cells[1].Text;
                txtEmail.Text = gvStudents.Rows[rowIndex].Cells[2].Text;
                hdnStudentID.Value = studentID.ToString();
            }
            else if (e.CommandName == "DeleteStudent")
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();

                        SqlCommand delEnrol = new SqlCommand(
                            "DELETE FROM Enrolments WHERE StudentID = @ID", conn);
                        delEnrol.Parameters.AddWithValue("@ID", studentID);
                        delEnrol.ExecuteNonQuery();

                        SqlCommand delStudent = new SqlCommand(
                            "DELETE FROM Students WHERE StudentID = @ID", conn);
                        delStudent.Parameters.AddWithValue("@ID", studentID);
                        delStudent.ExecuteNonQuery();

                        ShowSuccess("Student and related enrolments deleted.");
                    }
                    LoadStudents();
                }
                catch (Exception ex)
                {
                    ShowError("Error deleting student: " + ex.Message);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string term = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(term))
            {
                ShowError("Error: Please enter a search term.");
                return;
            }
            LoadStudents(term);
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadStudents();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtEmail.Text = "";
            hdnStudentID.Value = "0";
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