using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace prySchwartzComercio
{
    public partial class frmRegistro : MaterialForm
    {
        public frmRegistro()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.BlueGrey800, Primary.BlueGrey900,
                Primary.BlueGrey500, Accent.LightBlue200,
                TextShade.WHITE);
        }

        //Insatanciacion de clsConexion
        clsConexion conexion = new clsConexion();

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;
            string contraseña = txtContraseña.Text;

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contraseña))
            {
                MessageBox.Show("Complete todos los campos.");
                return;
            }
           
            if (conexion.RegistrarUsuario(usuario, contraseña))
            {
                MessageBox.Show("Usuario creado exitosamente.");
                this.Close(); // o this.Hide();
            }
            else
            {
                MessageBox.Show("Ese usuario ya existe. Intente con otro nombre.");
            }
        }
    }
}
