using CarAgency.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarAgency.Forms
{
    public partial class RolesForm : TemplateForm
    {
        public RolesForm()
        {
            InitializeComponent();
        }

        // properties to handle update process
        public int RoleID { get; set; }
        public bool IsUpDate { get; set; }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(IsFormValid())
            {
                if(this.IsUpDate)
                {
                    // Do Update Operation
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Roles_UpdateRoleByRoleID", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Role_ID", this.RoleID);
                            cmd.Parameters.AddWithValue("@RoleTitle", TitleTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Description", DescriptionTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreatedBy", LoggedInUser.UserName);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Role is successfully updated in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }

                }
                else
                {
                    // Do Insert Operation
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Roles_InsertNewRole", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@RoleTitle", TitleTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Description", DescriptionTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreatedBy", LoggedInUser.UserName);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Role is successfully saved in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }
                }
            }
        }

        private void ResetFormControl()
        {
            TitleTextBox.Clear();
            DescriptionTextBox.Clear();

            TitleTextBox.Focus();

            // Check if form is loaded for update process
            if(this.IsUpDate)
            {
                this.RoleID = 0;
                this.IsUpDate = false;
                SaveButton.Text = "Save Information";
                DeleteButton.Enabled = false;
            }
        }

        private bool IsFormValid()
        {
            if(TitleTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Role Title is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TitleTextBox.Focus();
                return false;
            }

            if (TitleTextBox.Text.Length >= 50)
            {
                MessageBox.Show("Role Title length should be less than or equal to 50 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TitleTextBox.Focus();
                return false;
            }

            return true;
        }

        private void RolesForm_Load(object sender, EventArgs e)
        {
            if(this.IsUpDate == true)
            {
                using(SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Roles_ReloadDataForUpdate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Role_ID", RoleID);

                        if(con.State != ConnectionState.Open)
                            con.Open();

                        DataTable dtRole = new DataTable();

                        SqlDataReader sdr = cmd.ExecuteReader();

                        dtRole.Load(sdr);

                        DataRow row = dtRole.Rows[0];

                        TitleTextBox.Text = row["RoleTitle"].ToString();
                        DescriptionTextBox.Text = row["Description"].ToString();

                        // Change Controls
                        SaveButton.Text = "Update Information";
                        DeleteButton.Enabled = true;
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if(this.IsUpDate)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this row?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if(result == DialogResult.Yes)
                {
                    // Delete recore from database
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Roles_DeleteRoleByID", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Role_ID", this.RoleID);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Role is successfully deleted from the system.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }
                }
            }
        }
    }
}
