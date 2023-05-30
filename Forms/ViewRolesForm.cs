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
    public partial class ViewRolesForm : TemplateForm
    {
        public ViewRolesForm()
        {
            InitializeComponent();
            LoadDataIntoDataGridView();
        }

        private void LoadDataIntoDataGridView()
        {
            using(SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
            {
                using(SqlCommand cmd = new SqlCommand("usp_Roles_LoadDataIntoDataGridView", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if(con.State != ConnectionState.Open)
                        con.Open();

                    DataTable dtRoles = new DataTable();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    dtRoles.Load(sdr);

                    RolesDataGridView.DataSource = dtRoles;
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(SearchTextBox.Text != String.Empty)
            {
                using(SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                {
                    using(SqlCommand cmd = new SqlCommand("usp_Roles_SearchByTitle", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //passing parameter
                        cmd.Parameters.AddWithValue("@RoleTitle", SearchTextBox.Text.Trim());

                        if (con.State != ConnectionState.Open)
                            con.Open();

                        DataTable dtRole = new DataTable();

                        SqlDataReader sdr = cmd.ExecuteReader();

                        if(sdr.HasRows)
                        {
                            dtRole.Load(sdr);
                            RolesDataGridView.DataSource = dtRole;
                        }
                        else
                        {
                            MessageBox.Show("No matching Role is found", "No record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void refreshRecorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
            SearchTextBox.Clear();
            SearchTextBox.Focus();
        }

        private void newRoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RolesForm rf = new RolesForm();
            rf.ShowDialog();
            LoadDataIntoDataGridView();
        }

        private void RolesDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(RolesDataGridView.Rows.Count > 0)
            {
                int roleID = Convert.ToInt32(RolesDataGridView.SelectedRows[0].Cells[0].Value);

                RolesForm rolesForm = new RolesForm();
                rolesForm.RoleID = roleID;
                rolesForm.IsUpDate = true;
                rolesForm.ShowDialog();

                LoadDataIntoDataGridView();
            }
        }
    }
}
