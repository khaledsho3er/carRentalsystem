using CarAgency.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace CarAgency.Forms
{
    public partial class CarsForm : TemplateForm
    {
        public CarsForm()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Properties to handle update and delete operations
        public bool IsUpdate { get; set; }

        private void CarsForm_Load(object sender, EventArgs e)
        {

            LoadDataIntoSuppliersComboBox();

            // for update process
            if (this.IsUpdate == true)
            {
                using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Cars_ReloadDataForUpdate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@cID", this.CarIDLabel.Text);

                        if (con.State != ConnectionState.Open)
                            con.Open();

                        DataTable dtCars = new DataTable();

                        SqlDataReader sdr = cmd.ExecuteReader();

                        dtCars.Load(sdr);

                        DataRow row = dtCars.Rows[0];

                        CarBrandTextBox.Text = row["cBrand"].ToString();
                        ModelNameTextBox.Text = row["model"].ToString();
                        ModelYearTextBox.Text = row["modelYear"].ToString();
                        QtyTextBox.Text = row["cQty"].ToString();
                        PriceTextBox.Text = row["cPrice"].ToString();
                        DescriptionTextBox.Text = row["Description"].ToString();
                        CarIDLabel.Text = row["cID"].ToString();

                        // Change Controls
                        SaveButton.Text = "Update Information";
                        DeleteButton.Enabled = true;
                        car_id.Visible = true;
                    }
                }
            }
        }

        private void LoadDataIntoSuppliersComboBox()
        {
            using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Suppliers_LoadDataIntoComboBox", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    DataTable dtSuppliers = new DataTable();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    dtSuppliers.Load(sdr);

                    SupplierComboBox.DataSource = dtSuppliers;
                    SupplierComboBox.DisplayMember = "Supplier_Name";
                    SupplierComboBox.ValueMember = "Supplier_ID";
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
                        using (SqlCommand cmd = new SqlCommand("usp_Cars_UpdateCarsByCarID", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@cID", this.CarIDLabel.Text.Trim());
                            cmd.Parameters.AddWithValue("@cBrand", CarBrandTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@model", ModelNameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@modelYear", ModelYearTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@cQty", QtyTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@cPrice", PriceTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Description", DescriptionTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@cSupplier", SupplierComboBox.SelectedValue);
                            cmd.Parameters.AddWithValue("@CreatedBy", LoggedInUser.UserName);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Car record successfully updated in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }
                }
                else
                {
                    // Do Insert Operation
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Cars_InsertNewCar", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@cBrand", CarBrandTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@model", ModelNameTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@modelYear", ModelYearTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@cQty", QtyTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@cPrice", PriceTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@Description", DescriptionTextBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@cSupplier", SupplierComboBox.SelectedValue);
                            cmd.Parameters.AddWithValue("@CreatedBy", LoggedInUser.UserName);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Car record successfully saved in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetFormControl();
                        }
                    }
                }
            }
        }

        private void ResetFormControl()
        {
            CarBrandTextBox.Clear();
            ModelNameTextBox.Clear();
            ModelYearTextBox.Clear();
            QtyTextBox.Clear();
            PriceTextBox.Clear();
            DescriptionTextBox.Clear();
            SupplierComboBox.SelectedIndex = 0;

            CarBrandTextBox.Focus();

            if (this.IsUpdate)
            {
                this.IsUpdate = false;
                SaveButton.Text = "Save Car Information";
                DeleteButton.Enabled = false;
                this.CarIDLabel.Text = "";
                this.car_id.Visible= false;
            }
        }

        private bool IsFormValid()
        {
            if (CarBrandTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Brand Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CarBrandTextBox.Focus();
                return false;
            }

            if (ModelNameTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Model Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ModelNameTextBox.Focus();
                return false;
            }

            if (ModelYearTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Model year is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ModelYearTextBox.Focus();
                return false;
            }

            if (QtyTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Quantity is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                QtyTextBox.Focus();
                return false;
            }

            if (PriceTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Price is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PriceTextBox.Focus();
                return false;
            }

            if (SupplierComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select employee role from drop down.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SupplierComboBox.Focus();
                return false;
            }

            return true;
        }

        private void ModelYearTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void QtyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void PriceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (this.IsUpdate == true)
            {
                DialogResult result = MessageBox.Show("Are you sure, you want to delete this Car?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // delete employee
                    using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Cars_DeleteCarsByCarId", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@cID", this.CarIDLabel.Text.Trim());

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Car is successfully deleted from the system.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
