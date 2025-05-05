using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Globalization;

namespace prySchwartzComercio
{
    public class clsConexion
    {
        string cadenaConexion = "Server=localhost;Database=Comercio;Trusted_Connection=True;";

        SqlConnection coneccionBaseDatos;

        public string nombreBaseDeDatos;

        public void ConectarBD()
        {
            try
            {
                coneccionBaseDatos = new SqlConnection(cadenaConexion);
                nombreBaseDeDatos = coneccionBaseDatos.Database;
                coneccionBaseDatos.Open();
                MessageBox.Show("Conectado a " + nombreBaseDeDatos);
            }
            catch (Exception error)
            {
                MessageBox.Show("Hay un error : " + error.Message);
            }
        }

        public DataTable ejecutarConsulta(string consulta)
        {
            DataTable tabla = new DataTable();

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion))
                    {
                        adaptador.Fill(tabla);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar consulta: " + ex.Message);
            }

            return tabla;
        }

        public bool agregarProducto(string nombre, string descripcion, decimal precio, int stock, int categoriaId)
        {
            bool exito = false;

            string consulta = "INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, CategoriaId) " +
                              "VALUES (@Nombre, @Descripcion, @Precio, @Stock, @CategoriaId)";

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                {
                    conexion.Open();

                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@Nombre", nombre);
                        comando.Parameters.AddWithValue("@Descripcion", descripcion);
                        comando.Parameters.AddWithValue("@Precio", precio);
                        comando.Parameters.AddWithValue("@Stock", stock);
                        comando.Parameters.AddWithValue("@CategoriaId", categoriaId);

                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            exito = true;
                            MessageBox.Show("Producto agregado correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("No se agregó el producto.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el producto: " + ex.Message);
            }

            return exito;
        }

        public DataTable obtenerProductos()
        {
            return ejecutarConsulta("SELECT * FROM Productos");
        }

        public DataTable obtenerNombres()
        {
            return ejecutarConsulta("SELECT Nombre FROM Productos");
        }
        public bool VerificarUsuarioExiste(string usuario)
        {
            bool existe = false;
            string consulta = "SELECT COUNT(*) FROM Datos WHERE LOWER(Usuario) = @Usuario";

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.Add(new SqlParameter("@Usuario", SqlDbType.VarChar) { Value = usuario.ToLower().Trim() });

                        int cantidad = (int)comando.ExecuteScalar();
                        existe = cantidad > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar usuario: " + ex.Message);
            }

            return existe;
        }

        public bool RegistrarUsuario(string usuario, string contraseña)
        {
            if (VerificarUsuarioExiste(usuario))
                return false;

            bool exito = false;
            string consulta = "INSERT INTO Datos (Usuario, Contraseña) VALUES (@Usuario, @Contraseña)";

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.Add(new SqlParameter("@Usuario", SqlDbType.VarChar) { Value = usuario.ToLower().Trim() });
                        comando.Parameters.Add(new SqlParameter("@Contraseña", SqlDbType.VarChar) { Value = contraseña.Trim() });

                        int filas = comando.ExecuteNonQuery();
                        exito = filas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar usuario: " + ex.Message);
            }

            return exito;
        }

        public bool VerificarLogin(string usuario, string contraseña)
        {
            bool correcto = false;
            string consulta = "SELECT COUNT(*) FROM Datos WHERE LOWER(Usuario) = @Usuario AND Contraseña = @Contraseña";

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.Add(new SqlParameter("@Usuario", SqlDbType.VarChar) { Value = usuario.ToLower().Trim() });
                        comando.Parameters.Add(new SqlParameter("@Contraseña", SqlDbType.VarChar) { Value = contraseña.Trim() });

                        int cantidad = (int)comando.ExecuteScalar();
                        correcto = cantidad > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar login: " + ex.Message);
            }

            return correcto;
        }


    }

}
