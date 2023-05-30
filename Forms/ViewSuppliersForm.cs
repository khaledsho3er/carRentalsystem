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
    public partial class ViewSuppliersForm : TemplateForm
    {
        public ViewSuppliersForm()
        {
            InitializeComponent();
            LoadDataIntoDataGridView();
        }

        private void LoadDataIntoDataGridView()
        {
            using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Suppliers_LoadDataIntoDataGridView", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    DataTable dtClients = new DataTable();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    dtClients.Load(sdr);

                    SupplierDataGridView.DataSource = dtClients;
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SuppliersForm suppliersForm = new SuppliersForm();
            suppliersForm.ShowDialog();
            LoadDataIntoDataGridView();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (SearchTextBox.Text != String.Empty)
            {
                using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Suppliers_SearchBySupplierName", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //passing parameter
                        cmd.Parameters.AddWithValue("@filter", SearchTextBox.Text.Trim());

                        if (con.State != ConnectionState.Open)
                            con.Open();

                        DataTable dtRole = new DataTable();

                        SqlDataReader sdr = cmd.ExecuteReader();

                        if (sdr.HasRows)
                        {
                            dtRole.Load(sdr);
                            SupplierDataGridView.DataSource = dtRole;
                        }
                        else
                        {
                            MessageBox.Show("No matching Supplier is found", "No record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void refreshRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
            SearchTextBox.Clear();
            SearchTextBox.Focus();
        }

        private void SupplierDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (SupplierDataGridView.Rows.Count > 0)
            {
                string SupplierID = SupplierDataGridView.SelectedRows[0].Cells[0].Value.ToString();

                SuppliersForm suppliersForm = new SuppliersForm();
                suppliersForm.LabelSupplierID.Text = SupplierID;
                suppliersForm.IsUpdate = true;
                suppliersForm.ShowDialog();

                LoadDataIntoDataGridView();
            }
        }
    }
}
