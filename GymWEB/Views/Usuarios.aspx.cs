using GymWEB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GymWEB.Views
{
    public partial class Usuarios : BasePage
    {
        // La gestión de usuarios solo debe estar disponible para administradores.
        protected override string[] RolesPermitidos => new[] { "Admin" };

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}