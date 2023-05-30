using CarAgency.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarAgency.Forms
{
    public partial class OrdersForm : TemplateForm
    {

        SqlConnection con = new SqlConnection(AppConnection.GetConnectionString());
        SqlCommand cmd = new SqlCommand();
        SqlDataReader sdr;
        int qty = 0;

        public OrdersForm()
        {
            InitializeComponent();
            LoadClients();
            LoadCars();
        }

        public void LoadClients()
        {
            int i = 0;
            dgvClients.Rows.Clear();
            cmd = new SqlCommand("Select * from Clients Where CONCAT(Client_ID, Fname, Lname) Like '%"+ ClientSearchBox.Text + "%'", con);
            con.Open();
            sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                i++;
                dgvClients.Rows.Add(i, sdr[0].ToString(), sdr[1].ToString());
            }
            sdr.Close();
            con.Close();
        }

        public void LoadCars()
        {
            int i = 0;
            dgvCars.Rows.Clear();
            cmd = new SqlCommand("Select * from Cars Where CONCAT(cID, cBrand, cSupplier) Like '%" + CarSearchBox.Text + "%'", con);
            con.Open();
            sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                i++;
                dgvCars.Rows.Add(i, sdr[0].ToString(), sdr[1].ToString(), sdr[2].ToString(), sdr[3].ToString(), sdr[4].ToString(), sdr[5].ToString());
            }
            sdr.Close();
            con.Close();
        }

        private void ClientSearchBox_TextChanged(object sender, EventArgs e)
        {
            LoadClients();
        }

        private void CarSearchBox_TextChanged(object sender, EventArgs e)
        {
            LoadCars();
        }
        
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            GetQty();

            if(Convert.ToInt32(numericUpDown1.Value) > qty) 
            {
                MessageBox.Show("Instock quantity is not enough!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numericUpDown1.Value = numericUpDown1.Value - 1;
                return;
            }

            if(Convert.ToInt32(numericUpDown1.Value) > 0)
            {
                int total = Convert.ToInt32(txtCarPrice.Text) * Convert.ToInt32(numericUpDown1.Value);
                txtTotal.Text = total.ToString();
            }
            
        }

        private void dgvClients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtClientId.Text = dgvClients.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtClientname.Text = dgvClients.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void dgvCars_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCarID.Text = dgvCars.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtCBrand.Text = dgvCars.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtCarPrice.Text = dgvCars.Rows[e.RowIndex].Cells[6].Value.ToString();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtClientId.Text  == "")
                {
                    MessageBox.Show("Please select a client!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtCarID.Text == "")
                {
                    MessageBox.Show("Please select a Car!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if(numericUpDown1.Value == 0)
                {
                    MessageBox.Show("Please select the Quantity!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to insert this order?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cmd = new SqlCommand("INSERT INTO Orders (Car_ID, Client_ID, Qty, OrderPrice, TotalPrice, CreatedDate) VALUES (@Car_ID, @Client_ID, @Qty, @OrderPrice, @TotalPrice, @CreatedDate)", con);

                    cmd.Parameters.AddWithValue("@Car_ID", Convert.ToInt32(txtCarID.Text));
                    cmd.Parameters.AddWithValue("@Client_ID", Convert.ToInt32(txtClientId.Text));
                    cmd.Parameters.AddWithValue("@Qty", Convert.ToInt32(numericUpDown1.Value));
                    cmd.Parameters.AddWithValue("@OrderPrice", Convert.ToInt32(txtCarPrice.Text));
                    cmd.Parameters.AddWithValue("@TotalPrice", Convert.ToInt32(txtTotal.Text));
                    cmd.Parameters.AddWithValue("@CreatedDate", dateTimePicker1.Value);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Order has been successfully inserted");

                    cmd = new SqlCommand("UPDATE Cars set cQty = (cQty-@Qty) Where cID LIKE '" + txtCarID.Text + "'", con);
                    cmd.Parameters.AddWithValue("@Qty", Convert.ToInt32(numericUpDown1.Value));

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Clear();

                    LoadCars();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show( ex.Message);
            }
   
        }

        private void Clear()
        {
            txtClientId.Clear();
            txtClientname.Clear();

            txtCarID.Clear();
            txtCBrand.Clear();

            txtCarPrice.Clear();
            txtTotal.Clear();
            numericUpDown1.Value = 0;
            dateTimePicker1.Value = DateTime.Now;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void GetQty()
        {
            cmd = new SqlCommand("Select cQty from Cars Where cID ='" + txtCarID.Text + "'", con);
            con.Open();
            sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                qty = Convert.ToInt32(sdr[0].ToString());
            }
            sdr.Close();
            con.Close();
        }

    }
}
