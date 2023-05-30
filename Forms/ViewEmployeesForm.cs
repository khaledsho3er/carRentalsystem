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
    public partial class ViewEmployeesForm : TemplateForm
    {
        public ViewEmployeesForm()
        {
            InitializeComponent();
            LoadDataIntoDataGridView();
        }

        private void LoadDataIntoDataGridView()
        {
            using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Employees_LoadDataIntoDataGridView", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    DataTable dtRoles = new DataTable();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    dtRoles.Load(sdr);

                    EmployeeDataGridView.DataSource = dtRoles;
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmployeesForm userform = new EmployeesForm();
            userform.ShowDialog();
            LoadDataIntoDataGridView();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (SearchTextBox.Text != String.Empty)
            {
                using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Employees_SearchByUsernameOrRole", con))
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
                            EmployeeDataGridView.DataSource = dtRole;
                        }
                        else
                        {
                            MessageBox.Show("No matching Employee is found", "No record", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void EmployeeDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(EmployeeDataGridView.Rows.Count > 0)
            {
                string userName = EmployeeDataGridView.SelectedRows[0].Cells[0].Value.ToString();
                
                EmployeesForm userForm = new EmployeesForm();
                userForm.UserName = userName;
                userForm.IsUpdate = true;
                userForm.ShowDialog();
                LoadDataIntoDataGridView();
            }
        }
    }
}
