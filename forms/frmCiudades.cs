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
    public partial class frmCiudades : Form 
    {
        public frmCiudades()
        {
            InitializeComponent(); 
        }

        public SqlDataAdapter BDControl, BDCiudades;
        public DataSet TBControl, TBCiudades;
        public string conexion, nomCiu, con;
        public int idCiu;
        public SqlCommand cmd;
        public DataRow Registro, RegCiu, Reg2;
        SQL sql = new SQL();
        int pos = 0;

        private void BtnPrimero_Click(object sender, EventArgs e)
        {
            pos = 0;
            Registro = TBControl.Tables["Control"].Rows[pos];
            visualizaDatos();
            /*
            try
            {
                string StrControl = "SELECT top 1 * FROM Ciudades";
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
            */
        }

        private void BtnSiguiente_Click(object sender, EventArgs e)
        {
            if (pos >= TBControl.Tables["Control"].Rows.Count - 1)
            {
                MessageBox.Show("No existen más ciudades registradas.");
            }
            else
            {
                pos += 1;
                Registro = TBControl.Tables["Control"].Rows[pos];
            }
            visualizaDatos();
            /*
            try
            {
                
                string StrControl = "SELECT * FROM Ciudades WHERE Id > @IdPlus";
                cmd = new SqlCommand(StrControl, sql.connect());

                cmd.Parameters.Add("@IdPlus", SqlDbType.Int).Value = Convert.ToInt32(TxtId.Text);

                BDControl = new SqlDataAdapter(cmd);
                TBControl = new DataSet();
                BDControl.Fill(TBControl, "Control");
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("No existen más ciudades registradas.");
            }
            */
        }

        private void BtnAnterior_Click(object sender, EventArgs e)
        {
            if (pos == 0)
            {
                MessageBox.Show("Este es el primer registro.");
            }
            else
            {
                pos -= 1;
                Registro = TBControl.Tables["Control"].Rows[pos];
            }
            visualizaDatos();
            /*
            try
            {

                string StrControl = "SELECT top 1 * FROM Ciudades WHERE Id < @IdLess ORDER BY Id DESC";
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
            */
        }
         
        private void BtnUltimo_Click(object sender, EventArgs e)
        {
            pos = TBControl.Tables["Control"].Rows.Count - 1;
            Registro = TBControl.Tables["Control"].Rows[pos];
            visualizaDatos();
            /*
            try
            {
                string StrControl = "SELECT TOP 1 * FROM Ciudades ORDER BY Id DESC;";
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
            */
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            int idEdo = 1;
            string str2 = "SELECT * FROM Estados";
            SqlCommand cmd2 = new SqlCommand(str2, sql.connect());
            SqlDataAdapter bd2 = new SqlDataAdapter(cmd2);
            DataSet tb2 = new DataSet();
            bd2.Fill(tb2, "Control");
            for (int m = 0; m < tb2.Tables["Control"].Rows.Count; m++)
            {
                Reg2 = tb2.Tables["Control"].Rows[m];
                if (Convert.ToString(Reg2["Nombre"]) == Convert.ToString(CboEstado.SelectedItem))
                {
                    idEdo = Convert.ToInt32(Reg2["Id"]);
                }
            }
            cmd.CommandText = "UPDATE Ciudades SET " +
                              "Nombre='" + TxtNombre.Text +
                              "',IdEdo=" + idEdo +
                              " WHERE Id=" + TxtId.Text;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Registro Actualizado");
            //TBControl.Clear(BDControl, "Control");
            Registro = TBControl.Tables["Control"].Rows[pos];
            visualizaDatos();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                idCiu = Convert.ToInt32(TxtId.Text);
                string StrControl = "DELETE FROM Ciudades WHERE Id = " + idCiu + ";" ;
                //cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idCiu;
                cmd.Parameters.AddWithValue("@Id", idCiu);
                cmd = new SqlCommand(StrControl, sql.connect());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registro eliminado exitosamente.");
                //BDControl.Update(TBControl, "Control");
                cmd = new SqlCommand(StrControl, sql.connect());
                BDControl = new SqlDataAdapter(cmd);
                TBControl = new DataSet();
                BDControl.Fill(TBControl, "Control");
                if (pos == 0)
                {
                    pos += 1;
                }
                else
                {
                    pos -= 1;
                }
                visualizaDatos();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            if (BtnRegistrar.Text == "&Registrar")
            {
                cmd = new SqlCommand("INSERT INTO Ciudades VALUES (1,'') ; SELECT SCOPE_IDENTITY()", sql.getConn());
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                TxtNombre.Text = "";
                BtnPrimero.Enabled = false;
                BtnUltimo.Enabled = false;
                BtnSiguiente.Enabled = false;
                BtnAnterior.Enabled = false;
                BtnEliminar.Enabled = false;
                BtnActualizar.Enabled = false;
                BtnSalir.Enabled = false;
                CboEstado.Enabled = true;
                TxtId.Text = Convert.ToString(id);
                BtnRegistrar.Text = "Aceptar";
            }
            else
            {
                int idEdo = 1;
                string str2 = "SELECT * FROM Estados";
                SqlCommand cmd2 = new SqlCommand(str2, sql.connect());
                SqlDataAdapter bd2 = new SqlDataAdapter(cmd2);
                DataSet tb2 = new DataSet();
                bd2.Fill(tb2, "Control");
                for (int m = 0; m < tb2.Tables["Control"].Rows.Count; m++)
                {
                    Reg2 = tb2.Tables["Control"].Rows[m];
                    if (Convert.ToString(Reg2["Nombre"]) == Convert.ToString(CboEstado.SelectedItem))
                    {
                        idEdo = Convert.ToInt32(Reg2["Id"]);
                    }
                }
                cmd.CommandText = "UPDATE Ciudades SET " +
                                  "Nombre='" + TxtNombre.Text + 
                                  "',IdEdo=" + idEdo +
                                  " WHERE Id=" + TxtId.Text;
                cmd.ExecuteNonQuery();
                visualizaDatos();
                BtnPrimero.Enabled = true;
                BtnUltimo.Enabled = true;
                BtnSiguiente.Enabled = true;
                BtnAnterior.Enabled = true;
                BtnEliminar.Enabled = true;
                BtnActualizar.Enabled = true;
                BtnSalir.Enabled = true;
                CboEstado.Enabled = false;
                BtnRegistrar.Text = "&Registrar";
            }
        }

        private void Frm_Ciudades_Load(object sender, EventArgs e)
        {
            string StrControl = "SELECT * FROM Estados";
            cmd = new SqlCommand(StrControl, sql.connect());
            BDControl = new SqlDataAdapter(cmd);
            TBControl = new DataSet();
            BDControl.Fill(TBControl, "Control");
            for (int m = 0; m < TBControl.Tables["Control"].Rows.Count; m++)
            {
                Registro = TBControl.Tables["Control"].Rows[m];
                CboEstado.Items.Add(Convert.ToString(Registro["Nombre"]));
            }
            StrControl = "SELECT C.Id, C.Nombre, E.Nombre AS Edo, E.Id AS IdEdo FROM Ciudades C INNER JOIN Estados E ON C.IdEdo = E.Id";
            cmd = new SqlCommand(StrControl, sql.connect());
            BDControl = new SqlDataAdapter(cmd);
            TBControl = new DataSet();
            BDControl.Fill(TBControl, "Control");

            Registro = TBControl.Tables["Control"].Rows[0];
            visualizaDatos();
        }

        public void visualizaDatos()
        {
            BDControl.Dispose();
            TBControl.Dispose();
            BDControl = new SqlDataAdapter(cmd);
            TBControl = new DataSet();
            BDControl.Fill(TBControl, "Control");
            TxtNombre.Text = Convert.ToString(Registro["Nombre"]);
            TxtId.Text = Convert.ToString(Registro["Id"]);
            try
            {
                CboEstado.SelectedItem = Convert.ToString(Registro["Edo"]);
            }
            catch
            {
                CboEstado.SelectedIndex = -1;
            }
        }
    }
}
