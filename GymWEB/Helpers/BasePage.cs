
using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace GymWEB.Helpers
{
    
    public class BasePage : Page
    {
        
        protected virtual string[] RolesPermitidos => new string[0];

        protected override void OnLoad(EventArgs e)
        {
            var token = Session["Token"] as string;

            if (string.IsNullOrWhiteSpace(token))
            {
                RedirigirALogin(null);
                return;
            }

            if (TokenExpirado(token))
            {
                Session.Clear();
                RedirigirALogin("Tu sesión expiró. Inicia sesión nuevamente.");
                return;
            }

            if (RolesPermitidos.Length > 0 &&
                !RolesPermitidos.Contains(RolActual, StringComparer.OrdinalIgnoreCase))
            {
                Session.Clear();
                RedirigirALogin("No tienes permisos para acceder a esa sección.");
                return;
            }

            base.OnLoad(e);
        }

        private void RedirigirALogin(string mensaje)
        {
            string url = "~/Views/Login.aspx";

            if (!string.IsNullOrWhiteSpace(mensaje))
                url += "?msg=" + HttpUtility.UrlEncode(mensaje);

            Response.Redirect(url);
        }

        
        private static bool TokenExpirado(string jwt)
        {
            try
            {
                string[] partes = jwt.Split('.');

                if (partes.Length != 3)
                    return true;

                string payload = partes[1]
                    .Replace('-', '+')
                    .Replace('_', '/');

                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }

                byte[] bytes = Convert.FromBase64String(payload);
                string json = Encoding.UTF8.GetString(bytes);

                var match = System.Text.RegularExpressions.Regex.Match(json, "\"exp\"\\s*:\\s*(\\d+)");

                if (!match.Success)
                    return true;

                long expUnix = long.Parse(match.Groups[1].Value);

                DateTime expiracion = DateTimeOffset
                    .FromUnixTimeSeconds(expUnix)
                    .UtcDateTime;

                return expiracion <= DateTime.UtcNow;
            }
            catch
            {
                return true;
            }
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
