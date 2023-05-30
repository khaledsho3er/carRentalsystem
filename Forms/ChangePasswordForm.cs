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
    public partial class ChangePasswordForm : TemplateForm
    {
        public ChangePasswordForm()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChangePasswordButton_Click(object sender, EventArgs e)
        {
            if(IsFormValid())
            {
                // Verify Existing Password
                if(IsPasswordVerified())
                {
                    // Go and update password

                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Employee_ChangePassword", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Username", LoggedInUser.UserName);
                            cmd.Parameters.AddWithValue("@NewPassword", NewPasswordTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreatedBy", LoggedInUser.UserName);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Password is successfully changed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Your old password is not correct, please enter correct password.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ResetFormControl()
        {
            NewPasswordTextBox.Clear();
            OldPasswordTextBox.Clear();
            ConfirmPasswordTextBox.Clear();

            OldPasswordTextBox.Focus();
        }

        private bool IsPasswordVerified()
        {
            bool isCorrect = false;

            using(SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Employees_VerifyPassword", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Username", LoggedInUser.UserName);
                    cmd.Parameters.AddWithValue("@Password", OldPasswordTextBox.Text.Trim());

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    
                    SqlDataReader sdr = cmd.ExecuteReader();

                    if(sdr.HasRows)
                    {
                        isCorrect = true;
                    }
                }
            }

            return isCorrect;
        }

        private bool IsFormValid()
        {
            if(OldPasswordTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Old Password is required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OldPasswordTextBox.Focus();
                return false;
            }

            if (NewPasswordTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("New Password is required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NewPasswordTextBox.Focus();
                return false;
            }

            if (ConfirmPasswordTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Confirming Password is required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ConfirmPasswordTextBox.Focus();
                return false;
            }

            if (NewPasswordTextBox.Text.Trim() != ConfirmPasswordTextBox.Text.Trim())
            {
                MessageBox.Show("Passwords are not matched", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ConfirmPasswordTextBox.Focus();
                return false;
            }

            return true;
        }
    }
}
