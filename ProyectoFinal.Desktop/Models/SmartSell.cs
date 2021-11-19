using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProyectoFinal.Desktop.Models
{
    public class SmartSell
    {
        public static ObservableCollection<Usuario> GetUsuarios(string connectionString)
        {
            const string GetUsersQuery = "Select * from Usuarios";
            var usuarios = new ObservableCollection<Usuario>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = GetUsersQuery;
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var usuario = new Usuario();
                                    usuario.UsuarioID = reader.GetInt32(0);
                                    usuario.Nombres = reader.GetString(1);
                                    usuario.Apellidos = reader.GetString(2);
                                    usuario.Correo = reader.GetString(3);
                                    usuario.Clave = reader.GetString(4);
                                    usuario.Activo = reader.GetBoolean(5);
                                    usuarios.Add(usuario);
                                }
                            }
                        }
                    }
                }
                return usuarios;
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return null;
        }
    }
 
}
