using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace HostelSecurityFace
{
    public partial class AdminHome : Form
    {
        public AdminHome()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\admin\Desktop\HostelSecurityFace\HostelSecurityFace\hostelfacedb.mdf;Integrated Security=True;User Instance=True");
        SqlCommand cmd;
        private void AdminHome_Load(object sender, EventArgs e)
        {
            cmd = new SqlCommand("select * from regtb ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
        }

        private void outPassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutpassApproved aa = new OutpassApproved();
            aa.Show();

        }

        private void logooutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void faceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Check cc = new Check();
            cc.Show();

        }

        private void attendanceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Attendance at = new Attendance();
            at.Show();

        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AttendanceReport ar = new AttendanceReport();
            ar.Show();

        }

        private void emergencyAleartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmergencyAleart ar = new EmergencyAleart();
            ar.Show();

        }
    }
}
