﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace ProyectoFinal.Desktop.Models
{
    //Delete user: DELETE FROM Usuarios WHERE UsuarioID = *
    public sealed class SmartSell
    {
        private string connectionString = (App.Current as App).ConnectionString;
        public Usuario CurrentUser { get; set; }


        private static readonly SmartSell instance = new SmartSell();
        static SmartSell() {}
        private SmartSell() {}

        public static SmartSell Instance
        {
            get
            {
                return instance;
            }
        }

        public string ToSHA256(string data)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public ObservableCollection<Usuario> GetUsuarios()
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
                                    var usuario = new Usuario
                                    {
                                        UsuarioID = reader.GetInt32(0),
                                        Nombres = reader.GetString(1),
                                        Apellidos = reader.GetString(2),
                                        Correo = reader.GetString(3),
                                        Clave = reader.GetString(4),
                                        Activo = reader.GetBoolean(5)
                                    };
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

        public ObservableCollection<Subasta> GetSubastas()
        {
            const string GetUsersQuery = "Select * from Subastas";
            var subastas = new ObservableCollection<Subasta>();
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
                                    var subasta = new Subasta
                                    {
                                        SubastaID = reader.GetInt32(0),
                                        UsuarioID = reader.GetInt32(1),
                                        NombreProducto = reader.GetString(2),
                                        DescripcionProducto = reader.GetString(3),
                                        FotoUrlProducto = reader.GetString(4),
                                        PrecioInicial = reader.GetFloat(5),
                                        FechaLimite = reader.GetDateTime(6),
                                        Usuario = GetUsuarios().Where(u => u.UsuarioID == reader.GetInt32(1)).FirstOrDefault()
                                    };
                                    subastas.Add(subasta);
                                }
                            }
                        }
                    }
                }
                return subastas;
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return null;
        }

        public ObservableCollection<Oferta> GetOfertas()
        {
            const string GetUsersQuery = "Select * from Ofertas";
            var ofertas = new ObservableCollection<Oferta>();
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
                                    var oferta = new Oferta
                                    {
                                        OfertaID = reader.GetInt32(0),
                                        UsuarioID = reader.GetInt32(1),
                                        SubastaID = reader.GetInt32(2),
                                        Monto = reader.GetFloat(3),
                                        FechaCreacion = reader.GetDateTime(4),
                                        Usuario = GetUsuarios().Where(u => u.UsuarioID == reader.GetInt32(1)).FirstOrDefault(),
                                        Subasta = GetSubastas().Where(u => u.SubastaID == reader.GetInt32(2)).FirstOrDefault()
                                    };
                                    ofertas.Add(oferta);
                                }
                            }
                        }
                    }
                }
                return ofertas;
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return null;
        }

        public ObservableCollection<Comentario> GetComentarios()
        {
            const string GetUsersQuery = "Select * from Comentarios";
            var comentarios = new ObservableCollection<Comentario>();
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
                                    var comentario = new Comentario
                                    {
                                        ComentarioID = reader.GetInt32(0),
                                        UsuarioID = reader.GetInt32(1),
                                        SubastaID = reader.GetInt32(2),
                                        Descripcion = reader.GetString(3),
                                        FechaCreacion = reader.GetDateTime(4),
                                        Usuario = GetUsuarios().Where(u => u.UsuarioID == reader.GetInt32(1)).FirstOrDefault(),
                                        Subasta = GetSubastas().Where(u => u.SubastaID == reader.GetInt32(2)).FirstOrDefault()

                                    };
                                    comentarios.Add(comentario);
                                }
                            }
                        }
                    }
                }
                return comentarios;
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return null;
        }

        public ObservableCollection<RatingUsuario> GetRatingUsuario()
        {
            const string GetUsersQuery = "Select * from RatingUsuarios";
            var ratings = new ObservableCollection<RatingUsuario>();
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
                                    var rating = new RatingUsuario
                                    {
                                        RatingUsuarioID = reader.GetInt32(0),
                                        UsuarioCalificadoID = reader.GetInt32(1),
                                        UsuarioCalificadorID = reader.GetInt32(2),
                                        Rating = reader.GetInt32(3),
                                        UsuarioCalificado = GetUsuarios().Where(u => u.UsuarioID == reader.GetInt32(1)).FirstOrDefault(),
                                        UsuarioCalificador = GetUsuarios().Where(u => u.UsuarioID == reader.GetInt32(2)).FirstOrDefault()
                                    };
                                    ratings.Add(rating);
                                }
                            }
                        }
                    }
                }
                return ratings;
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return null;
        }

        public ICollection<Oferta> FindOfertasBySubastaID(int ID)
        {
            string GetUsersQuery = $"Select * from Ofertas WHERE SubastaID = {ID}";
            var ofertas = new ObservableCollection<Oferta>();
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
                                    var oferta = new Oferta
                                    {
                                        OfertaID = reader.GetInt32(0),
                                        UsuarioID = reader.GetInt32(1),
                                        SubastaID = reader.GetInt32(2),
                                        Monto = reader.GetFloat(3),
                                        FechaCreacion = reader.GetDateTime(4),
                                        Usuario = GetUsuarios().Where(u => u.UsuarioID == reader.GetInt32(1)).FirstOrDefault(),
                                        Subasta = GetSubastas().Where(u => u.SubastaID == reader.GetInt32(2)).FirstOrDefault()
                                    };
                                    ofertas.Add(oferta);
                                }
                            }
                        }
                    }
                }
                return ofertas;
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return null;
        }

        public bool AddUserDB(string nombres, string apellidos, string correo, string clave)
        {
            
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                string addUser = $"INSERT INTO Usuarios VALUES('{nombres}','{apellidos}','{correo}','{clave}',1)";
                conn.Open();
                SqlCommand cmd = new SqlCommand(addUser, conn);
                int cantidad = cmd.ExecuteNonQuery();
                if (cantidad == 1)
                {
                    return true;
                }
                return false;
                


            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
                return false;
            }
        }
    }
}