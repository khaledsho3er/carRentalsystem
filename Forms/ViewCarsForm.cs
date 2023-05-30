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
    public partial class ViewCarsForm : TemplateForm
    {
        
        public ViewCarsForm()
        {
            InitializeComponent();
            LoadDataIntoDataGridView();
        }

        private void LoadDataIntoDataGridView()
        {
            using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cars_LoadDataIntoDataGridView", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    DataTable dtCars = new DataTable();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    dtCars.Load(sdr);

                    CarsDataGridView.DataSource = dtCars;
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newCarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CarsForm carsForm = new CarsForm();
            carsForm.ShowDialog();
            LoadDataIntoDataGridView();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (SearchTextBox.Text != String.Empty)
            {
                using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Cars_SearchByBrandOrSupplier", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //passing parameter
                        cmd.Parameters.AddWithValue("@filter", SearchTextBox.Text.Trim());

                        if (con.State != ConnectionState.Open)
                            con.Open();

                        DataTable dtCar = new DataTable();

                        SqlDataReader sdr = cmd.ExecuteReader();

                        if (sdr.HasRows)
                        {
                            dtCar.Load(sdr);
                            CarsDataGridView.DataSource = dtCar;
                        }
                        else
                        {
                            MessageBox.Show("No matching Car is found", "No record", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void CarsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CarsDataGridView.Rows.Count > 0)
            {
                string carid = CarsDataGridView.SelectedRows[0].Cells[0].Value.ToString();

                CarsForm carForm = new CarsForm();
                carForm.CarIDLabel.Text = carid;
                carForm.IsUpdate = true;
                carForm.ShowDialog();
                LoadDataIntoDataGridView();
            }
        }
    }
}
