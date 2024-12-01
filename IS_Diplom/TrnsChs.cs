using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static IS_Diplom.FormOrder;

namespace IS_Diplom
{
    public partial class TrnsChs : Form
    {

        string t,p2;
        double w,p3;
        int c,p1;

        public TrnsChs(string a1, double a2, int a3)
        {
            InitializeComponent();
            t = a1;
            w = a2;
            c = a3;
            Upd8();
        }

        public class ReturnTrnsp
        {
            public static int idtr { get; set; }
            public static string nm { get; set; }
            public static double cst { get; set; }
        }

        public void Upd8()
        {
            SqlConnection db = new SqlConnection();
            db.openConn();
            NpgsqlCommand cmnd = new NpgsqlCommand("Select * from public.\"Transport\" where type=@typ and weight >= @weight and capacity >= @cpct", db.getConnection());

            cmnd.Parameters.Add("@typ", NpgsqlTypes.NpgsqlDbType.Varchar).Value = t;
            cmnd.Parameters.Add("@weight", NpgsqlTypes.NpgsqlDbType.Double).Value = w;
            cmnd.Parameters.Add("@cpct", NpgsqlTypes.NpgsqlDbType.Integer).Value = c;

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
                MessageBox.Show("Нету подходящего транспорта");
            }
            cmnd.Dispose();
            db.closeConn();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //    ReturnTrnsp.idtr = p1;
            //    ReturnTrnsp.nm = p2;
            //    ReturnTrnsp.cst = p3;
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
            p1 = int.Parse(row.Cells["id"].Value.ToString());
            p2 = row.Cells["name"].Value.ToString();
            p3 = double.Parse(row.Cells["cost"].Value.ToString());
            ReturnTrnsp.idtr = p1;
            ReturnTrnsp.nm = p2;
            ReturnTrnsp.cst = p3;
        }
    }
}
