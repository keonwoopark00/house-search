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
    public partial class CreateAccount : Form
    {
        projectEntities context = new projectEntities();

        public CreateAccount()
        {
            InitializeComponent();
        }

        private void IntializeInputs()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            comboType.Text = "User Type";
        }

        private void TxtEnter_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            User user = context.Users.Find(username);

            if (user != null)
            {
                MessageBox.Show(username + " already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsername.Text = "";
                txtUsername.Focus();
            }
            else
            {
                User usr = new User();
                switch (comboType.SelectedIndex)
                {
                    case 0:
                        usr.username = username;
                        usr.password = password;
                        usr.user_type = "admin";
                        context.Users.Add(usr);
                        context.SaveChanges();
                        MessageBox.Show("A user is created.", "Success");
                        IntializeInputs();
                        break;
                    case 1:
                        usr.username = username;
                        usr.password = password;
                        usr.user_type = "normal";
                        context.Users.Add(usr);
                        context.SaveChanges();
                        MessageBox.Show("A user is created.", "Success");
                        IntializeInputs();
                        break;
                    default:
                        MessageBox.Show("Please select the user type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        comboType.Focus();
                        break;
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Hide();
        }
    }
}
