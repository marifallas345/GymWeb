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
    public partial class Membresias : BasePage
    {
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
                        CargarMembresiasAsync));
            }
        }

        // =========================================================
        // CARGAR MEMBRESÍAS
        // =========================================================

        private async Task CargarMembresiasAsync()
        {
            try
            {
                List<Membresia> membresias =
                    await _membresiaService
                        .ObtenerTodosAsync();

                gvMembresias.DataSource =
                    membresias;

                gvMembresias.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "No fue posible cargar las membresías: "
                    + ex.Message;
            }
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
                    GuardarMembresiaAsync));
        }

        private async Task GuardarMembresiaAsync()
        {
            try
            {
                if (!ValidarFormulario())
                {
                    return;
                }

                decimal precio;

                if (!decimal.TryParse(
                    txtPrecio.Text,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out precio))
                {
                    lblMensaje.Text =
                        "Ingrese un precio válido.";

                    return;
                }

                int duracionMeses;

                if (!int.TryParse(
                    txtDuracionMeses.Text,
                    out duracionMeses))
                {
                    lblMensaje.Text =
                        "Ingrese una duración válida.";

                    return;
                }

                Membresia membresia =
                    new Membresia
                    {
                        Nombre =
                            txtNombre.Text.Trim(),

                        Precio =
                            precio,

                        DuracionMeses =
                            duracionMeses,

                        Estado =
                            true,

                        Creado_Por =
                            UsuarioActual
                    };

                bool resultado =
                    await _membresiaService
                        .AgregarAsync(membresia);

                if (resultado)
                {
                    lblMensaje.Text =
                        "Membresía registrada correctamente.";

                    LimpiarFormulario();

                    await CargarMembresiasAsync();
                }
                else
                {
                    lblMensaje.Text =
                        "No fue posible registrar la membresía.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "Error al registrar la membresía: "
                    + ex.Message;
            }
        }

        // =========================================================
        // EDITAR / ELIMINAR
        // =========================================================

        protected void gvMembresias_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            int indiceFila;

            if (!int.TryParse(
                e.CommandArgument?.ToString(),
                out indiceFila))
            {
                lblMensaje.Text =
                    "No fue posible identificar la membresía.";

                return;
            }

            if (indiceFila < 0 ||
                indiceFila >= gvMembresias.Rows.Count)
            {
                lblMensaje.Text =
                    "La fila seleccionada no es válida.";

                return;
            }

            int id =
                Convert.ToInt32(
                    gvMembresias
                        .DataKeys[indiceFila]
                        .Value);

            if (e.CommandName == "EditarMembresia")
            {
                ViewState["MembresiaId"] = id;

                Page.RegisterAsyncTask(
                    new PageAsyncTask(
                        () => CargarMembresiaParaEditarAsync(id)));
            }

            if (e.CommandName == "EliminarMembresia")
            {
                Page.RegisterAsyncTask(
                    new PageAsyncTask(
                        () => EliminarMembresiaAsync(id)));
            }
        }

        // =========================================================
        // CARGAR PARA EDITAR
        // =========================================================

        private async Task CargarMembresiaParaEditarAsync(
            int id)
        {
            try
            {
                Membresia membresia =
                    await _membresiaService
                        .ObtenerPorIdAsync(id);

                if (membresia == null)
                {
                    lblMensaje.Text =
                        "No se encontró la membresía.";

                    return;
                }

                txtNombre.Text =
                    membresia.Nombre;

                txtPrecio.Text =
                    membresia.Precio
                        .ToString(
                            "0.00",
                            CultureInfo.InvariantCulture);

                txtDuracionMeses.Text =
                    membresia.DuracionMeses
                        .ToString();

                ViewState["MembresiaId"] =
                    membresia.Id;

                btnGuardar.Visible = false;
                btnActualizar.Visible = true;

                lblMensaje.Text =
                    "Membresía cargada para edición.";
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "No fue posible cargar la membresía: "
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
                    ActualizarMembresiaAsync));
        }

        private async Task ActualizarMembresiaAsync()
        {
            try
            {
                if (!ValidarFormulario())
                {
                    return;
                }

                if (ViewState["MembresiaId"] == null)
                {
                    lblMensaje.Text =
                        "No se seleccionó ninguna membresía.";

                    return;
                }

                int id =
                    Convert.ToInt32(
                        ViewState["MembresiaId"]);

                decimal precio;

                if (!decimal.TryParse(
                    txtPrecio.Text,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out precio))
                {
                    lblMensaje.Text =
                        "Ingrese un precio válido.";

                    return;
                }

                int duracionMeses;

                if (!int.TryParse(
                    txtDuracionMeses.Text,
                    out duracionMeses))
                {
                    lblMensaje.Text =
                        "Ingrese una duración válida.";

                    return;
                }

                Membresia membresia =
                    await _membresiaService
                        .ObtenerPorIdAsync(id);

                if (membresia == null)
                {
                    lblMensaje.Text =
                        "No se encontró la membresía.";

                    return;
                }

                membresia.Nombre =
                    txtNombre.Text.Trim();

                membresia.Precio =
                    precio;

                membresia.DuracionMeses =
                    duracionMeses;

                membresia.Modificado_Por =
                    UsuarioActual;

                bool resultado =
                    await _membresiaService
                        .ActualizarAsync(membresia);

                if (resultado)
                {
                    lblMensaje.Text =
                        "Membresía actualizada correctamente.";

                    LimpiarFormulario();

                    SalirModoEdicion();

                    await CargarMembresiasAsync();
                }
                else
                {
                    lblMensaje.Text =
                        "No fue posible actualizar la membresía.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "Error al actualizar la membresía: "
                    + ex.Message;
            }
        }

        // =========================================================
        // ELIMINAR
        // =========================================================

        private async Task EliminarMembresiaAsync(int id)
        {
            try
            {
                bool resultado =
                    await _membresiaService
                        .EliminarAsync(id);

                if (resultado)
                {
                    lblMensaje.Text =
                        "Membresía eliminada correctamente.";

                    if (ViewState["MembresiaId"] != null &&
                        Convert.ToInt32(
                            ViewState["MembresiaId"]) == id)
                    {
                        LimpiarFormulario();

                        SalirModoEdicion();
                    }

                    await CargarMembresiasAsync();
                }
                else
                {
                    lblMensaje.Text =
                        "No fue posible eliminar la membresía.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "Error al eliminar la membresía: "
                    + ex.Message;
            }
        }

        // =========================================================
        // VALIDACIONES
        // =========================================================

        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(
                txtNombre.Text))
            {
                lblMensaje.Text =
                    "Ingrese el nombre de la membresía.";

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                txtPrecio.Text))
            {
                lblMensaje.Text =
                    "Ingrese el precio.";

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                txtDuracionMeses.Text))
            {
                lblMensaje.Text =
                    "Ingrese la duración en meses.";

                return false;
            }

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
            txtNombre.Text = "";
            txtPrecio.Text = "";
            txtDuracionMeses.Text = "";
        }

        // =========================================================
        // SALIR DEL MODO EDICIÓN
        // =========================================================

        private void SalirModoEdicion()
        {
            ViewState["MembresiaId"] = null;

            btnGuardar.Visible = true;
            btnActualizar.Visible = false;
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