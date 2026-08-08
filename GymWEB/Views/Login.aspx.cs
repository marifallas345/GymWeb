
using GymWEB.Exceptions;
using GymWEB.Models;
using GymWEB.Services;
using System;
using System.Web.UI;

namespace GymWEB.Views
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();

                string msg = Request.QueryString["msg"];

                if (!string.IsNullOrWhiteSpace(msg))
                {
                    // El valor llega URL-decodificado desde Request.QueryString;
                    // Se codifica como HTML al asignarlo para evitar XSS reflejado.
                    lblMensaje.Text = Server.HtmlEncode(msg);
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                lblMensaje.Text = "Ingresa usuario y contraseña.";
                return;
            }

            try
            {
                LoginRequest login = new LoginRequest
                {
                    Username = usuario,
                    Password = password
                };

                UsuarioService servicio = new UsuarioService();
                LoginResponse respuesta = servicio.Login(login);

                Session.Clear();
                Session["Token"] = respuesta.Token;
                Session["Usuario"] = respuesta.Usuario.Nombre;
                Session["Rol"] = respuesta.Usuario.Rol;

                Response.Redirect("~/Views/Dashboard.aspx");
            }
            catch (ApiException apiEx) when (apiEx.EsNoAutorizado)
            {
                // Credenciales inválidas: Mensaje genérico, sin indicar si el
                // usuario existe o no.
                lblMensaje.Text = "Usuario o contraseña incorrectos.";
            }
            catch (ApiException apiEx)
            {
               
                lblMensaje.Text = apiEx.Message;
            }
            catch (Exception)
            {
           
                lblMensaje.Text = "Ocurrió un error inesperado. Intenta nuevamente.";
            }
        }
    }
}
