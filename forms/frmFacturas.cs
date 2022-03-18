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
using System.IO;
//aaaaaaaaaa
namespace Sistema_Inventarios
{
    public partial class frmFacturas : Form
    {
        SQL sql = new SQL();
        public SqlDataAdapter bdFacturas, bdProdFact, bdClientes, bdProd, bdControl;
        public DataSet tbFacturas, tbProdFact, tbClientes, tbProd, tbControl;

        private void cboProdFact_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvProductos.RowCount != 0)
            {
                prodSearch(cboProdFact.Text);
                int index = dgvProductos.CurrentRow.Index;
                dgvProductos.Rows[index].Cells[0].Value = Convert.ToString(regProd["Id"]);
                dgvProductos.Rows[index].Cells[1].Value = Convert.ToString(regProd["Descripcion"]);
                dgvProductos.Rows[index].Cells[3].Value = Convert.ToString(regProd["Descuento"]);
                if (Convert.ToString(regClientes["NivelPrecio"]) == "1")
                {
                    dgvProductos.Rows[index].Cells[4].Value = Convert.ToString(regProd["Precio_1"]);
                }
                else if (Convert.ToString(regClientes["NivelPrecio"]) == "2")
                {
                    dgvProductos.Rows[index].Cells[4].Value = Convert.ToString(regProd["Precio_2"]);
                }
                else
                {
                    dgvProductos.Rows[index].Cells[4].Value = Convert.ToString(regProd["Precio_3"]);
                }
                dgvProductos.Rows[index].Cells[5].Value = 0;
                if (regProd["Foto"] != DBNull.Value)
                {
                    byte[] byteImg = ((byte[])regProd["Foto"]);
                    pctFotoProducto.Image = ByteArrayToImage(byteImg);
                }
            }
            else 
            {
                MessageBox.Show("Agrega un producto primero!");
            }
        }

        public DataRow regFacturas, regProdFact, regClientes, regProd, regControl;

        private void dgvProductos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvProductos.CurrentRow.Index;
            float descuento = Convert.ToSingle(dgvProductos.Rows[index].Cells[3].Value) / 100;
            float importe = Convert.ToInt16(dgvProductos.Rows[index].Cells[2].Value) * Convert.ToInt16(dgvProductos.Rows[index].Cells[4].Value);
            importe -= importe * descuento;
            dgvProductos.Rows[index].Cells[5].Value = importe;
            float subtotal = 0;
            for (int m = 0; m < dgvProductos.RowCount; m++)
            {
                subtotal += Convert.ToSingle(dgvProductos.Rows[m].Cells[5].Value);
            }
            txtSubtotal.Text = subtotal.ToString();
            float descuentoTotal = subtotal * (Convert.ToSingle(regClientes["Descuento"]) / 100);
            float iva = (subtotal - descuentoTotal) * (Convert.ToSingle(regControl["IVA"]) / 100);
            float total = subtotal - descuentoTotal + iva;
            txtTotal.Text = total.ToString();
        }

        private void rdbCredito_CheckedChanged(object sender, EventArgs e)
        {
            txtFolio.Text = Convert.ToString(regControl["FolioVtasCredito"]);
        }

        private void rdbContado_CheckedChanged(object sender, EventArgs e)
        {
            txtFolio.Text = Convert.ToString(regControl["FolioVtasContado"]);
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            cboCliente.Enabled = true;
            cboCliente.Focus();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnEliminarProd_Click(object sender, EventArgs e)
        {
            if (dgvProductos.RowCount != 0)
            {
                dgvProductos.Rows.RemoveAt(dgvProductos.CurrentRow.Index);
            }
        }

        private void btnAgregarProd_Click(object sender, EventArgs e)
        {
            dgvProductos.Rows.Add();
            dgvProductos.Rows[0].Cells[0].Selected = false;
            dgvProductos.Rows[dgvProductos.CurrentRow.Index].Cells[2].Selected = false;
            dgvProductos.Rows[dgvProductos.Rows.Count - 1].Cells[2].Selected = true;
        }

        
        private void frmFacturas_Load(object sender, EventArgs e)
        {
            sql.connect();
            showData();
            opcionesProd.Enabled = false;
            totalesProd.Enabled = false;
            btnEliminar.Enabled = false;
            btnImprimir.Enabled = false;
            cboProdFact.Enabled = false;
            cboCliente.Enabled = false;
        }

        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i <= BindingContext[tbClientes, "Clientes"].Count - 1; i++)
            {
                regClientes = tbClientes.Tables["Clientes"].Rows[i];
                if (cboCliente.Text == Convert.ToString(regClientes["RazonSocial"]))
                {
                    txtDatosPersonales.Text = Convert.ToString(regClientes["Domicilio"]) + "\r\n" +
                                       Convert.ToString(regClientes["NombreCiudad"]) + ", " + Convert.ToString(regClientes["NombreEstado"]) + "\r\n" +
                                       "Codigo Postal: " + Convert.ToString(regClientes["CodigoPostal"]) + "\r\n" +
                                       Convert.ToString(regClientes["Telefono"]) + "    " + Convert.ToString(regClientes["RFC"]) + "\r\n" +
                                       Convert.ToString(regClientes["Correo"]) + "\r\n";
                    break;
                }
            }
            if (Convert.ToString(regClientes["Tipo"]) == "1")
            {
                rdbContado.Checked = true;
                txtFolio.Text = Convert.ToString(regControl["FolioVtasContado"]);
            }
            else
            {
                rdbCredito.Checked = true;
                txtFolio.Text = Convert.ToString(regControl["FolioVtasCredito"]);
            }
            opcionesProd.Enabled = true;
            cboProdFact.Enabled = true;
            txtDescuento.Text = Convert.ToString(regClientes["Descuento"]) + "%";
        }



        public frmFacturas()
        {
            InitializeComponent();
        }

        void showData()
        {
            SqlCommand cmd = new SqlCommand("SELECT Estados.Id AS IdEdo, Estados.Nombre AS NombreEstado, Clientes.*, Ciudades.Nombre AS NombreCiudad " +
                    "FROM Estados INNER JOIN " +
                    "Clientes ON Estados.Id = Clientes.Estado INNER JOIN " +
                    "Ciudades ON Estados.Id = Ciudades.IdEdo AND Clientes.Ciudad = Ciudades.Id", sql.getConn());
            bdClientes = new SqlDataAdapter(cmd);
            tbClientes = new DataSet();
            bdClientes.Fill(tbClientes, "Clientes");
            for (int m = 0; m < BindingContext[tbClientes, "Clientes"].Count; m++)
            {
                BindingContext[tbClientes, "Clientes"].Position = m;
                regClientes = tbClientes.Tables["Clientes"].Rows[m];
                cboCliente.Items.Add(Convert.ToString(regClientes["RazonSocial"]));
            }

            cmd = new SqlCommand("SELECT * FROM Productos", sql.getConn());
            bdProd = new SqlDataAdapter(cmd);
            tbProd = new DataSet();
            bdProd.Fill(tbProd, "Productos");
            for (int m = 0; m < BindingContext[tbProd, "Productos"].Count; m++)
            {
                BindingContext[tbProd, "Productos"].Position = m;
                regProd = tbProd.Tables["Productos"].Rows[m];
                cboProdFact.Items.Add(Convert.ToString(regProd["NombreCorto"]));
            }

            cmd = new SqlCommand("SELECT * FROM Control", sql.getConn());
            bdControl = new SqlDataAdapter(cmd);
            tbControl = new DataSet();
            bdControl.Fill(tbControl, "Control");
            BindingContext[tbControl, "Control"].Position = 0;
            regControl = tbControl.Tables["Control"].Rows[0];
            txtIva.Text = Convert.ToString(regControl["IVA"]) + "%";
        }

        void prodSearch(string prodName)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Productos", sql.getConn());
            bdProd = new SqlDataAdapter(cmd);
            tbProd = new DataSet();
            bdProd.Fill(tbProd, "Productos");
            for (int m = 0; m < BindingContext[tbProd, "Productos"].Count; m++)
            {
                BindingContext[tbProd, "Productos"].Position = m;
                regProd = tbProd.Tables["Productos"].Rows[m];
                if (prodName == Convert.ToString(regProd["NombreCorto"]))
                {
                    break;
                }
            }
        }

        public System.Drawing.Image ByteArrayToImage(byte[] byteImg)
        {
            MemoryStream ms = new MemoryStream(byteImg);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }
    }
}
