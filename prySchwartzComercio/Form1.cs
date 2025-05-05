using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MaterialSkin;
using MaterialSkin.Controls;



namespace prySchwartzComercio
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.BlueGrey800, Primary.BlueGrey900,
                Primary.BlueGrey500, Accent.LightBlue200,
                TextShade.WHITE);




            dgv.BackgroundColor = Color.White;
            dgv.GridColor = Color.LightGray;
            dgv.BorderStyle = BorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(63, 81, 181);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.EnableHeadersVisualStyles = false;


        }

        //INSTANCIACION DE OBJETOS, DE VARIABLES GLOBALES,ETC
        clsConexion conexionBD = new clsConexion();
        private bool modoEdicion = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            
            numPrecio.Maximum = 1000000;
            numPrecio.Minimum = 10000;
            numStock.Maximum = 5000;
            numStock.Minimum = 10;

            numStock.Enabled = false;

            
            int[] vectorNumeros = { 1, 2, 3 };
            foreach (int numero in vectorNumeros)
            {
                cmbCategoria.Items.Add(numero);
            }

            
            conexionBD.ConectarBD();
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            txtDescripcion.Enabled = !string.IsNullOrWhiteSpace(txtNombre.Text);
            ValidarFormulario();
        }

        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {
            ValidarFormulario();
            cmbCategoria.Enabled = !string.IsNullOrWhiteSpace(txtDescripcion.Text);
        }

        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidarFormulario();
            numPrecio.Enabled = cmbCategoria.SelectedIndex != -1;
        }

        private void numPrecio_ValueChanged(object sender, EventArgs e)
        {
            // Habilita stock solo si precio es válido
            if (numPrecio.Value > 10000 && numPrecio.Value <= 1000000)
                numStock.Enabled = true;
            else
            {
                numStock.Enabled = false;
                numStock.Value = 10; 
            }
            ValidarFormulario();  
        }

        private void numStock_ValueChanged(object sender, EventArgs e)
        {
            ValidarFormulario();
        }


        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            // Rellenar los campos con los datos del producto seleccionado en el DataGridView
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgv.Rows[e.RowIndex];

                numCodigo.Value = Convert.ToDecimal(fila.Cells["Codigo"].Value);
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtDescripcion.Text = fila.Cells["Descripcion"].Value.ToString();
                numPrecio.Value = Convert.ToDecimal(fila.Cells["Precio"].Value);
                numStock.Value = Convert.ToDecimal(fila.Cells["Stock"].Value);
                cmbCategoria.SelectedItem = Convert.ToInt32(fila.Cells["CategoriaId"].Value);

                // Habilitar botones de modificar y eliminar
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;
                btnAgregar.Enabled = false;
            }

            modoEdicion = true;
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;
            btnAgregar.Enabled = false;

        }


        private void materialButton1_Click(object sender, EventArgs e)
        {
            dgv.DataSource = null;
            // Cargar todos los productos en el DataGridView
            DataTable tabla = conexionBD.obtenerProductos();
            dgv.DataSource = tabla;
            dgv.Columns["Codigo"].DisplayIndex = 0;
            dgv.Columns["Nombre"].DisplayIndex = 1;
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            // Cargar los nombres de los productos en el DataGridView
            DataTable tablaNombres = conexionBD.obtenerNombres();
            dgv.DataSource = tablaNombres;
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Estás seguro que quieres agregar este registro?", "Confirmación", MessageBoxButtons.YesNo);

            if (resultado == DialogResult.Yes)
            {
                // Obtener los valores ingresados en el formulario
                string nombre = txtNombre.Text;
                string descripcion = txtDescripcion.Text;
                int categoria = Convert.ToInt32(cmbCategoria.SelectedItem);
                decimal precio = numPrecio.Value;
                int stock = (int)numStock.Value;

                // Agregar el producto a la base de datos
                conexionBD.agregarProducto(nombre, descripcion, precio, stock, categoria);

                // Mostrar mensaje de éxito y actualizar el DataGridView
                dgv.DataSource = conexionBD.obtenerProductos();
            }
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Estás seguro que quieres modificar este registro?", "Confirmación", MessageBoxButtons.YesNo);

            if (resultado == DialogResult.Yes)
            {
                // Obtener los valores modificados en el formulario
                string nombre = txtNombre.Text;
                string descripcion = txtDescripcion.Text;
                int categoria = Convert.ToInt32(cmbCategoria.SelectedItem);
                int codigo = (int)numCodigo.Value;
                decimal precio = numPrecio.Value;
                int stock = (int)numStock.Value;

                // Formatear el precio para evitar errores de sintaxis en la consulta
                string consulta = $"UPDATE Productos SET Nombre = '{nombre}', Descripcion = '{descripcion}', Precio = {precio.ToString(CultureInfo.InvariantCulture)}, Stock = {stock}, CategoriaId = {categoria} WHERE Codigo = {codigo}";

                try
                {
                    // Ejecutar la consulta de modificación
                    conexionBD.ejecutarConsulta(consulta);
                    MessageBox.Show("Producto modificado correctamente.");
                    dgv.DataSource = conexionBD.obtenerProductos();
                }
                catch (Exception ex)
                {
                    // Manejo de errores en caso de fallo
                    MessageBox.Show("Error al ejecutar consulta: " + ex.Message);
                }
            }
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Estás seguro que quieres eliminar este registro?", "Confirmación", MessageBoxButtons.YesNo);

            if (resultado == DialogResult.Yes)
            {
                int codigo = (int)numCodigo.Value;

                // Consulta para eliminar el producto
                string consulta = $"DELETE FROM Productos WHERE Codigo = {codigo}";
                conexionBD.ejecutarConsulta(consulta);

                // Actualizar el DataGridView después de eliminar
                dgv.DataSource = conexionBD.obtenerProductos();
            }
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            numCodigo.Value = 0;
            numPrecio.Value = 10000;
            numStock.Value = 10;
            txtDescripcion.Text = "";
            txtNombre.Text = "";
            cmbCategoria.Text = "";
            numPrecio.Enabled = false;
            numStock.Enabled = false;
            btnEliminar.Enabled = false;
            btnModificar.Enabled = false;
            btnAgregar.Enabled = false;
            modoEdicion = false;
        }


        private void ValidarFormulario()
        {
            bool nombreValido = !string.IsNullOrWhiteSpace(txtNombre.Text);
            bool descripcionValida = !string.IsNullOrWhiteSpace(txtDescripcion.Text);
            bool precioValido = numPrecio.Value > 10000 && numPrecio.Value < 1000001;
            bool stockValido = numStock.Value > 10 && numStock.Value < 5001 ;
            bool categoriaValida = cmbCategoria.SelectedIndex != -1;

            if (!modoEdicion)
            {
                btnAgregar.Enabled = nombreValido && descripcionValida && precioValido && stockValido && categoriaValida;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}