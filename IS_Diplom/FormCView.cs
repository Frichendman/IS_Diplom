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

namespace IS_Diplom
{
    public partial class FormCView : Form
    {
        public FormCView()
        {
            InitializeComponent();
            Upd8();
        }

        public void Upd8()
        {
            SqlConnection db = new SqlConnection();
            db.openConn();
            NpgsqlCommand cmnd = new NpgsqlCommand("Select name,name_prod,typ,status from public.\"Order\" where name=@nam", db.getConnection());

            cmnd.Parameters.Add("@nam", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "Андрей";
            

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
