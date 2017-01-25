using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Karaoke
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        public string LabelText
        {
            get
            {
                return label1.Text;
            }
            set
            {
                this.label1.Text = value;
            }
        }

        public string Label2Text
        {
            get
            {
                return label2.Text;
            }
            set
            {
                this.label2.Text = value;
            }
        }

       
        private void Form4_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            if (label1.Text != "label1")
            {
                axVLCPlugin21.playlist.next();
                int last = "\\".LastIndexOf(label1.Text) + 1;
                int length = label1.Text.Length;
                string SafeFileName = label1.Text.Substring(last, length - last);
                string FileName = label1.Text;
                axVLCPlugin21.playlist.add("file:///" + FileName, SafeFileName, null);
                axVLCPlugin21.volume = 100;
                axVLCPlugin21.playlist.play();
            }
        }
    }
}
