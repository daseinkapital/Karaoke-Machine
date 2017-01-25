using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace Karaoke
{
    public partial class Form1 : Form
    {
        SQLiteConnection m_dbConnection; //establish a database connection

        public Form1()
        {
            InitializeComponent();
            m_dbConnection = new SQLiteConnection("Data Source=KaraokeDataBase.sqlite; Version=3"); //give database connection string in public
            m_dbConnection.Open(); //Open connection to SQLite
            if (File.Exists(@"KaraokeDataBase.sqlite")) //check whether or not DataBase exists
            {
                string query = "SELECT * FROM videos"; //populate database with all current songs
                SQLiteCommand command3 = new SQLiteCommand(query, m_dbConnection);
                command3.CommandText = query;
                SQLiteDataAdapter da = new SQLiteDataAdapter(command3.CommandText, m_dbConnection);
                da.Fill(dataTable);

                dataGridView1.DataSource = dataTable; //display database results
                SQLiteDataAdapter dataAdapt = new SQLiteDataAdapter(query, m_dbConnection);
                SQLiteCommandBuilder commandBuild = new SQLiteCommandBuilder(dataAdapt);
                DataTable tableTest = new DataTable();
                tableTest.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapt.Fill(tableTest);
                dataGridView1.DataSource = tableTest;
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }//Check to see if database exists in root directory. If it is, load the database
            else
            {
                SQLiteConnection.CreateFile("KaraokeDataBase.sqlite"); //if it doesn't, create the database
            }
        }

        public Form4 f4 = null; //tbh don't remember why I put this here but it's probably important

        public void MicListen(int deviceNumber)
        {
            if (sourceStream == null)
            {
                sourceStream = new NAudio.Wave.WaveIn();
                sourceStream.DeviceNumber = deviceNumber;
                sourceStream.WaveFormat = new NAudio.Wave.WaveFormat(44100, NAudio.Wave.WaveIn.GetCapabilities(deviceNumber).Channels);

                NAudio.Wave.WaveInProvider waveIn = new NAudio.Wave.WaveInProvider(sourceStream);

                waveOut = new NAudio.Wave.DirectSoundOut();
                waveOut.Init(waveIn);

                sourceStream.StartRecording();
                waveOut.Play();
            }
        }

        public void MicIgnore()
        {
            if (waveOut != null) //end the microphone process (if this is removed multiple instances of the microphone will be opened)
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Screen[] sc; //load up the video player on startup
            sc = Screen.AllScreens;

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            Form4 f4 = new Form4();

            try
            {
                string DoesStringExist = sc[1].ToString(); //I'm not sure why I did this either, but also probably important
                f4.FormBorderStyle = FormBorderStyle.None; //check to see if second screen exists
                f4.Left = sc[1].Bounds.Left;
                f4.Top = sc[1].Bounds.Top;
                f4.StartPosition = FormStartPosition.Manual;
            }
            catch (IndexOutOfRangeException)
            {
                f4.FormBorderStyle = FormBorderStyle.None; //if second screen doesn't exist, use primary screen
                f4.Left = sc[0].Bounds.Left;
                f4.Top = sc[0].Bounds.Top;
                f4.StartPosition = FormStartPosition.Manual;
            }
            f4.Show(); //open video player form
        }

        private DataTable dataTable = new DataTable();
        private void button1_Click(object sender, EventArgs e) //add song button
        {
            Form3 f3 = new Form3(); //open form to add songs
            f3.ShowDialog(); //Open the form to add song information
            string Path = f3.Box1; //save all user inputted values from Form3
            string Song = f3.Box2;
            string Artist = f3.Box3;
            string Album = f3.Box4;
            string Genre = f3.Box5;
            string Type = f3.Box6;
            DateTime now = DateTime.Now;
            string DateNow = now.ToString("G"); //formats the current date and time
            if ((Path != "") & (Song != "") & (Artist != "") & (Album != "") & (Genre != "") & (Type != "")) //if they are all non-zero, insert them into the database
            {
                string sql1 = "INSERT INTO videos (Song, Artist, Album, Genre, Type, FileLoc, DateAdded) VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7)"; //inserts saved data
                string sql2 = "CREATE TABLE IF NOT EXISTS videos (Song VARCHAR(100), Artist VARCHAR(100), Album VARCHAR(100), Genre VARCHAR(50), Type VARCHAR(25), FileLoc VARCHAR(250), DateAdded VARCHAR(30))"; // if the table doesn't exist, create it
                SQLiteCommand command1 = new SQLiteCommand(sql2, m_dbConnection);
                SQLiteCommand command2 = new SQLiteCommand(sql1, m_dbConnection);
                command1.ExecuteNonQuery(); //execute table creation
                command2.CommandText = sql1;
                command2.CommandType = CommandType.Text;
                command2.Parameters.Add(new SQLiteParameter("@param1", Song)); //establish parametric referencing for variables inserted in to database
                command2.Parameters.Add(new SQLiteParameter("@param2", Artist));
                command2.Parameters.Add(new SQLiteParameter("@param3", Album));
                command2.Parameters.Add(new SQLiteParameter("@param4", Genre));
                command2.Parameters.Add(new SQLiteParameter("@param5", Type));
                command2.Parameters.Add(new SQLiteParameter("@param6", Path));
                command2.Parameters.Add(new SQLiteParameter("@param7", DateNow));
                command2.ExecuteNonQuery(); //insert data into database

                string query = "SELECT * FROM videos";
                SQLiteCommand command3 = new SQLiteCommand(query, m_dbConnection);
                command3.CommandText = query;
                SQLiteDataAdapter da = new SQLiteDataAdapter(command3.CommandText, m_dbConnection);
                da.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
                SQLiteDataAdapter dataAdapt = new SQLiteDataAdapter(query, m_dbConnection);
                SQLiteCommandBuilder commandBuild = new SQLiteCommandBuilder(dataAdapt);
                DataTable tableTest = new DataTable();
                tableTest.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapt.Fill(tableTest);
                dataGridView1.DataSource = tableTest;
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }
        }

        private void button2_Click(object sender, EventArgs e) //select audio device button
        {
            Form2 f2 = new Form2(); //open Audio Device selector
            f2.ShowDialog();
            String Mic = f2.Mic;  //saves the selected microphone to form1
        }


        private NAudio.Wave.WaveIn sourceStream = null; //initialize empty audio inputs
        private NAudio.Wave.DirectSoundOut waveOut = null;

        private void button3_Click(object sender, EventArgs e) //play button
        {
            if (waveOut != null) //end the microphone process (if this is removed multiple instances of the microphone will be opened)
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

            button2.Enabled = false; //don't let user open Select Audio Device while video player playing
            button4.Enabled = true; //enable stop button once video starts playing

            if (label2.Text == "label2") //90% sure this is irrelevant because I set a default microphone, but you never know when you might need it
            {
                MessageBox.Show("Please select an audio device before playing a video.", "No Audio Device Selected.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows) // grab the selected song's file location 
                { //not sure why I used a foreach when it's literally a single row that is selected
                    label1.Text = row.Cells[5].Value.ToString();
                }
                if (File.Exists(label1.Text)) //make sure that the file path leads somewhere
                {
                    Screen[] sc; // initialize screens
                    sc = Screen.AllScreens;

                    Form4 f4 = new Form4(); //create new form and update with any new information
                    f4.LabelText = FileLoc;
                    f4.Label2Text = MicNum;
                    f4.Update(); //one of these is probably redundant but you can never be too safe
                    f4.Refresh();

                    try
                    {
                        string DoesStringExist = sc[1].ToString(); //same as form load function
                        f4.FormBorderStyle = FormBorderStyle.None;
                        f4.Left = sc[1].Bounds.Left;
                        f4.Top = sc[1].Bounds.Top;
                        f4.StartPosition = FormStartPosition.Manual;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        f4.FormBorderStyle = FormBorderStyle.None;
                        f4.Left = sc[0].Bounds.Left;
                        f4.Top = sc[0].Bounds.Top;
                        f4.StartPosition = FormStartPosition.Manual;
                    }

                    while (Application.OpenForms.Count > 1) //if other forms are open (specifically instance of the video player) close them
                    {
                        Application.OpenForms[Application.OpenForms.Count - 1].Close();
                    }
                    if (!f4.Visible) //if there are no form4s, create one
                    {
                        f4.Show();
                            int deviceNumber = Int32.Parse(label2.Text); //utilize selected audio device and start listening on microphone
                            if (sourceStream == null)
                            {
                                sourceStream = new NAudio.Wave.WaveIn();
                                sourceStream.DeviceNumber = deviceNumber;
                                sourceStream.WaveFormat = new NAudio.Wave.WaveFormat(44100, NAudio.Wave.WaveIn.GetCapabilities(deviceNumber).Channels);

                                NAudio.Wave.WaveInProvider waveIn = new NAudio.Wave.WaveInProvider(sourceStream);

                                waveOut = new NAudio.Wave.DirectSoundOut();
                                waveOut.Init(waveIn);

                                sourceStream.StartRecording();
                                waveOut.Play();
                            }
                    }
                }
                else
                {
                    Form5 f5 = new Form5(); //if the filepath doesn't exist create a new dialog for reestablishing or deleting the file
                    f5.ShowDialog();
                    if (f5.NewFileLoc == "label1")
                    {
                        string sql = "DELETE FROM videos WHERE FileLoc = @param1";
                        SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                        command.CommandText = sql;
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SQLiteParameter("@param1", label1.Text)); //delete the selected file
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        string sql = "UPDATE videos SET FileLoc = @param1 WHERE FileLoc = @param2";
                        SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                        command.CommandText = sql;
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SQLiteParameter("@param1", f5.NewFileLoc));
                        command.Parameters.Add(new SQLiteParameter("@param2", label1.Text));
                        command.ExecuteNonQuery();
                    }
                    string query = "SELECT * FROM videos";
                    SQLiteCommand command3 = new SQLiteCommand(query, m_dbConnection);
                    command3.CommandText = query;
                    SQLiteDataAdapter da = new SQLiteDataAdapter(command3.CommandText, m_dbConnection);
                    da.Fill(dataTable);

                    dataGridView1.DataSource = dataTable; //reupdate the table with whatever the user chooses from form5
                    SQLiteDataAdapter dataAdapt = new SQLiteDataAdapter(query, m_dbConnection);
                    SQLiteCommandBuilder commandBuild = new SQLiteCommandBuilder(dataAdapt);
                    DataTable tableTest = new DataTable();
                    tableTest.Locale = System.Globalization.CultureInfo.InvariantCulture;
                    dataAdapt.Fill(tableTest);
                    dataGridView1.DataSource = tableTest;
                    dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        public string FileLoc //public string for passing the selected file
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        public string MicNum //public string for passing the selected mic
        {
            get
            {
                return label2.Text;
            }
        }

        private void button4_Click(object sender, EventArgs e) //stop button
        {
            while (Application.OpenForms.Count > 1) //close video player when stop button clicked
            {
                Application.OpenForms[Application.OpenForms.Count - 1].Close();
            }

            button2.Enabled = true; //re-enable the Select Audio Device button
            button4.Enabled = false; //disable the stop button again
            if (waveOut != null) //stop the listening to the microphone
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

            Screen[] sc; //load up the video player on after closing the open video version
            sc = Screen.AllScreens;

            Form4 f4 = new Form4();

            try
            {
                string DoesStringExist = sc[1].ToString(); //I'm not sure why I did this either, but also probably important
                f4.FormBorderStyle = FormBorderStyle.None; //check to see if second screen exists
                f4.Left = sc[1].Bounds.Left;
                f4.Top = sc[1].Bounds.Top;
                f4.StartPosition = FormStartPosition.Manual;
            }
            catch (IndexOutOfRangeException)
            {
                f4.FormBorderStyle = FormBorderStyle.None; //if second screen doesn't exist, use primary screen
                f4.Left = sc[0].Bounds.Left;
                f4.Top = sc[0].Bounds.Top;
                f4.StartPosition = FormStartPosition.Manual;
            }
            f4.Show(); //open video player form
        }
    }
}
