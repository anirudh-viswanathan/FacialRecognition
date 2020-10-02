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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminLogin al = new AdminLogin();
            al.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            StudentLogin sl = new StudentLogin();
            sl.Show();
        }
    }
}
