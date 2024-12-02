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
using System.Web;
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
            Upd8();
        }


        int idt,x1,y1;
        byte[] file,file2;
        double costt;
        string nm;

        public void Upd8()
        {
            SqlConnection db = new SqlConnection();
            db.openConn();
            NpgsqlCommand cmnd = new NpgsqlCommand("Select \"Wp_numb\",\"Coord_name\",coord_x,coord_y,coord_coment from public.\"Waypoints\" where order_id is null", db.getConnection());
            cmnd.Connection = db.getConnection();
                NpgsqlDataReader dr = cmnd.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
            }
            
            cmnd.Dispose();
            db.closeConn();
        }


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

                NpgsqlCommand cmnd2 = new NpgsqlCommand("Select id from public.\"Order\" where name = @nameo and name_prod = @nameop", db.getConnection());
                cmnd2.Parameters.Add("@nameo", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox1.Text;
                cmnd2.Parameters.Add("@nameop", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox2.Text;

                string numb = cmnd2.ExecuteScalar().ToString(); 

                NpgsqlCommand cmnd3 = new NpgsqlCommand("Update public.\"Waypoints\" set order_id=@ido where order_id is null", db.getConnection());
                cmnd3.Parameters.Add("@ido", NpgsqlTypes.NpgsqlDbType.Integer).Value = int.Parse(numb);
                MessageBox.Show("Заказ сделан");
                db.closeConn();
                FormC fc = new FormC();
                fc.Show();
                this.Close();
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
            dialog.Filter = "Office Files|*.doc;*.docx|All files (*.*)|*.*";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                String path = dialog.FileName;
                file2 = File.ReadAllBytes(dialog.FileName);
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
                        label10.Text = costt.ToString();
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
                label10.Text = costt.ToString();

            }
            
        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
            textBox3.Text = row.Cells["Coord_name"].Value.ToString();
            textBox12.Text = row.Cells["coord_x"].Value.ToString();
            textBox13.Text = row.Cells["coord_y"].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlConnection db = new SqlConnection();
            NpgsqlCommand cmnd = new NpgsqlCommand("INSERT into public.\"Waypoints\" (\"Wp_numb\",\"Coord_name\",coord_x,coord_y,coord_file,coord_coment) values (@num,@name,@x,@y,@file,@comm)", db.getConnection());
            cmnd.Parameters.Add("@num", NpgsqlTypes.NpgsqlDbType.Integer).Value = int.Parse(textBox14.Text);
            cmnd.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox3.Text;
            cmnd.Parameters.Add("@x", NpgsqlTypes.NpgsqlDbType.Integer).Value = int.Parse(textBox12.Text);
            cmnd.Parameters.Add("@y", NpgsqlTypes.NpgsqlDbType.Integer).Value = int.Parse(textBox13.Text);
            if (richTextBox2.Text != "")
            {
                cmnd.Parameters.Add("@file", NpgsqlTypes.NpgsqlDbType.Bytea).Value = file2;
            }
            else {
                cmnd.Parameters.Add("@file", NpgsqlTypes.NpgsqlDbType.Bytea).Value = DBNull.Value;
            }
            cmnd.Parameters.Add("@comm", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox8.Text;

            db.openConn();
            if (cmnd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Точка поставленна сделан");              
                db.closeConn();
                textBox14.Text = "";
                textBox3.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                textBox8.Text = "";
                richTextBox2.Text = ""; 

                Upd8();
            }
            else
            {
                MessageBox.Show("Произошла ошибка, повторите снова.");
                textBox14.Text = "";
                textBox3.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                textBox8.Text = "";
                richTextBox2.Text = "";
                db.closeConn();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection db = new SqlConnection();
            NpgsqlCommand cmnd = new NpgsqlCommand("DELETE from public.\"Waypoints\" where order_id is NULL and \"Coord_name\"=@name and coord_x=@x and coord_y=@y", db.getConnection());
            cmnd.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox3.Text;
            cmnd.Parameters.Add("@x", NpgsqlTypes.NpgsqlDbType.Integer).Value = int.Parse(textBox12.Text);
            cmnd.Parameters.Add("@y", NpgsqlTypes.NpgsqlDbType.Integer).Value = int.Parse(textBox13.Text);

            db.openConn();
            if (cmnd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Точка удаленна");
                db.closeConn();
                textBox3.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                Upd8();
            }
            else
            {
                MessageBox.Show("Произошла ошибка, повторите снова.");
                db.closeConn();
                textBox3.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                Upd8();
            }
        }
    } 
}
