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
    public partial class ViewClientsForm : TemplateForm
    {
        public ViewClientsForm()
        {
            InitializeComponent();
            LoadDataIntoDataGridView();
        }

        private void LoadDataIntoDataGridView()
        {
            using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Clients_LoadDataIntoDataGridView", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    DataTable dtClients = new DataTable();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    dtClients.Load(sdr);

                    ClientDataGridView.DataSource = dtClients;
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClientsForm clientsForm = new ClientsForm();
            clientsForm.ShowDialog();
            LoadDataIntoDataGridView();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (SearchTextBox.Text != String.Empty)
            {
                using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Clients_SearchByClientName", con))
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
                            ClientDataGridView.DataSource = dtRole;
                        }
                        else
                        {
                            MessageBox.Show("No matching Client is found", "No record", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ClientDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ClientDataGridView.Rows.Count > 0)
            {
                string clientID = ClientDataGridView.SelectedRows[0].Cells[0].Value.ToString();

                ClientsForm clientsForm = new ClientsForm();
                clientsForm.LabelClientID.Text = clientID;
                clientsForm.IsUpdate = true;
                clientsForm.ShowDialog();

                LoadDataIntoDataGridView();
            }
        }
    }
}
