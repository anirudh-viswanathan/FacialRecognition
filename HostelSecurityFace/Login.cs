using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Luxand;
using System.Data.SqlClient;

using System.IO;
using System.Net;
using System.Net.Mail;
using System.Drawing.Imaging;



namespace HostelSecurityFace
{
    public partial class Login : Form
    {
        public string uname,type;

        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\admin\Desktop\HostelSecurityFace\HostelSecurityFace\hostelfacedb.mdf;Integrated Security=True;User Instance=True");
        SqlCommand cmd;

        // program states: whether we recognize faces, or user has clicked a face
        enum ProgramState { psRemember, psRecognize }
        ProgramState programState = ProgramState.psRecognize;
        string path = Path.GetDirectoryName(Application.ExecutablePath).ToString();
        String cameraName;
        bool needClose = false;
        string userName;
        String TrackerMemoryFile;
        int mouseX = 0;
        int mouseY = 0;

        // WinAPI procedure to release HBITMAP handles returned by FSDKCam.GrabFrame
        [DllImport("gdi32.dll")]
        static extern bool DeleteObject(IntPtr hObject);



        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

            label4.Text = uname;

            // MessageBox.Show(uname);

            TrackerMemoryFile = path + @"\tracker.dat";
          // TrackerMemoryFile = "D:\\tracker.dat";


            //con.Open();
            //cmd = new SqlCommand("select * from facetb where Name='" + uname + "' ", con);
            //SqlDataReader dr1 = cmd.ExecuteReader();
            //if (dr1.Read())
            //{
            //    byte[] photodat = (byte[])dr1["Image"];
            //    MemoryStream ms = new MemoryStream(photodat);
            //    File.WriteAllBytes(TrackerMemoryFile, photodat);
            //}
            //con.Close();



