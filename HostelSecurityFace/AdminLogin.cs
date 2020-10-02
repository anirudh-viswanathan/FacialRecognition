using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HostelSecurityFace
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "admin" & textBox2.Text == "admin")
            {
                AdminHome ah = new AdminHome();
                ah.Show();

            }
            else
            {

                MessageBox.Show("Password Mismatch!");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void AdminLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
