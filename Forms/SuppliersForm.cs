using CarAgency.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarAgency.Forms
{
    public partial class SuppliersForm : TemplateForm
    {
        public SuppliersForm()
        {
            InitializeComponent();
        }

        // Properties to handle update and delete operations
        public bool IsUpdate { get; set; }

        private void SuppliersForm_Load(object sender, EventArgs e)
        {
            // for update process
            if (this.IsUpdate == true)
            {
                using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Suppliers_ReloadDataForUpdate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Supplier_ID", this.LabelSupplierID.Text);

                        if (con.State != ConnectionState.Open)
                            con.Open();

                        DataTable dtSupplier = new DataTable();

                        SqlDataReader sdr = cmd.ExecuteReader();

                        dtSupplier.Load(sdr);

                        DataRow row = dtSupplier.Rows[0];

                        SupplierNameTextBox.Text = row["Supplier_Name"].ToString();
                        EmailTextBox.Text = row["Email"].ToString();
                        OriginTextBox.Text = row["Origin"].ToString();
                        DescriptionTextBox.Text = row["Description"].ToString();
                        LabelSupplierID.Text = row["Supplier_ID"].ToString();

                        // Change Controls
                        SaveButton.Text = "Update Information";
                        DeleteButton.Enabled = true;
                        LabelSID.Visible = true;
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
                        using (SqlCommand cmd = new SqlCommand("usp_Supplier_UpdateSupplierBySupplierID", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Supplier_ID", this.LabelSupplierID.Text);
                            cmd.Parameters.AddWithValue("@Supplier_Name", SupplierNameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", EmailTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Origin", OriginTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Description", DescriptionTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreatedBy", LoggedInUser.UserName);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Supplier successfully updated in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }
                }
                else
                {
                    // Do Insert Operation
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Suppliers_InsertNewSupplier", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Supplier_Name", SupplierNameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", EmailTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Origin",OriginTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Description", DescriptionTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreatedBy", LoggedInUser.UserName);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Supplier successfully saved in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }
                }
            }
        }

        private bool IsFormValid()
        {
            if (SupplierNameTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Supplier Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SupplierNameTextBox.Focus();
                return false;
            }

            if (EmailTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Email is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EmailTextBox.Focus();
                return false;
            }

            if (!IsValidEmail(EmailTextBox.Text.Trim()))
            {
                MessageBox.Show("Email address is not valid", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OriginTextBox.Focus();
                return false;
            }

            if (OriginTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Origin  is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OriginTextBox.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail( string email)
        {
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }

        private void ResetFormControl()
        {
            SupplierNameTextBox.Clear();
            EmailTextBox.Clear();
            OriginTextBox.Clear();
            DescriptionTextBox.Clear();

            SupplierNameTextBox.Focus();

            if (this.IsUpdate)
            {
                this.IsUpdate = false;
                SaveButton.Text = "Save Supplier Information";
                DeleteButton.Enabled = false;
                this.LabelSupplierID.Text = "";
                this.LabelSID.Visible = false;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (this.IsUpdate == true)
            {
                DialogResult result = MessageBox.Show("Are you sure, you want to delete this Supplier?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // delete employee
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Supplier_DeleteSupplierBySupplierID", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Supplier_ID", this.LabelSupplierID.Text);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Supplier is successfully deleted from the system.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
