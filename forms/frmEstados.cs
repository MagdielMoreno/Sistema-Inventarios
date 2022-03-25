using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; 
 
namespace Sistema_Inventarios 
{
    public partial class frmEstados : Form
    {
        public frmEstados()
        {
            InitializeComponent();
        }

        public SqlDataAdapter BDControl, BDCiudades;
        public DataSet TBControl, TBCiudades;
        public string conexion, nomCiu, con;
        public int idCiu;
        public SqlCommand cmd;
        SQL sql = new SQL();

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            if (BtnRegistrar.Text == "&Registrar")
            {
                cmd = new SqlCommand("INSERT INTO Estados VALUES ('') ; SELECT SCOPE_IDENTITY()", sql.getConn());
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                TxtNombre.Text = "";
                BtnPrimero.Enabled = false;
                BtnUltimo.Enabled = false;
                BtnSiguiente.Enabled = false;
                BtnAnterior.Enabled = false;
                BtnEliminar.Enabled = false;
                BtnActualizar.Enabled = false;
                BtnSalir.Enabled = false;
                TxtId.Text = Convert.ToString(id);
                BtnRegistrar.Text = "Aceptar";
            }
            else
            {
                cmd.CommandText = "UPDATE Estados SET Nombre='" + TxtNombre.Text + "' WHERE Id=" + TxtId.Text;
                cmd.ExecuteNonQuery();
                BtnPrimero.Enabled = true;
                BtnUltimo.Enabled = true;
                BtnSiguiente.Enabled = true;
                BtnAnterior.Enabled = true;
                BtnEliminar.Enabled = true;
                BtnActualizar.Enabled = true;
                BtnSalir.Enabled = true;
                BtnRegistrar.Text = "&Registrar";
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                idCiu = Convert.ToInt32(TxtId.Text);
                string StrControl = "DELETE FROM Estados WHERE Id = " + idCiu + ";";
                //cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idCiu;
                cmd.Parameters.AddWithValue("@Id", idCiu);
                cmd = new SqlCommand(StrControl, sql.connect());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registro eliminado exitosamente.");
                visualizaDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void BtnUltimo_Click(object sender, EventArgs e)
        {
            try
            {
                string StrControl = "SELECT TOP 1 * FROM Estados ORDER BY Id DESC;";
                cmd = new SqlCommand(StrControl, sql.connect());
                BDControl = new SqlDataAdapter(cmd);
                TBControl = new DataSet();
                BDControl.Fill(TBControl, "Control");
                Registro = TBControl.Tables["Control"].Rows[0];
                visualizaDatos();
            }
            catch
            {
                MessageBox.Show("Error.");
            }
        }



        private void BtnAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                string StrControl = "SELECT top 1 * FROM Estados WHERE Id < @IdLess ORDER BY Id DESC";
                cmd = new SqlCommand(StrControl, sql.connect());

                cmd.Parameters.Add("@IdLess", SqlDbType.Int).Value = Convert.ToInt32(TxtId.Text);

                BDControl = new SqlDataAdapter(cmd);
                TBControl = new DataSet();
                BDControl.Fill(TBControl, "Control");
                Registro = TBControl.Tables["Control"].Rows[0];
                visualizaDatos();
            }
            catch
            {
                MessageBox.Show("Este es el primer registro.");
            }
        }

        private void BtnSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                string StrControl = "SELECT * FROM Estados WHERE Id > @IdPlus";
                cmd = new SqlCommand(StrControl, sql.connect());

                cmd.Parameters.Add("@IdPlus", SqlDbType.Int).Value = Convert.ToInt32(TxtId.Text);

                BDControl = new SqlDataAdapter(cmd);
                TBControl = new DataSet();
                BDControl.Fill(TBControl, "Control");
                Registro = TBControl.Tables["Control"].Rows[0];
                visualizaDatos();
            }
            catch
            {
                MessageBox.Show("No existen más ciudades registradas.");
            }
        }

        private void BtnPrimero_Click(object sender, EventArgs e)
        {
            try
            {
                string StrControl = "SELECT top 1 * FROM Estados";
                cmd = new SqlCommand(StrControl, sql.connect());
                BDControl = new SqlDataAdapter(cmd);
                TBControl = new DataSet();
                BDControl.Fill(TBControl, "Control");
                Registro = TBControl.Tables["Control"].Rows[0];
                visualizaDatos();
            }
            catch
            {
                MessageBox.Show("Error.");
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            cmd.CommandText = "UPDATE Estados SET Nombre='" + TxtNombre.Text + "' WHERE Id=" + TxtId.Text;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Registro Actualizado");
        }

        public DataRow Registro, RegCiu;

        private void Frm_Estados_Load(object sender, EventArgs e)
        {
            string StrControl = "SELECT * FROM Estados";
            cmd = new SqlCommand(StrControl, sql.connect());
            BDControl = new SqlDataAdapter(cmd);
            TBControl = new DataSet();
            BDControl.Fill(TBControl, "Control");
            Registro = TBControl.Tables["Control"].Rows[0];
            visualizaDatos();
        }

        public void visualizaDatos()
        {
            TxtNombre.Text = Convert.ToString(Registro["Nombre"]);
            TxtId.Text = Convert.ToString(Registro["Id"]);
        }

        private void TxtId_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
