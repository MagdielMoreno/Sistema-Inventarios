using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sistema_Inventarios.forms
{
    public partial class inicioSesion : Form
    {
        SQL sql = new SQL();

        public inicioSesion()
        {
            InitializeComponent();
        } 
      
        private void Login_Load(object sender, EventArgs e)
        {
            sql.connect();
            //aasdasdasdasdasdasaaasadasdasd
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string queryComm = "SELECT Usuario, Password FROM Control";
            SqlCommand cmd = new SqlCommand(queryComm, sql.getConn());
            SqlDataAdapter dbControl = new SqlDataAdapter(cmd);
            DataSet tbControl = new DataSet();
            dbControl.Fill(tbControl, "Control");
            DataRow reg = tbControl.Tables["Control"].Rows[0];
            if (txtUsuario.Text == Convert.ToString(reg["Usuario"]) && (txtContra.Text == Convert.ToString(reg["Password"])))
            {
                ventanaPrincipal ventanaPrincipal = new ventanaPrincipal();
                this.Hide();
                ventanaPrincipal.ShowDialog();
                Dispose();
                Close();
            }
            else
            {
                MessageBox.Show("Datos Incorrectos!!!");
            }
        }
    }
}
