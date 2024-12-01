using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace IS_Diplom
{
    internal class SqlConnection
    {

        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Database = IS_diplom;User Id=postgres;Password = 1234;Port=5433");

        public void openConn()
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
        }
        public void closeConn()
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
        public NpgsqlConnection getConnection()
        {
            return conn;
        }
    }
}
