using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpeechLib;
using System.Data.SqlClient;
using System.Configuration;


namespace KelimeOgrenV2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static string conn = ConfigurationManager.ConnectionStrings["KelimeOgrenV2.Properties.Settings.fndcnt"].ConnectionString;
        SqlConnection con = new SqlConnection(conn);
        int a = 0;
        
        public void goster()
        {
            con.Open();

            SqlDataAdapter dap = new SqlDataAdapter("select * from words", con);
            DataTable table = new DataTable();
            dap.Fill(table);
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 250;
            con.Close();
        }
        public void gosterza()
        {
            con.Open();

            SqlDataAdapter dap = new SqlDataAdapter("select * from words order by ingilizce desc ", con);
            DataTable table = new DataTable();
            dap.Fill(table);
            dataGridView1.DataSource = table;
            con.Close();
        }
        public void gosteraz()
        {
            con.Open();

            SqlDataAdapter dap = new SqlDataAdapter("select * from words order by ingilizce asc ", con);
            DataTable table = new DataTable();
            dap.Fill(table);
            dataGridView1.DataSource = table;
            con.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SpVoice seslendir = new SpVoice();
            seslendir.Speak(textBox1.Text, SpeechVoiceSpeakFlags.SVSFDefault);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //goster();
            dataGridView1.Visible = false;
            button2.Visible = false;
            timer1.Start();
            this.Size = new System.Drawing.Size(517, 200);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from words where ingilizce like '%" + textBox1.Text + "%'", con);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            con.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show(" sıra no : " + dataGridView1.CurrentRow.Cells[0].Value.ToString() + Environment.NewLine+" İNGİLİZCE : " + dataGridView1.CurrentRow.Cells[1].Value.ToString() +Environment.NewLine+ " TÜRKÇE KARŞILIĞI : " + dataGridView1.CurrentRow.Cells[2].Value.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            goster();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            int rs = r.Next(0, 24027);
            con.Open();
            SqlCommand command = new SqlCommand("select [turkce],[ingilizce] from words where [id] =" + rs);
            command.Connection = con;
            SqlDataReader sec = command.ExecuteReader();
            while (sec.Read())
            {
                lblturkce.Text = sec[0].ToString();
                lblingilizce.Text = sec[1].ToString();
            }
            sec.Close();
          


            con.Close();
            timer1.Stop();

            SpVoice seslendir = new SpVoice();
            seslendir.Speak(lblingilizce.Text, SpeechVoiceSpeakFlags.SVSFDefault);
            timer1.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(517, 782);
            dataGridView1.Visible = true;
            goster();
          if (a == 0)
            {
                timer1.Stop();
                goster();
                a = 1;
                button3.Text = "Otomatik Seçimi Başlat";
            }
         else if (a == 1)
            {
                timer1.Start();
                a = 0;
                button3.Text = "Otomatik Seçimi Durdur";
                this.Size = new System.Drawing.Size(517, 200);
            }
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Size = new System.Drawing.Size(517, 782);
            goster();
            dataGridView1.Visible = true;
           
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            gosterza();

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            gosteraz();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into words (ingilizce,turkce) values ('" + txtyening.Text + "','" + txtyentrk.Text + "') ", con);
            cmd.ExecuteNonQuery();
            con.Close();
            this.Size = new System.Drawing.Size(517, 200);
            txtyening.Text = "";
            txtyentrk.Text = "";
            timer1.Start();

        }
    }
}
