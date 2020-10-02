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
    public partial class Check : Form
    {
        public Check()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login ll = new Login();
            ll.type = comboBox1.Text;
            ll.Show();

        }
    }
}
