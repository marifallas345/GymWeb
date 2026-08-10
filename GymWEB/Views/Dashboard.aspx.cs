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
        protected override string[] RolesPermitidos
        {
            get
            {
                return new[]
                {
                    "Administrador",
                    "Empleado"
                };
            }
        }

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

                // -------------------------------------------------
                // OPCIONES SEGÚN ROL
                // -------------------------------------------------

                btnMembresias.Visible = EsAdministrador;
                btnUsuarios.Visible = EsAdministrador;

                // También ocultamos las tarjetas
                // de información restringida.

                pnlMembresias.Visible = EsAdministrador;
                pnlUsuarios.Visible = EsAdministrador;

                // -------------------------------------------------
                // CARGAR DASHBOARD
                // -------------------------------------------------

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
            // =====================================================
            // CLIENTES
            // =====================================================

            try
            {
                List<Cliente> clientes =
                    await _clienteService
                        .ObtenerTodosAsync();

                lblClientes.Text =
                    clientes.Count.ToString();
            }
            catch (Exception ex)
            {
                lblClientes.Text = "0";

                System.Diagnostics.Debug.WriteLine(
                    "Error al cargar Clientes: "
                    + ex.Message);
            }


            // =====================================================
            // INSCRIPCIONES
            // =====================================================

            try
            {
                List<Inscripcion> inscripciones =
                    await _inscripcionService
                        .ObtenerTodosAsync();

                lblInscripciones.Text =
                    inscripciones.Count.ToString();
            }
            catch (Exception ex)
            {
                lblInscripciones.Text = "0";

                System.Diagnostics.Debug.WriteLine(
                    "Error al cargar Inscripciones: "
                    + ex.Message);
            }


            // =====================================================
            // MEMBRESÍAS
            // SOLO ADMINISTRADOR
            // =====================================================

            if (EsAdministrador)
            {
                try
                {
                    List<Membresia> membresias =
                        await _membresiaService
                            .ObtenerTodosAsync();

                    lblMembresias.Text =
                        membresias.Count.ToString();
                }
                catch (Exception ex)
                {
                    lblMembresias.Text = "0";

                    System.Diagnostics.Debug.WriteLine(
                        "Error al cargar Membresías: "
                        + ex.Message);
                }
            }


            // =====================================================
            // USUARIOS
            // SOLO ADMINISTRADOR
            // =====================================================

            if (EsAdministrador)
            {
                try
                {
                    List<Usuario> usuarios =
                        await _usuarioService
                            .ObtenerTodosAsync();

                    lblUsuarios.Text =
                        usuarios.Count.ToString();
                }
                catch (Exception ex)
                {
                    lblUsuarios.Text = "0";

                    System.Diagnostics.Debug.WriteLine(
                        "Error al cargar Usuarios: "
                        + ex.Message);
                }
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
            if (!EsAdministrador)
            {
                Response.Redirect(
                    "~/Views/Dashboard.aspx");

                return;
            }

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
            if (!EsAdministrador)
            {
                Response.Redirect(
                    "~/Views/Dashboard.aspx");

                return;
            }

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