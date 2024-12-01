using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static IS_Diplom.TrnsChs;

namespace IS_Diplom
{
    public partial class FormOrder : Form
    {
        public FormOrder()
        {
            InitializeComponent();
        }


        int idt;
        byte[] file;
        double costt;

        private void button1_Click(object sender, EventArgs e)
        {

            SqlConnection db = new SqlConnection();
            NpgsqlCommand cmnd = new NpgsqlCommand("INSERT into public.\"Order\" (name,name_prod,typ,weght,capacity,amount,transport_id,documents,status) values (@name,@namep,@typ,@weght,@capacity,@amount,@transport_id,@document,@status)", db.getConnection());
            cmnd.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox1.Text;
            cmnd.Parameters.Add("@namep", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox2.Text;
            cmnd.Parameters.Add("@typ", NpgsqlTypes.NpgsqlDbType.Varchar).Value = comboBox1.Text;
            cmnd.Parameters.Add("@weght", NpgsqlTypes.NpgsqlDbType.Integer).Value = int.Parse(textBox4.Text);
            cmnd.Parameters.Add("@capacity", NpgsqlTypes.NpgsqlDbType.Integer).Value = int.Parse(textBox5.Text);
            cmnd.Parameters.Add("@amount", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox6.Text;
            cmnd.Parameters.Add("@transport_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = idt;
            cmnd.Parameters.Add("@document", NpgsqlTypes.NpgsqlDbType.Bytea).Value = file;
            cmnd.Parameters.Add("@status", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "Проверяется";

            db.openConn();
            if (cmnd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Заказ сделан");
                db.closeConn();
            }
            else
            {
                MessageBox.Show("Произошла ошибка, повторите снова.");
                db.closeConn();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                String path = dialog.FileName;
                using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open), new UTF8Encoding())) // do anything you want, e.g. read it
                {
                    var fileContent = reader.ReadToEnd();
                };
                this.richTextBox2.Text = path;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Office Files|*.doc;*.docx|All files (*.*)|*.*";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                String path = dialog.FileName;
                file = File.ReadAllBytes(dialog.FileName);
                this.richTextBox1.Text = path;
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            txtCheck();
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            txtCheck();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            txtCheck();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            txtCheck();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            trnspChk();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            trnspChk();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            trnspChk();
        }

        private void trnspChk()
        {
            if (checkBox1.Checked == true && textBox5.Text != String.Empty && textBox4.Text != String.Empty && comboBox1.Text != String.Empty)
            {
                SqlConnection db = new SqlConnection();
                NpgsqlCommand cmnd = new NpgsqlCommand("SELECT id, name, cost From public.\"Transport\" Where type=@typ and weight = (select min(weight) from public.\"Transport\" where weight >= @weight and capacity = (select min(capacity) from public.\"Transport\" where capacity >= @cpct)) or capacity = (select min(capacity) from public.\"Transport\" where capacity >= @cpct  and weight = (select min(weight) from public.\"Transport\" where weight >= @weight));");
                cmnd.Parameters.Add("@typ", NpgsqlTypes.NpgsqlDbType.Varchar).Value = comboBox1.Text;
                cmnd.Parameters.Add("@weight", NpgsqlTypes.NpgsqlDbType.Double).Value = double.Parse(textBox4.Text);
                cmnd.Parameters.Add("@cpct", NpgsqlTypes.NpgsqlDbType.Integer).Value = int.Parse(textBox5.Text);

                db.openConn();
                cmnd.Connection = db.getConnection();
                NpgsqlDataReader dr = cmnd.ExecuteReader();
                if (dr.HasRows)
                {
                    string namet;
                    while (dr.Read())
                    {
                        idt = dr.GetFieldValue<int>(0);
                        namet = dr.GetFieldValue<string>(1);
                        textBox7.Text = namet;
                        costt = dr.GetFieldValue<double>(2);
                    }
                   
                    db.closeConn();
                }
                else
                {
                    MessageBox.Show("Произошла ошибка, повторите снова.");
                    db.closeConn();
                }

            }
        }
        
        private void txtCheck()
        {
            if (textBox9.Text != String.Empty && textBox10.Text != String.Empty && textBox11.Text != String.Empty && textBox6.Text != String.Empty)
            {
                float c = count();
                textBox5.Text = c.ToString();
            }
            else
            {
                return;
            }
        }

        private float count()
        {
            int ln, wth, hght, am;
            float cap,k;

            ln = int.Parse(textBox9.Text);
            wth = int.Parse(textBox10.Text);
            hght = int.Parse(textBox11.Text);
            am = int.Parse(textBox6.Text);

            k = ln * wth * hght;

            cap = k * am;

            return cap;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            trnspChk();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false && textBox5.Text != String.Empty && textBox4.Text != String.Empty && comboBox1.Text != String.Empty)
            {
                string a1 = comboBox1.Text;
                double a2 = double.Parse(textBox4.Text);
                int a3 = int.Parse(textBox5.Text);
                TrnsChs fc = new TrnsChs(a1, a2, a3);
                fc.ShowDialog();
                idt = ReturnTrnsp.idtr;
                costt = ReturnTrnsp.cst;
                textBox7.Text = ReturnTrnsp.nm;

            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    } 
}
