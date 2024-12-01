using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace IS_Diplom
{
    public partial class FormOrder : Form
    {
        public FormOrder()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            SqlConnection db = new SqlConnection();
            NpgsqlCommand cmnd = new NpgsqlCommand("INSERT into public.\"Order\" (name,name_prod,typ,weght,capacity,amount,transport_id,status) values (@name,@namep,@typ,@weght,@capacity,@amount,@transport_id,@status)", db.getConnection());
            cmnd.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox1.Text;
            cmnd.Parameters.Add("@namep", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox2.Text;
            cmnd.Parameters.Add("@typ", NpgsqlTypes.NpgsqlDbType.Varchar).Value = comboBox1.Text;
            cmnd.Parameters.Add("@weght", NpgsqlTypes.NpgsqlDbType.Integer).Value = int.Parse(textBox4.Text);
            cmnd.Parameters.Add("@capacity", NpgsqlTypes.NpgsqlDbType.Integer).Value = int.Parse(textBox5.Text);
            cmnd.Parameters.Add("@amount", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox6.Text;
            cmnd.Parameters.Add("@transport_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = 1;
            //cmnd.Parameters.Add("@document", NpgsqlTypes.NpgsqlDbType.Bytea).Value = "NULL";
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
            dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                String path = dialog.FileName;
                using (StreamReader  reader = new StreamReader(new FileStream(path, FileMode.Open), new UTF8Encoding())) // do anything you want, e.g. read it
                {
                    var fileContent = reader.ReadToEnd();
                };
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


        private void txtCheck()
        {
            if (textBox9.Text != "" || textBox10.Text != "" || textBox11.Text != "" || textBox6.Text != "")
            {
                float c = count();
                textBox5.Text = c.ToString();
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

    }
}
