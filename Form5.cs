using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Karaoke
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            label1.Text = "label1";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog choofdlog = new OpenFileDialog(); //allow the user to select a single file to upload
            choofdlog.Filter = "(*.mp4)|*.mp4"; //note to self: change file types
            choofdlog.FilterIndex = 1;
            choofdlog.Multiselect = false;

            if (choofdlog.ShowDialog() == DialogResult.OK)
            {
                label1.Text = choofdlog.FileName;
            }
            Close();
        }

        public string NewFileLoc
        {
            get
            {
                return label1.Text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
