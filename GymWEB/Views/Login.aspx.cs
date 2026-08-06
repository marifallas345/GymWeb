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
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                lblMensaje.Text = "1";

                LoginRequest login = new LoginRequest
                {
                    Username = txtUsuario.Text.Trim(),
                    Password = txtPassword.Text.Trim()
                };

                lblMensaje.Text = "2";

                UsuarioService servicio = new UsuarioService();

                lblMensaje.Text = "3";

                LoginResponse respuesta = servicio.Login(login);

                lblMensaje.Text = "4";

                Session["Token"] = respuesta.Token;

                lblMensaje.Text = "5";

                Session["Usuario"] = respuesta.Usuario.Nombre;
                Session["Rol"] = respuesta.Usuario.Rol;

                lblMensaje.Text = "6";

                Response.Redirect("~/Views/Dashboard.aspx");
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.ToString();
            }
        }
    }
}