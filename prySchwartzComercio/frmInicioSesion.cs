using MaterialSkin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;

namespace prySchwartzComercio
{
    public partial class frmInicioSesion : MaterialForm
    {
        public frmInicioSesion()
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

        //DECLARACION DE VARIABLE PARA CERRAR PROGRAMA
        private int intentosFallidos = 0;
        private void btnAcceder_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;
            string contraseña = txtContraseña.Text;
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contraseña))
            {
                MessageBox.Show("Complete todos los campos.");
                return;
            }

            clsConexion conexion = new clsConexion();

            if (conexion.VerificarLogin(usuario, contraseña))
            {
                MessageBox.Show("Bienvenido");
                this.Hide();
                using (Form1 f1 = new Form1())
                {
                    f1.ShowDialog(); // bloquea hasta que Form1 se cierre
                }
                this.Close();

            }
            else
            {
                intentosFallidos++;
                MessageBox.Show("Usuario o contraseña incorrectos.");

                if (intentosFallidos >= 3)
                {
                    MessageBox.Show("Demasiados intentos. La app se cerrará.");
                    Environment.Exit(0);
                }
            }
        }

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            frmRegistro formuregistro = new frmRegistro();
            formuregistro.ShowDialog();
        }
    }
}
