using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Karaoke
{
    public partial class Form3 : Form
    {

        public Form3()
        {
            InitializeComponent();
        }


        private void Form3_Load(object sender, EventArgs e)
        {
            textBox2.ReadOnly = true; //don't allow user to enter info until they've selected a file
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            textBox6.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e) //selected video file button
        {
            OpenFileDialog choofdlog = new OpenFileDialog(); //allow the user to select a single file to upload
            choofdlog.Filter = "(*.mp4)|*.mp4"; //note to self: change file types
            choofdlog.FilterIndex = 1;
            choofdlog.Multiselect = false;

            if (choofdlog.ShowDialog() == DialogResult.OK)
            {
                string sFileName = choofdlog.FileName;
                textBox1.Text = sFileName;
                textBox2.ReadOnly = false; //once file has been selected, other boxes can be editted
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false;
                textBox5.ReadOnly = false;
                textBox6.ReadOnly = false;
            }
        }

        private void button3_Click(object sender, EventArgs e) //cancel button
        {
            textBox1.Text = ""; //clear all data
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            Close(); //close form
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            string AlertMessage = ""; //create alert message if data missing
            if (textBox1.Text == "")
            {
                AlertMessage += "Please choose a video file to upload. \r\n";
            }
            if (textBox2.Text == "")
            {
                AlertMessage += "Please add a song name. \r\n";
            }
            if (textBox3.Text == "")
            {
                AlertMessage += "Please add an artist name. \r\n";
            }
            if (textBox4.Text == "")
            {
                AlertMessage += "Please add an album name. \r\n";
            }
            if (textBox5.Text == "")
            {
                AlertMessage += "Please add a Genre. \r\n";
            }
            if (textBox6.Text == "")
            {
                AlertMessage += "Please add the Karaoke Maker.";
            }
            if (AlertMessage == "")
            {
                Close();
            }
            else
            {
                MessageBox.Show(AlertMessage, "Information Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            }
               
        }
        //create public strings of user inputs to pass data back to form1
        public string Box1
        {
            get
            {
                return textBox1.Text;
            }
        }

        public string Box2
        {
            get
            {
                return textBox2.Text;
            }
        }

        public string Box3
        {
            get
            {
                return textBox3.Text;
            }
        }

        public string Box4
        {
            get
            {
                return textBox4.Text;
            }
        }

        public string Box5
        {
            get
            {
                return textBox5.Text;
            }
        }

        public string Box6
        {
            get
            {
                return textBox6.Text;
            }
        }
    }
}
