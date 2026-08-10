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
                    lblMensaje.Text = Server.HtmlEncode(msg);
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(usuario) ||
                string.IsNullOrWhiteSpace(password))
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

                LoginResponse respuesta =
                    servicio.Login(login);

                if (respuesta == null)
                {
                    lblMensaje.Text =
                        "La API no devolvió una respuesta.";
                    return;
                }

                Session.Clear();

                Session["Token"] =
                    respuesta.Token;

                Session["Usuario"] =
                    respuesta.Usuario.Nombre;

                Session["Rol"] =
                    respuesta.Usuario.Rol;

                Response.Redirect(
                    "~/Views/Dashboard.aspx",
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();
            }
            catch (ApiException apiEx)
            {
                if (apiEx.EsNoAutorizado)
                {
                    lblMensaje.Text =
                        "Usuario o contraseña incorrectos.";
                }
                else if (apiEx.EsProhibido)
                {
                    lblMensaje.Text =
                        "No tienes permisos para realizar esta acción.";
                }
                else
                {
                    lblMensaje.Text =
                        "Error de la API: " +
                        apiEx.Message;
                }
            }
            catch (Exception ex)
            {
                // TEMPORAL: mostrar el error real
                lblMensaje.Text =
                    "ERROR: " +
                    ex.Message;

                if (ex.InnerException != null)
                {
                    lblMensaje.Text +=
                        " | DETALLE: " +
                        ex.InnerException.Message;
                }
            }
        }
    }
}