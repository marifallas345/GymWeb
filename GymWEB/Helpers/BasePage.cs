using System;
using System.Web;
using System.Web.UI;

namespace GymWEB.Helpers
{
    public class BasePage : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (Session["Token"] == null)
            {
                Response.Redirect("~/Views/Login.aspx");
                return;
            }

            base.OnLoad(e);
        }

        protected string UsuarioActual
        {
            get
            {
                return Session["Usuario"]?.ToString() ?? "";
            }
        }

        protected string RolActual
        {
            get
            {
                return Session["Rol"]?.ToString() ?? "";
            }
        }
    }
}