using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IS_Diplom
{
    public partial class FormGruz2 : Form
    {
        public FormGruz2()
        {
            InitializeComponent();
            Upd8();
        }

        public void Upd8()
        {
            SqlConnection db = new SqlConnection();
            db.openConn();
            NpgsqlCommand cmnd = new NpgsqlCommand("Select name,name_prod,typ,status from public.\"Order\" where status=@st", db.getConnection());

            cmnd.Parameters.Add("@st", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "Ожидание разгрузки";


            cmnd.Connection = db.getConnection();
            NpgsqlDataReader dr = cmnd.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
            }
            else
            {
            }
            cmnd.Dispose();
            db.closeConn();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection db = new SqlConnection();
            db.openConn();
            NpgsqlCommand cmnd3 = new NpgsqlCommand("Update public.\"Order\" set status=@st where name = @nameo and name_prod = @nameop", db.getConnection());
            cmnd3.Parameters.Add("@nameo", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox1.Text;
            cmnd3.Parameters.Add("@nameop", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox2.Text;
            cmnd3.Parameters.Add("@st", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "Разгруженно";
            if (cmnd3.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Статус обновлён");
                db.closeConn();
                textBox1.Text = "";
                textBox2.Text = "";
                Upd8();
            }
            else
            {
                MessageBox.Show("Произошла ошибка, повторите снова.");
                db.closeConn();
                textBox2.Text = "";
                textBox1.Text = "";
                Upd8();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
            textBox1.Text = row.Cells["name"].Value.ToString();
            textBox2.Text = row.Cells["name_prod"].Value.ToString();
        }
    }
}
