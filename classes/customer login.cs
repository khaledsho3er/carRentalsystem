using CarAgency.Forms;
using CarAgency.General;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace CarAgency.classes
{
    internal class customer_login : customer
    {
        public customer_login()
        {
        }

        public override bool login(string username, string password)
        {
            string query = "SELECT username FROM customer WHERE username = @Username AND password = @Password";
            using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    DataTable dtEmployees = new DataTable();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    if (sdr.HasRows)
                    {
                        dtEmployees.Load(sdr);
                        DataRow EmployeeRow = dtEmployees.Rows[0];

                        LoggedInUser.UserName = EmployeeRow["UserName"].ToString();
                        LoggedInUser.RoleID = Convert.ToInt32(EmployeeRow["Role"]);

                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            
        }

       

        public override bool signup(string username, string password, string email)
        {
           

            String query = "INSERT into customer(username, password, email) VALUES(@username, @password, @email)";
            using (SqlConnection con = new SqlConnection(AppConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@email", email);
                    int i = cmd.ExecuteNonQuery();
                    con.Close();

                    if (i == 0)
                    {
                        return false;
                    }
                    return true;

                }
            }
        }
    }
}
