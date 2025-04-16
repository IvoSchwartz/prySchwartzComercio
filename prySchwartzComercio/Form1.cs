using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prySchwartzComercio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        //INSTANCIACION DE OBJETO DE CLASE CONEXION
        clsConexion conexionBD = new clsConexion();

        private void Form1_Load(object sender, EventArgs e)
        {
            conexionBD.ConectarBD();
        }

        private void btnCarga_Click(object sender, EventArgs e)
        {
            DataTable tabla = conexionBD.obtenerProductos(); 
            dgv.DataSource = tabla;
        }

        private void btnNombres_Click(object sender, EventArgs e)
        {
            DataTable tablaNombres = conexionBD.obtenerNombres();
            dgv.DataSource=tablaNombres;
        }
    }
}
