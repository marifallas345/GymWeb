using GymWEB.Helpers;
using GymWEB.Models;
using GymWEB.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GymWEB.Views
{
    public partial class Inscripciones : BasePage
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
        private readonly InscripcionService _inscripcionService =
            new InscripcionService();

        private readonly ClienteService _clienteService =
            new ClienteService();

        private readonly MembresiaService _membresiaService =
            new MembresiaService();

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
                        CargarDatosInicialesAsync));
            }
        }

        // =========================================================
        // CARGAR DATOS INICIALES
        // =========================================================

        private async Task CargarDatosInicialesAsync()
        {
            try
            {
                await CargarClientesAsync();

                await CargarMembresiasAsync();

                await CargarInscripcionesAsync();
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "No fue posible cargar la información: "
                    + ex.Message;
            }
        }

        // =========================================================
        // CLIENTES
        // =========================================================

        private async Task CargarClientesAsync()
        {
            List<Cliente> clientes =
                await _clienteService
                    .ObtenerTodosAsync();

            ddlCliente.Items.Clear();

            ddlCliente.Items.Add(
                new ListItem(
                    "-- Seleccione un cliente --",
                    "0"));

            foreach (Cliente cliente in clientes)
            {
                ddlCliente.Items.Add(
                    new ListItem(
                        cliente.Nombre,
                        cliente.Id.ToString()));
            }
        }

        // =========================================================
        // MEMBRESÍAS
        // =========================================================

        private async Task CargarMembresiasAsync()
        {
            List<Membresia> membresias =
                await _membresiaService
                    .ObtenerTodosAsync();

            ddlMembresia.Items.Clear();

            ddlMembresia.Items.Add(
                new ListItem(
                    "-- Seleccione una membresía --",
                    "0"));

            foreach (Membresia membresia in membresias)
            {
                ddlMembresia.Items.Add(
                    new ListItem(
                        membresia.Nombre,
                        membresia.Id.ToString()));
            }
        }

        // =========================================================
        // LISTAR INSCRIPCIONES
        // =========================================================

        private async Task CargarInscripcionesAsync()
        {
            List<Inscripcion> inscripciones =
                await _inscripcionService
                    .ObtenerTodosAsync();

            gvInscripciones.DataSource =
                inscripciones;

            gvInscripciones.DataBind();
        }

        // =========================================================
        // GUARDAR
        // =========================================================

        protected void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            Page.RegisterAsyncTask(
                new PageAsyncTask(
                    GuardarInscripcionAsync));
        }

        private async Task GuardarInscripcionAsync()
        {
            try
            {
                if (!ValidarFormulario())
                {
                    return;
                }

                DateTime fechaInicio;

                DateTime fechaVencimiento;

                if (!DateTime.TryParse(
                    txtFechaInicio.Text,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out fechaInicio))
                {
                    lblMensaje.Text =
                        "La fecha de inicio no es válida.";

                    return;
                }

                if (!DateTime.TryParse(
                    txtFechaVencimiento.Text,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out fechaVencimiento))
                {
                    lblMensaje.Text =
                        "La fecha de vencimiento no es válida.";

                    return;
                }

                if (fechaVencimiento <= fechaInicio)
                {
                    lblMensaje.Text =
                        "La fecha de vencimiento debe ser posterior a la fecha de inicio.";

                    return;
                }

                Inscripcion inscripcion =
                    new Inscripcion
                    {
                        ClienteId =
                            Convert.ToInt32(
                                ddlCliente.SelectedValue),

                        MembresiaId =
                            Convert.ToInt32(
                                ddlMembresia.SelectedValue),

                        FechaInicio =
                            fechaInicio,

                        FechaVencimiento =
                            fechaVencimiento,

                        Estado =
                            true,

                        Creado_Por =
                            UsuarioActual
                    };

                bool resultado =
                    await _inscripcionService
                        .AgregarAsync(inscripcion);

                if (resultado)
                {
                    lblMensaje.Text =
                        "Inscripción registrada correctamente.";

                    LimpiarFormulario();

                    await CargarInscripcionesAsync();
                }
                else
                {
                    lblMensaje.Text =
                        "No fue posible registrar la inscripción.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "Error al registrar la inscripción: "
                    + ex.Message;
            }
        }

        // =========================================================
        // EDITAR / ELIMINAR
        // =========================================================

        protected void gvInscripciones_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            int indiceFila;

            if (!int.TryParse(
                e.CommandArgument?.ToString(),
                out indiceFila))
            {
                lblMensaje.Text =
                    "No fue posible identificar la inscripción.";

                return;
            }

            if (indiceFila < 0 ||
                indiceFila >= gvInscripciones.Rows.Count)
            {
                lblMensaje.Text =
                    "La fila seleccionada no es válida.";

                return;
            }

            int id =
                Convert.ToInt32(
                    gvInscripciones
                        .DataKeys[indiceFila]
                        .Value);

            if (e.CommandName == "EditarInscripcion")
            {
                Page.RegisterAsyncTask(
                    new PageAsyncTask(
                        () => CargarInscripcionParaEditarAsync(id)));
            }
            else if (e.CommandName == "EliminarInscripcion")
            {
                Page.RegisterAsyncTask(
                    new PageAsyncTask(
                        () => EliminarInscripcionAsync(id)));
            }
        }

        // =========================================================
        // CARGAR INSCRIPCIÓN PARA EDITAR
        // =========================================================

        private async Task CargarInscripcionParaEditarAsync(
            int id)
        {
            try
            {
                Inscripcion inscripcion =
                    await _inscripcionService
                        .ObtenerPorIdAsync(id);

                if (inscripcion == null)
                {
                    lblMensaje.Text =
                        "No se encontró la inscripción.";

                    return;
                }

                ddlCliente.SelectedValue =
                    inscripcion.ClienteId.ToString();

                ddlMembresia.SelectedValue =
                    inscripcion.MembresiaId.ToString();

                txtFechaInicio.Text =
                    inscripcion.FechaInicio
                        .ToString(
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture);

                txtFechaVencimiento.Text =
                    inscripcion.FechaVencimiento
                        .ToString(
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture);

                ViewState["InscripcionId"] =
                    inscripcion.Id;

                btnGuardar.Visible =
                    false;

                btnActualizar.Visible =
                    true;

                lblMensaje.Text =
                    "Inscripción cargada para edición.";
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "No fue posible cargar la inscripción: "
                    + ex.Message;
            }
        }

        // =========================================================
        // ACTUALIZAR
        // =========================================================

        protected void btnActualizar_Click(
            object sender,
            EventArgs e)
        {
            Page.RegisterAsyncTask(
                new PageAsyncTask(
                    ActualizarInscripcionAsync));
        }

        private async Task ActualizarInscripcionAsync()
        {
            try
            {
                if (!ValidarFormulario())
                {
                    return;
                }

                if (ViewState["InscripcionId"] == null)
                {
                    lblMensaje.Text =
                        "No se seleccionó ninguna inscripción.";

                    return;
                }

                DateTime fechaInicio;

                DateTime fechaVencimiento;

                if (!DateTime.TryParse(
                    txtFechaInicio.Text,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out fechaInicio))
                {
                    lblMensaje.Text =
                        "La fecha de inicio no es válida.";

                    return;
                }

                if (!DateTime.TryParse(
                    txtFechaVencimiento.Text,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out fechaVencimiento))
                {
                    lblMensaje.Text =
                        "La fecha de vencimiento no es válida.";

                    return;
                }

                if (fechaVencimiento <= fechaInicio)
                {
                    lblMensaje.Text =
                        "La fecha de vencimiento debe ser posterior a la fecha de inicio.";

                    return;
                }

                int id =
                    Convert.ToInt32(
                        ViewState["InscripcionId"]);

                Inscripcion inscripcion =
                    await _inscripcionService
                        .ObtenerPorIdAsync(id);

                if (inscripcion == null)
                {
                    lblMensaje.Text =
                        "No se encontró la inscripción.";

                    return;
                }

                inscripcion.ClienteId =
                    Convert.ToInt32(
                        ddlCliente.SelectedValue);

                inscripcion.MembresiaId =
                    Convert.ToInt32(
                        ddlMembresia.SelectedValue);

                inscripcion.FechaInicio =
                    fechaInicio;

                inscripcion.FechaVencimiento =
                    fechaVencimiento;

                inscripcion.Modificado_Por =
                    UsuarioActual;

                bool resultado =
                    await _inscripcionService
                        .ActualizarAsync(inscripcion);

                if (resultado)
                {
                    lblMensaje.Text =
                        "Inscripción actualizada correctamente.";

                    LimpiarFormulario();

                    SalirModoEdicion();

                    await CargarInscripcionesAsync();
                }
                else
                {
                    lblMensaje.Text =
                        "No fue posible actualizar la inscripción.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "Error al actualizar la inscripción: "
                    + ex.Message;
            }
        }

        // =========================================================
        // ELIMINAR
        // =========================================================

        private async Task EliminarInscripcionAsync(int id)
        {
            try
            {
                bool resultado =
                    await _inscripcionService
                        .EliminarAsync(id);

                if (resultado)
                {
                    lblMensaje.Text =
                        "Inscripción eliminada correctamente.";

                    if (ViewState["InscripcionId"] != null &&
                        Convert.ToInt32(
                            ViewState["InscripcionId"]) == id)
                    {
                        LimpiarFormulario();

                        SalirModoEdicion();
                    }

                    await CargarInscripcionesAsync();
                }
                else
                {
                    lblMensaje.Text =
                        "No fue posible eliminar la inscripción.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "Error al eliminar la inscripción: "
                    + ex.Message;
            }
        }

        // =========================================================
        // VALIDACIONES
        // =========================================================

        private bool ValidarFormulario()
        {
            // =====================================================
            // CLIENTE
            // =====================================================

            if (string.IsNullOrWhiteSpace(
                ddlCliente.SelectedValue) ||
                ddlCliente.SelectedValue == "0")
            {
                lblMensaje.Text =
                    "Seleccione un cliente.";

                return false;
            }

            int clienteId;

            if (!int.TryParse(
                ddlCliente.SelectedValue,
                out clienteId) ||
                clienteId <= 0)
            {
                lblMensaje.Text =
                    "Seleccione un cliente válido.";

                return false;
            }

            // =====================================================
            // MEMBRESÍA
            // =====================================================

            if (string.IsNullOrWhiteSpace(
                ddlMembresia.SelectedValue) ||
                ddlMembresia.SelectedValue == "0")
            {
                lblMensaje.Text =
                    "Seleccione una membresía.";

                return false;
            }

            int membresiaId;

            if (!int.TryParse(
                ddlMembresia.SelectedValue,
                out membresiaId) ||
                membresiaId <= 0)
            {
                lblMensaje.Text =
                    "Seleccione una membresía válida.";

                return false;
            }

            // =====================================================
            // FECHA DE INICIO
            // =====================================================

            if (string.IsNullOrWhiteSpace(
                txtFechaInicio.Text))
            {
                lblMensaje.Text =
                    "Seleccione la fecha de inicio.";

                return false;
            }

            DateTime fechaInicio;

            if (!DateTime.TryParse(
                txtFechaInicio.Text,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fechaInicio))
            {
                lblMensaje.Text =
                    "Ingrese una fecha de inicio válida.";

                return false;
            }

            // =====================================================
            // FECHA DE VENCIMIENTO
            // =====================================================

            if (string.IsNullOrWhiteSpace(
                txtFechaVencimiento.Text))
            {
                lblMensaje.Text =
                    "Seleccione la fecha de vencimiento.";

                return false;
            }

            DateTime fechaVencimiento;

            if (!DateTime.TryParse(
                txtFechaVencimiento.Text,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fechaVencimiento))
            {
                lblMensaje.Text =
                    "Ingrese una fecha de vencimiento válida.";

                return false;
            }

            // =====================================================
            // COMPARACIÓN DE FECHAS
            // =====================================================

            if (fechaVencimiento <= fechaInicio)
            {
                lblMensaje.Text =
                    "La fecha de vencimiento debe ser posterior a la fecha de inicio.";

                return false;
            }

            // =====================================================
            // VALIDACIÓN DE FECHA DE INICIO
            // =====================================================

            DateTime fechaMaxima =
                DateTime.Today.AddYears(1);

            DateTime fechaMinima =
                DateTime.Today.AddYears(-1);

            if (fechaInicio > fechaMaxima)
            {
                lblMensaje.Text =
                    "La fecha de inicio no puede ser más de un año futura.";

                return false;
            }

            if (fechaInicio < fechaMinima)
            {
                lblMensaje.Text =
                    "La fecha de inicio no puede ser anterior a un año.";

                return false;
            }

            // =====================================================
            // VALIDACIÓN DE FECHA DE VENCIMIENTO
            // =====================================================

            if (fechaVencimiento > DateTime.Today.AddYears(10))
            {
                lblMensaje.Text =
                    "La fecha de vencimiento no puede superar los 10 años.";

                return false;
            }

            // =====================================================
            // TODO CORRECTO
            // =====================================================

            return true;
        }

        // =========================================================
        // LIMPIAR
        // =========================================================

        protected void btnLimpiar_Click(
            object sender,
            EventArgs e)
        {
            LimpiarFormulario();

            SalirModoEdicion();

            lblMensaje.Text = "";
        }

        private void LimpiarFormulario()
        {
            if (ddlCliente.Items.Count > 0)
            {
                ddlCliente.SelectedIndex = 0;
            }

            if (ddlMembresia.Items.Count > 0)
            {
                ddlMembresia.SelectedIndex = 0;
            }

            txtFechaInicio.Text = "";

            txtFechaVencimiento.Text = "";
        }

        // =========================================================
        // SALIR DEL MODO EDICIÓN
        // =========================================================

        private void SalirModoEdicion()
        {
            ViewState["InscripcionId"] = null;

            btnGuardar.Visible =
                true;

            btnActualizar.Visible =
                false;
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