using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using Examen.Models;

namespace Examen.Controllers
{
    public class AccessController : Controller
    {
        // GET: Access
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario oUsuario)
        {
            // Validar si faltan campos
            if (string.IsNullOrEmpty(oUsuario.IdUsuario) || string.IsNullOrEmpty(oUsuario.clave))
            {
                ViewBag.ErrorMessage = "Usuario y contraseña son requeridos.";
                return View();
            }

            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["cn"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_LoginUsuario", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@nombre_usuario", oUsuario.IdUsuario);

                        // Convertir la contraseña a hash
                        string hashedPassword = ComputeSha256Hash(oUsuario.clave);
                        cmd.Parameters.AddWithValue("@contrasena", hashedPassword);

                        // Parámetro de salida para indicar si la autenticación fue exitosa
                        SqlParameter outputParam = new SqlParameter("@es_autenticado", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        // Parámetro de salida para el mensaje
                        SqlParameter messageParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, 50)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        bool isAuthenticated = Convert.ToBoolean(cmd.Parameters["@es_autenticado"].Value);
                        string message = cmd.Parameters["@mensaje"].Value.ToString();

                        if (isAuthenticated)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = message; // Mostrar el mensaje de error
                            return View();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Se produjo un error al intentar iniciar sesión: " + ex.Message;
                return View();
            }
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public ActionResult Registrar()
        {
            return View();
        }

    }
}