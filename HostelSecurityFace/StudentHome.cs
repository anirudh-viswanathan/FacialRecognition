using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace HostelSecurityFace
{
    public partial class StudentHome : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\admin\Desktop\HostelSecurityFace\HostelSecurityFace\hostelfacedb.mdf;Integrated Security=True;User Instance=True");
        SqlCommand cmd;

        public string sname,rname,mob;

        public StudentHome()
        {
            InitializeComponent();
        }

        private void StudentHome_Load(object sender, EventArgs e)
        {
            reg.Text = rname;
            textBox1.Text = sname;


        }

        private void button1_Click(object sender, EventArgs e)
        {

            cmd = new SqlCommand("insert into outpas values('" + reg.Text + "','" + textBox1.Text + "','" + dateTimePicker1.Text + "','" + dateTimePicker2.Text + "','" + dateTimePicker4.Text + "','" + dateTimePicker3.Text + "','" + textBox2.Text + "','','0','0','" + mob + "')", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Out Pass Info saved!");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";

        }

        private void outpassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutPassInfo oo = new OutPassInfo();
            oo.rname = rname;
            oo.Show();

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Home hh = new Home();
            hh.Show();
            this.Close();

        }
    }
}
