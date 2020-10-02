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
    public partial class OutpassApproved : Form
    {
        public OutpassApproved()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\admin\Desktop\HostelSecurityFace\HostelSecurityFace\hostelfacedb.mdf;Integrated Security=True;User Instance=True");
        SqlCommand cmd;


        private void button1_Click(object sender, EventArgs e)
        {

            cmd = new SqlCommand("update outpas set Status='" + comboBox2.Text + "' where id='" + comboBox1.Text + "' ", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


        

            MessageBox.Show("Out Pass Approved!");


            cmd = new SqlCommand("select * from outpas ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();

        }

        private void OutpassApproved_Load(object sender, EventArgs e)
        {
            cmd = new SqlCommand("select * from outpas ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();



            con.Open();
            cmd = new SqlCommand("select * from outpas ", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                comboBox1.Items.Add(dr["id"].ToString());

            }

            con.Close();
          
        }
    }
}
