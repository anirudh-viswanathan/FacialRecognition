using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.IO;



namespace HostelSecurityFace
{
    public partial class EmergencyAleart : Form
    {
        public EmergencyAleart()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\admin\Desktop\HostelSecurityFace\HostelSecurityFace\hostelfacedb.mdf;Integrated Security=True;User Instance=True");
        SqlCommand cmd;
        private void EmergencyAleart_Load(object sender, EventArgs e)
        {
            con.Open();
            cmd = new SqlCommand("select * from regtb ", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                comboBox1.Items.Add(dr["Regno"].ToString());

            }
           
            con.Close();

        }


        string name, mobile;


        string mmm;

        private void button1_Click(object sender, EventArgs e)
        {
            mmm = "";

            for (int ii = 0; ii < comboBox1.Items.Count; ii++)
            {
                string regno = comboBox1.Items[ii].ToString();


                con.Open();
                cmd = new SqlCommand("select * from regtb  where Regno='" + regno + "'", con);
                SqlDataReader dr22 = cmd.ExecuteReader();
                if (dr22.Read())
                {
                    name = dr22["Name"].ToString();
                    mobile = dr22["Mobile"].ToString();

                    if (mmm == "")
                    {
                        mmm = mobile;
                    }
                    else
                    {
                        mmm += "," + mobile;
                    }

                }


                con.Close();

                cmd = new SqlCommand("insert into outpas values('" + regno + "','" + name + "','" + dateTimePicker1.Text + "','" + dateTimePicker2.Text + "','" + dateTimePicker4.Text + "','" + dateTimePicker3.Text + "','" + textBox2.Text + "','Approved','0','0','" + mobile + "')", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }

            MessageBox.Show("Emergency Info Saved!");

            sendmessage(mmm, " Emergency Alert " + textBox2.Text);


           // label2.Text = mmm;


        }

        public void sendmessage(string targetno, string message)
        {

            String query = "http://bulksms.mysmsmantra.com:8080/WebSMS/SMSAPI.jsp?username=fantasy5535&password=1163974702&sendername=Sample&mobileno=" + targetno + "&message=" + message;
            WebClient client = new WebClient();
            Stream sin = client.OpenRead(query);
            // Response.Write("<script> alert('Message Send') </script>");
            MessageBox.Show("Message Send");
        }
    }
}
