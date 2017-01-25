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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public bool TestClick = true;

        private void button1_Click(object sender, EventArgs e)
        {
            List<NAudio.Wave.WaveInCapabilities> sources = new List<NAudio.Wave.WaveInCapabilities>();

            for (int i = 0; i < NAudio.Wave.WaveIn.DeviceCount; i++)
            {
                sources.Add(NAudio.Wave.WaveIn.GetCapabilities(i));
            }

            SourceList.Items.Clear();

            foreach (var source in sources)
            {
                ListViewItem item = new ListViewItem(source.ProductName);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, source.Channels.ToString()));
                SourceList.Items.Add(item);
            }
        }

        private NAudio.Wave.WaveIn sourceStream = null;
        private NAudio.Wave.DirectSoundOut waveOut = null;

        private void button2_Click(object sender, EventArgs e)
        {
            if (TestClick == false)
            {
                TestClick = true;
            }
            else
            {
                TestClick = false;
            }

            if (TestClick == false)
            {
                button2.Text = "Stop Test";
                if (SourceList.SelectedItems.Count == 0) return;
                int deviceNumber = SourceList.SelectedItems[0].Index;

                sourceStream = new NAudio.Wave.WaveIn();
                sourceStream.DeviceNumber = deviceNumber;
                sourceStream.WaveFormat = new NAudio.Wave.WaveFormat(44100, NAudio.Wave.WaveIn.GetCapabilities(deviceNumber).Channels);

                NAudio.Wave.WaveInProvider waveIn = new NAudio.Wave.WaveInProvider(sourceStream);

                waveOut = new NAudio.Wave.DirectSoundOut();
                
                waveOut.Init(waveIn);

                sourceStream.StartRecording();
                waveOut.Play();
            }
            else
            {
                button2.Text = "Audio Test";
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int deviceNumber = SourceList.SelectedItems[0].Index;
                label1.Text = deviceNumber.ToString();
                button4_Click(sender, e);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Please select an audio device.", "No Audio Device Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (sourceStream != null)
            {
                sourceStream.StopRecording();
                sourceStream.Dispose();
                sourceStream = null;
            }
            Close();
        }

        public string Mic
        {
            get
            {
                return label1.Text;
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
