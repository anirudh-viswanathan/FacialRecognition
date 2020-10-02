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
    public partial class NewStudent : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\admin\Desktop\HostelSecurityFace\HostelSecurityFace\hostelfacedb.mdf;Integrated Security=True;User Instance=True");
        SqlCommand cmd;

        
        public NewStudent()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox9.Text == textBox10.Text)
            {
                string gender;
                if (radioButton1.Checked == true)
                {

                    gender = radioButton1.Text;

                }
                else
                {

                    gender = radioButton2.Text;

                }

                cmd = new SqlCommand("insert into regtb values('" + reg.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + gender + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "','" + textBox7.Text + "','" + comboBox1.Text + "','" + comboBox2.Text + "','" + textBox9.Text + "')", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


                Form1 sl = new Form1();
                sl.uname = textBox2.Text;
                sl.Show();
                    

                MessageBox.Show("Record Saved!");



            }
            else
            {
                MessageBox.Show("Password Mismatch!");

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox10.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox9.Text = "";

        }

        private void NewStudent_Load(object sender, EventArgs e)
        {

        }
    }
}
