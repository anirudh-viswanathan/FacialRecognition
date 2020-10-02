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
    public partial class Attendance : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\admin\Desktop\HostelSecurityFace\HostelSecurityFace\hostelfacedb.mdf;Integrated Security=True;User Instance=True");
        SqlCommand cmd;

        public Attendance()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand("select Regno,Name,Department,Year from regtb  ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value) == true)
                {
                    // MessageBox.Show("OK!");


                    cmd = new SqlCommand("insert into atttb values('" + row.Cells[1].Value + "','" + row.Cells[2].Value + "','" + row.Cells[3].Value + "','" + row.Cells[4].Value + "','"+dateTimePicker1.Text +"','1')", con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();


                }

                else
                {

                    cmd = new SqlCommand("insert into atttb values('" + row.Cells[1].Value + "','" + row.Cells[2].Value + "','" + row.Cells[3].Value + "','" + row.Cells[4].Value + "','" + dateTimePicker1.Text + "','0')", con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();


                }

            }



            MessageBox.Show("Record Saved!");
        }
    }
}
