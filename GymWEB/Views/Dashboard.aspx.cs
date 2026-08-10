using GymWEB.Helpers;
using GymWEB.Models;
using GymWEB.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI;

namespace GymWEB.Views
{
    public partial class Dashboard : BasePage
    {
        private readonly ClienteService _clienteService =
            new ClienteService();

        private readonly MembresiaService _membresiaService =
            new MembresiaService();

        private readonly InscripcionService _inscripcionService =
            new InscripcionService();

        private readonly UsuarioService _usuarioService =
            new UsuarioService();

        // =========================================================
        // PAGE LOAD
        // =========================================================

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUsuario.Text = UsuarioActual;

                Page.RegisterAsyncTask(
                    new PageAsyncTask(
                        CargarDashboardAsync));
            }
        }

        // =========================================================
        // CARGAR DASHBOARD
        // =========================================================

        private async Task CargarDashboardAsync()
        {
            try
            {
                // =================================================
                // CLIENTES
                // =================================================

                List<Cliente> clientes =
                    await _clienteService
                        .ObtenerTodosAsync();

                lblClientes.Text =
                    clientes.Count.ToString();


                // =================================================
                // MEMBRESÍAS
                // =================================================

                List<Membresia> membresias =
                    await _membresiaService
                        .ObtenerTodosAsync();

                lblMembresias.Text =
                    membresias.Count.ToString();


                // =================================================
                // INSCRIPCIONES
                // =================================================

                List<Inscripcion> inscripciones =
                    await _inscripcionService
                        .ObtenerTodosAsync();

                lblInscripciones.Text =
                    inscripciones.Count.ToString();


                // =================================================
                // USUARIOS
                // =================================================

                List<Usuario> usuarios =
                    await _usuarioService
                        .ObtenerTodosAsync();

                lblUsuarios.Text =
                    usuarios.Count.ToString();
            }
            catch (Exception ex)
            {
                // Si ocurre algún problema con la API,
                // mostramos el error en el Dashboard.

                lblClientes.Text = "0";
                lblMembresias.Text = "0";
                lblInscripciones.Text = "0";
                lblUsuarios.Text = "0";

                System.Diagnostics.Debug.WriteLine(
                    "Error al cargar Dashboard: "
                    + ex.Message);
            }
        }

        // =========================================================
        // NAVEGACIÓN
        // =========================================================

        protected void btnDashboard_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "~/Views/Dashboard.aspx");
        }

        protected void btnClientes_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "~/Views/Clientes.aspx");
        }

        protected void btnMembresias_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "~/Views/Membresias.aspx");
        }

        protected void btnInscripciones_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "~/Views/Inscripciones.aspx");
        }

        protected void btnUsuarios_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "~/Views/Usuarios.aspx");
        }

        // =========================================================
        // CERRAR SESIÓN
        // =========================================================

        protected void btnCerrarSesion_Click(
            object sender,
            EventArgs e)
        {
            Session.Clear();

            Session.Abandon();

            Response.Redirect(
                "~/Views/Login.aspx",
                false);

            Context.ApplicationInstance
                .CompleteRequest();
        }
    }
}