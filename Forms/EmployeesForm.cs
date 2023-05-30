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
    public partial class EmployeesForm : TemplateForm
    {
        public EmployeesForm()
        {
            InitializeComponent();
        }

        // Properties to handle update and delete operations
        public string UserName { get; set; }
        public bool IsUpdate { get; set; }

        private void UserForm_Load(object sender, EventArgs e)
        {
            LoadDataIntoRolesComboBox();

            // for update process
            if (this.IsUpdate == true)
            {
                using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Employee_ReloadDataForUpdate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Username", this.UserName);

                        if (con.State != ConnectionState.Open)
                            con.Open();

                        DataTable dtEmployee = new DataTable();

                        SqlDataReader sdr = cmd.ExecuteReader();

                        dtEmployee.Load(sdr);

                        DataRow row = dtEmployee.Rows[0];

                        FirstNameTextBox.Text = row["Fname"].ToString();
                        LastNameTextBox.Text = row["Lname"].ToString();
                        UserNameTextBox.Text = row["Username"].ToString();
                        PasswordTextBox.Text = row["Password"].ToString();
                        RolesComboBox.SelectedValue = row["Role"];
                        IsActiveCheckBox.Checked = Convert.ToBoolean(row["IsActive"]);
                        DescriptionTextBox.Text = row["Description"].ToString();

                        // Change Controls
                        SaveButton.Text = "Update Information";
                        DeleteButton.Enabled = true;
                    }
                }
            }
        }

        private void LoadDataIntoRolesComboBox()
        {
            using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Roles_LoadDataIntoComboBox", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    DataTable dtRoles = new DataTable();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    dtRoles.Load(sdr);

                    RolesComboBox.DataSource = dtRoles;
                    RolesComboBox.DisplayMember = "RoleTitle";
                    RolesComboBox.ValueMember = "Role_ID";
                }
            }
        }

        private void saveInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFormValid())
            {
                if(this.IsUpdate == true)
                {
                    // Do update operation
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Employee_UpdateEmployeeByUsername", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@OldUsername", this.UserName);
                            cmd.Parameters.AddWithValue("@Username", UserNameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Password", PasswordTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Fname", FirstNameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Lname", LastNameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Role", RolesComboBox.SelectedValue);
                            cmd.Parameters.AddWithValue("@IsActive", IsActiveCheckBox.Checked);
                            cmd.Parameters.AddWithValue("@Description", DescriptionTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreatedBy", LoggedInUser.UserName);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Employee successfully updated in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }
                }
                else
                {
                    // Do Insert Operation
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Employee_InsertNewEmployee", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Username", UserNameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Password", PasswordTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Fname", FirstNameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Lname", LastNameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Role", RolesComboBox.SelectedValue);
                            cmd.Parameters.AddWithValue("@IsActive", IsActiveCheckBox.Checked);
                            cmd.Parameters.AddWithValue("@Description", DescriptionTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreatedBy", LoggedInUser.UserName);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Employee successfully saved in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }
                }
            }
        }

        private void ResetFormControl()
        {
            FirstNameTextBox.Clear();
            LastNameTextBox.Clear();
            UserNameTextBox.Clear();
            PasswordTextBox.Clear();
            RolesComboBox.SelectedIndex = 0;
            IsActiveCheckBox.Checked = true;
            DescriptionTextBox.Clear();

            FirstNameTextBox.Focus();

            if(this.IsUpdate)
            {
                this.IsUpdate = false;
                SaveButton.Text = "Save Employee Information";
                DeleteButton.Enabled = false;
                this.UserName = null;
            }
        }

        private bool IsFormValid()
        {
            if (FirstNameTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("First Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FirstNameTextBox.Focus();
                return false;
            }

            if (LastNameTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Last Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LastNameTextBox.Focus();
                return false;
            }

            if (UserNameTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("User Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UserNameTextBox.Focus();
                return false;
            }

            if (UserNameTextBox.Text.Length >= 50)
            {
                MessageBox.Show("User Name length should be less than or equal to 50 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UserNameTextBox.Focus();
                return false;
            }

            if (PasswordTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Password is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PasswordTextBox.Focus();
                return false;
            }

            if (PasswordTextBox.Text.Length >= 50)
            {
                MessageBox.Show("Password length should be less than or equal to 50 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PasswordTextBox.Focus();
                return false;
            }

            if (RolesComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select employee role from drop down.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RolesComboBox.Focus();
                return false;
            }

            return true;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if(this.IsUpdate == true)
            {
                DialogResult result = MessageBox.Show("Are you sure, you want to delete this Employee?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if(result == DialogResult.Yes)
                {
                    // delete employee
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Employees_DeleteEmployeeByUsername", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Username", this.UserName);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Employee is successfully deleted from the system.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You cancelled this process.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
