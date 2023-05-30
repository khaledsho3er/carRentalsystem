using CarAgency.classes;
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
    public partial class LoginForm : TemplateForm
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
           /*  using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
              {
                  using (SqlCommand cmd = new SqlCommand("usp_Employees_VerifyLoginDetails", con))
                  {
                      cmd.CommandType = CommandType.StoredProcedure;

                      cmd.Parameters.AddWithValue("@Username", UserNameTextBox.Text.Trim());
                      cmd.Parameters.AddWithValue("@Password", PasswordTextBox.Text.Trim());

                      if(con.State != ConnectionState.Open)
                          con.Open();

                      DataTable dtEmployees = new DataTable();

                      SqlDataReader sdr = cmd.ExecuteReader();

                      if(sdr.HasRows)
                      {
                          dtEmployees.Load(sdr);
                          DataRow EmployeeRow = dtEmployees.Rows[0];

                          LoggedInUser.UserName = EmployeeRow["UserName"].ToString();
                          LoggedInUser.RoleID = Convert.ToInt32(EmployeeRow["Role"]);

                          this.Hide();

                          DashboardForm dashboardForm = new DashboardForm();
                          dashboardForm.ShowDialog();
                      }
                      else
                      {
                          MessageBox.Show("User Name or Password is incorrect./ Account might be Deactivated.", "Authentication failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      }
                  }
              }*/
            customer_login customer = new customer_login();
           

            if(customer.login(UserNameTextBox.Text.Trim(), PasswordTextBox.Text.Trim()) == true)
            {
                
                this.Hide();

                DashboardForm dashboardForm = new DashboardForm();
                dashboardForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("User Name or Password is incorrect./ Account might be Deactivated.", "Authentication failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            signup signup = new signup();
            signup.Show();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void Regestier_Click(object sender, EventArgs e)
        {
            
        }
    }
}