            if (FSDK.FSDKE_OK != FSDK.ActivateLibrary("gyYgVWQTSzjiuGB/hH8dKgg0QrrIuhoHdfUCzD9rY+vru3WRZsaezTX6YWj9osdI/cmxY1NSdLkyWuugMPCxUG7/xNLegHLeaUpzVyKpDkaWL8tJIUsIL7xv9bhmgifPbAyTDuxF3VGxXmHkv/L/MStf9kdXV/A1vVvT93QC4vQ="))
            {
                MessageBox.Show("Please run the License Key Wizard (Start - Luxand - FaceSDK - License Key Wizard)", "Error activating FaceSDK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            FSDK.InitializeLibrary();
            FSDKCam.InitializeCapturing();

            string[] cameraList;
            int count;
            FSDKCam.GetCameraList(out cameraList, out count);

            if (0 == count)
            {
                MessageBox.Show("Please attach a camera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            cameraName = cameraList[0];

            FSDKCam.VideoFormatInfo[] formatList;
            FSDKCam.GetVideoFormatList(ref cameraName, out formatList, out count);

            int VideoFormat = 0; 

            int tracker = 0; 	// creating a Tracker
            if (FSDK.FSDKE_OK != FSDK.LoadTrackerMemoryFromFile(ref tracker, TrackerMemoryFile)) // try to load saved tracker state
                FSDK.CreateTracker(ref tracker);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            int cameraHandle = 0;

            int r = FSDKCam.OpenVideoCamera(ref cameraName, ref cameraHandle);
            if (r != FSDK.FSDKE_OK)
            {
                MessageBox.Show("Error opening the first camera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            int tracker = 0; 	// creating a Tracker
            if (FSDK.FSDKE_OK != FSDK.LoadTrackerMemoryFromFile(ref tracker, TrackerMemoryFile)) // try to load saved tracker state
                FSDK.CreateTracker(ref tracker); // if could not be loaded, create a new tracker

            int err = 0; // set realtime face detection parameters
            FSDK.SetTrackerMultipleParameters(tracker, "HandleArbitraryRotations=false; DetermineFaceRotationAngle=false; InternalResizeWidth=100; FaceDetectionThreshold=5;", ref err);

            while (!needClose)
            {
                Int32 imageHandle = 0;
                if (FSDK.FSDKE_OK != FSDKCam.GrabFrame(cameraHandle, ref imageHandle)) // grab the current frame from the camera
                {
                    Application.DoEvents();
                    continue;
                }
                FSDK.CImage image = new FSDK.CImage(imageHandle);

                long[] IDs;
                long faceCount = 0;
                FSDK.FeedFrame(tracker, 0, image.ImageHandle, ref faceCount, out IDs, sizeof(long) * 256); // maximum of 256 faces detected
                Array.Resize(ref IDs, (int)faceCount);

                // make UI controls accessible (to find if the user clicked on a face)
                Application.DoEvents();

                Image frameImage = image.ToCLRImage();
                Graphics gr = Graphics.FromImage(frameImage);

                for (int i = 0; i < IDs.Length; ++i)
                {
                    FSDK.TFacePosition facePosition = new FSDK.TFacePosition();
                    FSDK.GetTrackerFacePosition(tracker, 0, IDs[i], ref facePosition);

                    int left = facePosition.xc - (int)(facePosition.w * 0.6);
                    int top = facePosition.yc - (int)(facePosition.w * 0.5);
                    int w = (int)(facePosition.w * 1.2);

                    String name;
                    int res = FSDK.GetAllNames(tracker, IDs[i], out name, 65536); // maximum of 65536 characters

                    if (FSDK.FSDKE_OK == res && name.Length > 0)
                    { // draw name
                        StringFormat format = new StringFormat();
                        format.Alignment = StringAlignment.Center;

                        gr.DrawString("", new System.Drawing.Font("Arial", 16),
                            new System.Drawing.SolidBrush(System.Drawing.Color.LightGreen),
                            facePosition.xc, top + w + 5, format);
                        label3.Text = name;
                    }
                    else
                    {
                        label3.Text = "UnKnow FACE";

                    }


                    if (type == "Out")
                    {
                        send();
                    }
                    else if (type == "In")
                    {
                        send1();
                    }








                    Pen pen = Pens.LightGreen;
                    if (mouseX >= left && mouseX <= left + w && mouseY >= top && mouseY <= top + w)
                    {
                        pen = Pens.Blue;
                        if (ProgramState.psRemember == programState)
                        {
                            if (FSDK.FSDKE_OK == FSDK.LockID(tracker, IDs[i]))
                            {
                                // get the user name
                                //LiveRecognition.InputName inputName = new LiveRecognition.InputName();

                                //inputName.bmp = new Bitmap(pictureBox1.Image);


                                ////if (DialogResult.OK == inputName.ShowDialog())
                                ////{
                                //    userName = inputName.userName;
                                //    if (userName == null || userName.Length <= 0)
                                //    {
                                //        String s = "";
                                //        FSDK.SetName(tracker, IDs[i], "");
                                //        FSDK.PurgeID(tracker, IDs[i]);
                                //    }
                                //    else
                                //    {
                                //        FSDK.SetName(tracker, IDs[i], userName);
                                //    }
                                //    FSDK.UnlockID(tracker, IDs[i]);
                                //}
                            }
                        }
                    }
                    gr.DrawRectangle(pen, left, top, w, w);
                }
                programState = ProgramState.psRecognize;

                // display current frame
                pictureBox1.Image = frameImage;
                GC.Collect(); // collect the garbage after the deletion
                FSDK.SaveTrackerMemoryToFile(tracker, TrackerMemoryFile);
            }
            FSDK.SaveTrackerMemoryToFile(tracker, TrackerMemoryFile);

            FSDK.FreeTracker(tracker);

            FSDKCam.CloseVideoCamera(cameraHandle);
            FSDKCam.FinalizeCapturing();     
        }

        private void button2_Click(object sender, EventArgs e)
        {
            needClose = true;
        }




        public void send()
        {
            con.Open();
            cmd = new SqlCommand("select * from outpas where Name='" + label3.Text + "' and Date='" + DateTime.Now.ToShortDateString() + "' and outstatus='0' and Status='Approved'", con);
            SqlDataReader dr1 = cmd.ExecuteReader();
            if (dr1.Read())
            {

                string time1 = dr1["Time1"].ToString();
                mobile = dr1["Mobile"].ToString();

                string time2 = System.DateTime.Now.ToShortTimeString();



                string ddddd = System.DateTime.Now.ToShortDateString();

                DateTime d11 = Convert.ToDateTime(ddddd);

                DateTime d22 = Convert.ToDateTime(dr1["Date"].ToString());

                TimeSpan ts = d22 - d11;



                DateTime d1 = new DateTime();
                d1 = Convert.ToDateTime(time1);

                DateTime d2 = new DateTime();
                d2 = Convert.ToDateTime(time2);

                TimeSpan tms = d1.Subtract(d2);


                decimal dd = Convert.ToDecimal(tms.TotalHours.ToString());

                //  Label5.Text = dd.ToString();

                if (dd >= 0)
                {
                    MessageBox.Show("Out Time Not Start");

                }
                else
                {
                    sendmessage(mobile, "Your Son or Daughter out Time" + System.DateTime.Now.ToShortTimeString());

                 //   sendmessage(mobile, "Your Son or Daughter out Time" + System.DateTime.Now.ToShortTimeString());

                    dr1.Close();


                    cmd = new SqlCommand("update outpas set outstatus='1'  where Name='" + label3.Text + "' and Date='" + DateTime.Now.ToShortDateString() + "' and outstatus='0' ", con);
                   // con.Open();
                    cmd.ExecuteNonQuery();
                   // con.Close();

                }




            }

            else
            {
               
            }
            con.Close();

            //Response.Write("<Script> alert('" + id + "') </Script>");


        }


        string mobile;

        public void send1()
        {

            con.Open();
            cmd = new SqlCommand("select * from outpas where Name='" + label3.Text + "' and Date='" + DateTime.Now.ToShortDateString() + "' and instatus='0' and Status='Approved'", con);
            SqlDataReader dr1 = cmd.ExecuteReader();
            if (dr1.Read())
            {

                string time1 = dr1["Time2"].ToString();

                mobile = dr1["Mobile"].ToString();
                string time2 = System.DateTime.Now.ToShortTimeString();



                string ddddd = System.DateTime.Now.ToShortDateString();

                DateTime d11 = Convert.ToDateTime(ddddd);

                DateTime d22 = Convert.ToDateTime(dr1["Date"].ToString());

                TimeSpan ts = d22 - d11;



                DateTime d1 = new DateTime();
                d1 = Convert.ToDateTime(time1);

                DateTime d2 = new DateTime();
                d2 = Convert.ToDateTime(time2);

                TimeSpan tms = d1.Subtract(d2);


                decimal dd = Convert.ToDecimal(tms.TotalHours.ToString());

                //  Label5.Text = dd.ToString();

                if (dd >= 0)
                {
                    //MessageBox.Show("Out Time Not Start");

                    sendmessage(mobile, "Your Son or Daughter In On Time" + System.DateTime.Now.ToShortTimeString());

                   // sendmessage("", "Your Son or Daughter In On Time" + System.DateTime.Now.ToShortTimeString());
                    dr1.Close();


                    cmd = new SqlCommand("update outpas set instatus='1'  where Name='" + label3.Text + "' and Date='" + DateTime.Now.ToShortDateString() + "' and instatus='0' ", con);
                    // con.Open();
                    cmd.ExecuteNonQuery();
                    // con.Close();
                }
                else
                {

                    sendmessage(mobile, "Your Son or Daughter In Delay Time" + System.DateTime.Now.ToShortTimeString());


                    dr1.Close();


                    cmd = new SqlCommand("update outpas set instatus='1'  where Name='" + label3.Text + "' and Date='" + DateTime.Now.ToShortDateString() + "' and instatus='0' ", con);
                    // con.Open();
                    cmd.ExecuteNonQuery();
                    // con.Close();
                }





            }

            else
            {

            }
            con.Close();

            //Response.Write("<Script> alert('" + id + "') </Script>");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //con.Open();
            //cmd = new SqlCommand("select * from regtb where UserId='" + label3.Text + "'", con);
            //SqlDataReader dr = cmd.ExecuteReader();
            //if (dr.Read())
            //{

            //    if (dr["pin"].ToString() == "")
            //    {
            //        NewPin ii = new NewPin();
            //        ii.id = label3.Text;
            //        ii.pass = dr["Password"].ToString();


            //        ii.Show();
            //        this.Close();

            //    }
            //    else
            //    {

            //        brightnessPin iii = new brightnessPin();
            //        iii.id = dr["UserId"].ToString();

                   
            //        iii.pin = dr["Pin"].ToString();
            //        iii.Show();

            //        this.Close();
            //    }

            //}

            //else
            //{
            //    MessageBox.Show("UserName Incorret");

            //}

            //con.Close();
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
