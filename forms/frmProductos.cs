using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistema_Inventarios
{
    public partial class frmProductos : Form
    {

        SQL sql = new SQL();
        public SqlDataAdapter bdProductos;
        public DataSet tbProductos;
        public DataRow regProductos;

        int pos = 0;

        public frmProductos()
        {
            InitializeComponent();
        }
        public void showData()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Productos", sql.connect());
            bdProductos = new SqlDataAdapter(cmd);
            tbProductos = new DataSet();
            bdProductos.Fill(tbProductos, "Productos");
            if (pos > BindingContext[tbProductos, "Productos"].Count - 1)
            {
                pos -= 1;
            }
            else if (pos <= 0)
            {
                pos = 0;
            }
            BindingContext[tbProductos, "Productos"].Position = pos;
            regProductos = tbProductos.Tables["Productos"].Rows[pos];
            txtId.Text = Convert.ToString(regProductos["Id"]);
            txtNombre.Text = Convert.ToString(regProductos["NombreCorto"]);
            txtDescripcion.Text = Convert.ToString(regProductos["Descripcion"]);
            txtCosto.Text = Convert.ToString(regProductos["Costo"]);
            txtPrecio1.Text = Convert.ToString(regProductos["Precio_1"]);
            txtPrecio2.Text = Convert.ToString(regProductos["Precio_2"]);
            txtPrecio3.Text = Convert.ToString(regProductos["Precio_3"]);
            byte[] byteLogotipo = ((byte[])regProductos["Foto"]);
            imgProducto.Image = ByteArrayToImage(byteLogotipo); 
        }
   
        //Image a Byte
        public byte[] ImageToByteArray(System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        //Byte a Image 
        public System.Drawing.Image ByteArrayToImage(byte[] byteImg)
        {
            MemoryStream ms = new MemoryStream(byteImg);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnRegistrar.Text == "Registrar")
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Productos(NombreCorto,Descripcion,Costo,Precio_1,Precio_2,Precio_3) VALUES ('','',0,0,0,0); SELECT SCOPE_IDENTITY()", sql.getConn());
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    btnPrimero.Enabled = false;
                    btnUltimo.Enabled = false;
                    btnSiguiente.Enabled = false;
                    btnAnterior.Enabled = false;
                    btnEliminar.Enabled = false;
                    btnActualizar.Enabled = false;
                    btnSalir.Enabled = false;
                    txtNombre.Text = "";
                    txtDescripcion.Text = "";
                    txtCosto.Text = "";
                    txtUnidadMedida.Text = "";
                    txtPrecio1.Text = "";
                    txtPrecio2.Text = "";
                    txtPrecio3.Text = "";
                    imgProducto.Image.Dispose();
                    imgProducto.Image = null;
                    txtId.Text = Convert.ToString(id);
                    btnRegistrar.Text = "Aceptar";
                }
                else
                {
                    if (txtNombre.Text == "" || txtDescripcion.Text == "" || txtCosto.Text == "" || txtPrecio1.Text == "" || txtPrecio2.Text == "" || txtPrecio3.Text == "" || imgProducto.Image == null)
                    {
                        MessageBox.Show("LLena todos los campos");
                    }
                    else
                    {
                        byte[] byteLogotipo = ImageToByteArray(imgProducto.Image); //Convertir Imagen a Byte

                        //Consulta SQL 
                        string query = "UPDATE Productos SET " +
                        "NombreCorto = @Nombre," +
                        "Descripcion = @Descripcion," +
                        "Costo = @Costo," +
                        "Precio_1 = @Precio_1," +
                        "Precio_2 = @Precio_2," +
                        "Precio_3 = @Precio_3," +
                        "Foto = @Foto " +
                        "WHERE id = @Id ";
                        SqlCommand cmd = new SqlCommand(query, sql.connect());
                        //Parametros
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text);
                        cmd.Parameters.AddWithValue("@Costo", float.Parse(txtCosto.Text));
                        cmd.Parameters.AddWithValue("@Precio_1", float.Parse(txtPrecio1.Text));
                        cmd.Parameters.AddWithValue("@Precio_2", float.Parse(txtPrecio2.Text));
                        cmd.Parameters.AddWithValue("@Precio_3", float.Parse(txtPrecio3.Text));
                        cmd.Parameters.AddWithValue("@Foto", byteLogotipo);
                        cmd.Parameters.AddWithValue("@Id", int.Parse(txtId.Text));
                        //Ejecutar Consulta
                        cmd.ExecuteNonQuery();
                                                btnPrimero.Enabled = true;
                        btnUltimo.Enabled = true;
                        btnSiguiente.Enabled = true;
                        btnAnterior.Enabled = true;
                        btnEliminar.Enabled = true;
                        btnActualizar.Enabled = true;
                        btnSalir.Enabled = true;
                        btnRegistrar.Text = "Registrar";
                        MessageBox.Show("Datos Guardados Correctamente");
                        showData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtId.Text == "")
                {
                    MessageBox.Show("LLena todos los campos");
                }
                else
                {
                    if (MessageBox.Show("Quieres eliminar a este producto?", "Advertencia", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        string query = "SELECT * FROM Productos WHERE Id = " + int.Parse(txtId.Text);
                        SqlCommand cmd = new SqlCommand(query, sql.connect());
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            //Consulta SQL  
                            query = "DELETE FROM Productos WHERE Id = @Id";
                            cmd = new SqlCommand(query, sql.connect());
                            //Parametros
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@Id", int.Parse(txtId.Text));
                            //Ejecutar Consulta
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Datos Eliminados Correctamente");
                            showData();
                        }
                        else
                        {
                            MessageBox.Show("ID Invalido");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string imagen = openFileDialog1.FileName;
                    imgProducto.Image = Image.FromFile(imagen);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("El archivo seleccionado no es un tipo de imagen válido" + ex.ToString());
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtId.Text == "" || txtNombre.Text == "" || txtDescripcion.Text == "" || txtCosto.Text == "" || txtPrecio1.Text == "" || txtPrecio2.Text == "" || txtPrecio3.Text == "" || imgProducto.Image == null)
                {
                    MessageBox.Show("LLena todos los campos");
                }
                else
                {
                    string query = "SELECT * FROM Productos WHERE Id = " + int.Parse(txtId.Text);
                    SqlCommand cmd = new SqlCommand(query, sql.connect());
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        byte[] byteLogotipo = ImageToByteArray(imgProducto.Image); //Convertir Imagen a Byte
                        //Consulta SQL
                        query = "UPDATE Productos SET " +
                        "NombreCorto = @Nombre," +
                        "Descripcion = @Descripcion," +
                        "Costo = @Costo," +
                        "Precio_1 = @Precio_1," +
                        "Precio_2 = @Precio_2," +
                        "Precio_3 = @Precio_3," +
                        "Foto = @Foto " +
                        "WHERE id = @Id ";
                        cmd = new SqlCommand(query, sql.connect());
                        //Parametros
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text);
                        cmd.Parameters.AddWithValue("@Costo", float.Parse(txtCosto.Text));
                        cmd.Parameters.AddWithValue("@Precio_1", float.Parse(txtPrecio1.Text));
                        cmd.Parameters.AddWithValue("@Precio_2", float.Parse(txtPrecio2.Text));
                        cmd.Parameters.AddWithValue("@Precio_3", float.Parse(txtPrecio3.Text));
                        cmd.Parameters.AddWithValue("@Foto", byteLogotipo);
                        cmd.Parameters.AddWithValue("@Id", int.Parse(txtId.Text));
                        //Ejecutar Consulta
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Datos Actualizados Correctamente");
                        showData();
                    }
                    else
                    {
                        MessageBox.Show("ID Invalido");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnPrimero_Click_1(object sender, EventArgs e)
        {

        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            pos = BindingContext[tbProductos, "Productos"].Count - 1;
            showData();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            pos -= 1;
            showData();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            pos += 1;
            showData();
        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            pos = 0;
            showData();
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            //sql.connect();
            showData();
        }
    }
}
