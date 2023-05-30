using CarAgency.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarAgency.Forms
{
    public partial class DashboardForm : TemplateForm
    {
        public DashboardForm()
        {
            InitializeComponent();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
           

            SetUpEmployeeAccess();
           
        }

        private void SetUpEmployeeAccess()
        {
            switch(LoggedInUser.RoleID)
            {
                case 1:
                    RoleLabel.Text = "Full Rights";
                    adminMenu.Visible = true;
                    break;
                case 2:
                    RoleLabel.Text = "Normal Rights";
                    break;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            ChangePasswordForm cp = new ChangePasswordForm();
            cp.ShowDialog();
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            ViewOrdersForm vo = new ViewOrdersForm();
            vo.ShowDialog();
        }

        private void btnCars_Click(object sender, EventArgs e)
        {
            ViewCarsForm vc = new ViewCarsForm();
            vc.ShowDialog();
        }

        private void btnViewClient_Click(object sender, EventArgs e)
        {
            ViewClientsForm vClients= new ViewClientsForm();
            vClients.ShowDialog();
        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            ViewSuppliersForm vs = new ViewSuppliersForm();
            vs.ShowDialog();
        }

        private void newClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClientsForm cf = new ClientsForm();
            cf.ShowDialog();
        }

        private void viewClientsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewClientsForm vClients = new ViewClientsForm();
            vClients.ShowDialog();
        }

        private void newSuppliersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SuppliersForm sf = new SuppliersForm();
            sf.ShowDialog();
        }

        private void viewSuppliersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewSuppliersForm vs = new ViewSuppliersForm();
            vs.ShowDialog();
        }

        private void newCarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CarsForm carsF = new CarsForm();
            carsF.ShowDialog();
        }

        private void viewCarsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewCarsForm vc = new ViewCarsForm();
            vc.ShowDialog();
        }

        private void newOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrdersForm of= new OrdersForm();
            of.ShowDialog();
        }

        private void viewOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewOrdersForm vo = new ViewOrdersForm();
            vo.ShowDialog();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm lf = new LoginForm();
            lf.ShowDialog();
            
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void changePasswordToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ChangePasswordForm changePasswordForm = new ChangePasswordForm();
            changePasswordForm.ShowDialog();
        }

        private void newEmployeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmployeesForm ef = new EmployeesForm();
            ef.ShowDialog();
        }

        private void viewEmployeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewEmployeesForm vef = new ViewEmployeesForm();
            vef.ShowDialog();
        }

        private void newRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RolesForm rf = new RolesForm();
            rf.ShowDialog();
        }

        private void viewRolesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewRolesForm vrf = new ViewRolesForm();
            vrf.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}



