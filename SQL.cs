using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sistema_Inventarios
{
    class SQL
    {
        SqlConnection conn;

        static string server = "localhost, 1433";
        static string user = "admin";
        static string password = "E5C7K10fl7Xu";
        static string database = "INVENTARIOS1";
        static string cadCon = "Server = " + server + ";"
                             + "User Id = " + user + ";"
                             + "Password = " + password + ";"
                             + "DataBase = " + database + ";";
        public SqlConnection connect()
        {
            try
            {
                conn = new SqlConnection(cadCon);
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexion. " + ex.ToString());
            }
            return conn;
        }
        public SqlConnection getConn()
        {
            return conn;
        }
    }
}
