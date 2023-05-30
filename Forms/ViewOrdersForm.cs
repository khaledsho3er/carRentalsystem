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
    public partial class ViewOrdersForm : TemplateForm
    {

        SqlConnection con = new SqlConnection(AppConnection.GetConnectionString());
        SqlCommand cmd = new SqlCommand();
        SqlDataReader sdr;

        public ViewOrdersForm()
        {
            InitializeComponent();
            LoadOrder();
        }

        public void LoadOrder()
        {
            double total = 0;
            int i = 0;

            dgvOrders.Rows.Clear();
            cmd = new SqlCommand("SELECT Order_ID, Orders.Client_ID, Clients.Fname + ' ' + Clients.Lname, Car_ID, Cars.model, OrderPrice, Qty, TotalPrice, " +
                "Orders.CreatedDate FROM Orders JOIN Clients ON Orders.Client_ID = Clients.Client_ID JOIN Cars ON Orders.Car_ID = Cars.cID WHERE CONCAT( Clients.Fname + ' ' + Clients.Lname , Cars.model, OrderPrice) LIKE '%" + txtSearch.Text+"%'", con);
            con.Open();
            sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                i++;
                dgvOrders.Rows.Add(i, sdr[0].ToString(), sdr[1].ToString(), sdr[2].ToString(), sdr[3].ToString(), sdr[4].ToString(), sdr[5].ToString(), sdr[6].ToString(), sdr[7].ToString(),Convert.ToDateTime(sdr[8].ToString()).ToString("dd/MM/yyyy"));
                total += Convert.ToInt32(sdr[7].ToString());
            }
            sdr.Close();
            con.Close();

            lblQty.Text = i.ToString();
            lblTotal.Text = total.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OrdersForm ordersForm = new OrdersForm();
            ordersForm.ShowDialog();
            LoadOrder();
        }

        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvOrders.Columns[e.ColumnIndex].Name;

            if(colName == "Delete")
            {
                if(MessageBox.Show("Are you sure you want to delete this Order?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cmd = new SqlCommand("Delete From Orders WHERE Order_ID LIKE '" + dgvOrders.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record has been successfully deleted!");

                    cmd = new SqlCommand("UPDATE Cars set cQty = (cQty+@Qty) Where cID LIKE '" + dgvOrders.Rows[e.RowIndex].Cells[4].Value.ToString() + "'", con);
                    cmd.Parameters.AddWithValue("@Qty", Convert.ToInt32(dgvOrders.Rows[e.RowIndex].Cells[7].Value.ToString()));

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            LoadOrder();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadOrder();
        }
    }
}
