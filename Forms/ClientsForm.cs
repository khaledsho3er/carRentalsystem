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
    public partial class ClientsForm : TemplateForm
    {
        public ClientsForm()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Properties to handle update and delete operations
        public bool IsUpdate { get; set; }

        private void ClientsForm_Load(object sender, EventArgs e)
        {
            // for update process
            if (this.IsUpdate == true)
            {
                using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Clients_ReloadDataForUpdate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Client_ID", this.LabelClientID.Text);

                        if (con.State != ConnectionState.Open)
                            con.Open();

                        DataTable dtClient = new DataTable();

                        SqlDataReader sdr = cmd.ExecuteReader();

                        dtClient.Load(sdr);

                        DataRow row = dtClient.Rows[0];

                        FirstNameTextBox.Text = row["Fname"].ToString();
                        LastNameTextBox.Text = row["Lname"].ToString();
                        MobileNumberTextBox.Text = row["Mobile_Number"].ToString();
                        DescriptionTextBox.Text = row["Description"].ToString();
                        LabelClientID.Text = row["Client_ID"].ToString();

                        // Change Controls
                        SaveButton.Text = "Update Information";
                        DeleteButton.Enabled = true;
                        LabelCID.Visible= true;
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (IsFormValid())
            {
                if (this.IsUpdate == true)
                {
                    // Do update operation
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Clients_UpdateClientByClientID", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Client_ID", this.LabelClientID.Text);
                            cmd.Parameters.AddWithValue("@Fname", FirstNameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Lname", LastNameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Mobile_Number", MobileNumberTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Description", DescriptionTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreatedBy", LoggedInUser.UserName);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Client successfully updated in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }
                }
                else
                {
                    // Do Insert Operation
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Clients_InsertNewClient", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Fname", FirstNameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Lname", LastNameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Mobile_Number", MobileNumberTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Description", DescriptionTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreatedBy", LoggedInUser.UserName);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Client successfully saved in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            MobileNumberTextBox.Clear();
            DescriptionTextBox.Clear();

            FirstNameTextBox.Focus();

            if (this.IsUpdate)
            {
                this.IsUpdate = false;
                SaveButton.Text = "Save Client Information";
                DeleteButton.Enabled = false;
                this.LabelClientID.Text = "";
                this.LabelCID.Visible= false;
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

            if (MobileNumberTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Mobile number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MobileNumberTextBox.Focus();
                return false;
            }

            if (MobileNumberTextBox.Text.Length != 11)
            {
                MessageBox.Show("Moblie number should be 11 digits", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MobileNumberTextBox.Focus();
                return false;
            }

            return true;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (this.IsUpdate == true)
            {
                DialogResult result = MessageBox.Show("Are you sure, you want to delete this Client?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // delete employee
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Clients_DeleteClientByClientID", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Client_ID", this.LabelClientID.Text);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Client is successfully deleted from the system.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void MobileNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
