using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HouseSearch
{
    public partial class LoginForm : Form
    {
        projectEntities context = new projectEntities();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string password = txtPassword.Text;

            User usr = context.Users.Find(username);
            if (usr != null)
            {
                if(usr.password == password)
                {
                    if(usr.user_type == "admin")
                    {
                        Admin ad = new Admin();
                        ad.Show();
                        this.Hide();
                    }
                    else
                    {
                        Customer cus = new Customer();
                        cus.Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Password is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Username is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            CreateAccount form1 = new CreateAccount();
            form1.Show();
            this.Hide();
        }
    }
}
