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
    public partial class Clientes : BasePage
    {
        private readonly ClienteService _clienteService =
            new ClienteService();

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
                        CargarClientesAsync));
            }
        }

        // =========================================================
        // CARGAR CLIENTES
        // =========================================================

        private async Task CargarClientesAsync()
        {
            try
            {
                List<Cliente> clientes =
                    await _clienteService
                        .ObtenerTodosAsync();

                gvClientes.DataSource = clientes;
                gvClientes.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "No fue posible cargar los clientes: "
                    + ex.Message;
            }
        }

        // =========================================================
        // GUARDAR CLIENTE
        // =========================================================

        protected void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            Page.RegisterAsyncTask(
                new PageAsyncTask(
                    GuardarClienteAsync));
        }

        private async Task GuardarClienteAsync()
        {
            try
            {
                if (!ValidarFormulario())
                {
                    return;
                }

                DateTime fechaNacimiento;

                if (!DateTime.TryParse(
                    txtFechaNacimiento.Text,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out fechaNacimiento))
                {
                    lblMensaje.Text =
                        "Ingrese una fecha de nacimiento válida.";

                    return;
                }

                Cliente cliente =
                    new Cliente
                    {
                        Nombre =
                            txtNombre.Text.Trim(),

                        Cedula =
                            txtCedula.Text.Trim(),

                        Telefono =
                            txtTelefono.Text.Trim(),

                        Email =
                            txtEmail.Text.Trim(),

                        FechaNacimiento =
                            fechaNacimiento,

                        Sexo =
                            ddlSexo.SelectedValue,

                        Estado = true,

                        Creado_Por =
                            UsuarioActual
                    };

                bool resultado =
                    await _clienteService
                        .AgregarAsync(cliente);

                if (resultado)
                {
                    lblMensaje.Text =
                        "Cliente registrado correctamente.";

                    LimpiarFormulario();

                    await CargarClientesAsync();
                }
                else
                {
                    lblMensaje.Text =
                        "No fue posible registrar el cliente.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "Error al registrar el cliente: "
                    + ex.Message;
            }
        }

        // =========================================================
        // EDITAR / ELIMINAR
        // =========================================================

        protected void gvClientes_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            int indiceFila;

            if (!int.TryParse(
                e.CommandArgument?.ToString(),
                out indiceFila))
            {
                lblMensaje.Text =
                    "No fue posible identificar el cliente.";

                return;
            }

            if (indiceFila < 0 ||
                indiceFila >= gvClientes.Rows.Count)
            {
                lblMensaje.Text =
                    "La fila seleccionada no es válida.";

                return;
            }

            int id =
                Convert.ToInt32(
                    gvClientes
                        .DataKeys[indiceFila]
                        .Value);

            if (e.CommandName == "EditarCliente")
            {
                ViewState["ClienteId"] = id;

                Page.RegisterAsyncTask(
                    new PageAsyncTask(
                        () => CargarClienteParaEditarAsync(id)));
            }

            if (e.CommandName == "EliminarCliente")
            {
                Page.RegisterAsyncTask(
                    new PageAsyncTask(
                        () => EliminarClienteAsync(id)));
            }
        }

        // =========================================================
        // CARGAR CLIENTE PARA EDITAR
        // =========================================================

        private async Task CargarClienteParaEditarAsync(
            int id)
        {
            try
            {
                Cliente cliente =
                    await _clienteService
                        .ObtenerPorIdAsync(id);

                if (cliente == null)
                {
                    lblMensaje.Text =
                        "No se encontró el cliente.";

                    return;
                }

                txtNombre.Text =
                    cliente.Nombre;

                txtCedula.Text =
                    cliente.Cedula;

                txtTelefono.Text =
                    cliente.Telefono;

                txtEmail.Text =
                    cliente.Email;

                txtFechaNacimiento.Text =
                    cliente.FechaNacimiento
                        .ToString(
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture);

                ddlSexo.SelectedValue =
                    cliente.Sexo;

                ViewState["ClienteId"] =
                    cliente.Id;

                btnGuardar.Visible =
                    false;

                btnActualizar.Visible =
                    true;

                lblMensaje.Text =
                    "Cliente cargado para edición.";
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "No fue posible cargar el cliente: "
                    + ex.Message;
            }
        }

        // =========================================================
        // ELIMINAR CLIENTE
        // =========================================================

        private async Task EliminarClienteAsync(
            int id)
        {
            try
            {
                bool resultado =
                    await _clienteService
                        .EliminarAsync(id);

                if (resultado)
                {
                    lblMensaje.Text =
                        "Cliente eliminado correctamente.";

                    if (ViewState["ClienteId"] != null &&
                        Convert.ToInt32(
                            ViewState["ClienteId"]) == id)
                    {
                        LimpiarFormulario();

                        SalirModoEdicion();
                    }

                    await CargarClientesAsync();
                }
                else
                {
                    lblMensaje.Text =
                        "No fue posible eliminar el cliente.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "Error al eliminar el cliente: "
                    + ex.Message;
            }
        }

        // =========================================================
        // ACTUALIZAR CLIENTE
        // =========================================================

        protected void btnActualizar_Click(
            object sender,
            EventArgs e)
        {
            Page.RegisterAsyncTask(
                new PageAsyncTask(
                    ActualizarClienteAsync));
        }

        private async Task ActualizarClienteAsync()
        {
            try
            {
                if (!ValidarFormulario())
                {
                    return;
                }

                if (ViewState["ClienteId"] == null)
                {
                    lblMensaje.Text =
                        "No se seleccionó ningún cliente.";

                    return;
                }

                int id =
                    Convert.ToInt32(
                        ViewState["ClienteId"]);

                DateTime fechaNacimiento;

                if (!DateTime.TryParse(
                    txtFechaNacimiento.Text,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out fechaNacimiento))
                {
                    lblMensaje.Text =
                        "Ingrese una fecha de nacimiento válida.";

                    return;
                }

                Cliente cliente =
                    await _clienteService
                        .ObtenerPorIdAsync(id);

                if (cliente == null)
                {
                    lblMensaje.Text =
                        "No se encontró el cliente.";

                    return;
                }

                cliente.Nombre =
                    txtNombre.Text.Trim();

                cliente.Cedula =
                    txtCedula.Text.Trim();

                cliente.Telefono =
                    txtTelefono.Text.Trim();

                cliente.Email =
                    txtEmail.Text.Trim();

                cliente.FechaNacimiento =
                    fechaNacimiento;

                cliente.Sexo =
                    ddlSexo.SelectedValue;

                cliente.Modificado_Por =
                    UsuarioActual;

                bool resultado =
                    await _clienteService
                        .ActualizarAsync(cliente);

                if (resultado)
                {
                    lblMensaje.Text =
                        "Cliente actualizado correctamente.";

                    LimpiarFormulario();

                    SalirModoEdicion();

                    await CargarClientesAsync();
                }
                else
                {
                    lblMensaje.Text =
                        "No fue posible actualizar el cliente.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "Error al actualizar el cliente: "
                    + ex.Message;
            }
        }

        // =========================================================
        // LIMPIAR FORMULARIO
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
            txtCedula.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtFechaNacimiento.Text = "";

            if (ddlSexo.Items.Count > 0)
            {
                ddlSexo.SelectedIndex = 0;
            }
        }

        // =========================================================
        // SALIR DEL MODO EDICIÓN
        // =========================================================

        private void SalirModoEdicion()
        {
            ViewState["ClienteId"] = null;

            btnGuardar.Visible =
                true;

            btnActualizar.Visible =
                false;
        }

        // =========================================================
        // VALIDAR FORMULARIO
        // =========================================================

        private bool ValidarFormulario()
        {
            // =====================================================
            // NOMBRE
            // =====================================================

            string nombre =
                txtNombre.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                lblMensaje.Text =
                    "Ingrese el nombre del cliente.";

                return false;
            }

            if (nombre.Length < 3)
            {
                lblMensaje.Text =
                    "El nombre debe tener al menos 3 caracteres.";

                return false;
            }

            if (nombre.Length > 100)
            {
                lblMensaje.Text =
                    "El nombre no puede superar los 100 caracteres.";

                return false;
            }

            // =====================================================
            // CÉDULA
            // =====================================================

            string cedula =
                txtCedula.Text.Trim();

            if (string.IsNullOrWhiteSpace(cedula))
            {
                lblMensaje.Text =
                    "Ingrese la cédula del cliente.";

                return false;
            }

            if (cedula.Length < 9)
            {
                lblMensaje.Text =
                    "La cédula debe tener al menos 9 caracteres.";

                return false;
            }

            if (cedula.Length > 20)
            {
                lblMensaje.Text =
                    "La cédula no puede superar los 20 caracteres.";

                return false;
            }

            // =====================================================
            // TELÉFONO
            // =====================================================

            string telefono =
                txtTelefono.Text.Trim();

            if (string.IsNullOrWhiteSpace(telefono))
            {
                lblMensaje.Text =
                    "Ingrese el teléfono del cliente.";

                return false;
            }

            if (telefono.Length < 8)
            {
                lblMensaje.Text =
                    "El teléfono debe tener al menos 8 caracteres.";

                return false;
            }

            if (telefono.Length > 20)
            {
                lblMensaje.Text =
                    "El teléfono no puede superar los 20 caracteres.";

                return false;
            }

            // =====================================================
            // CORREO ELECTRÓNICO
            // =====================================================

            string email =
                txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                lblMensaje.Text =
                    "Ingrese el correo electrónico del cliente.";

                return false;
            }

            if (email.Length > 150)
            {
                lblMensaje.Text =
                    "El correo electrónico no puede superar los 150 caracteres.";

                return false;
            }

            try
            {
                var direccion =
                    new System.Net.Mail.MailAddress(email);

                if (direccion.Address != email)
                {
                    lblMensaje.Text =
                        "Ingrese un correo electrónico válido.";

                    return false;
                }
            }
            catch
            {
                lblMensaje.Text =
                    "Ingrese un correo electrónico válido.";

                return false;
            }

            // =====================================================
            // FECHA DE NACIMIENTO
            // =====================================================

            if (string.IsNullOrWhiteSpace(
                txtFechaNacimiento.Text))
            {
                lblMensaje.Text =
                    "Ingrese la fecha de nacimiento.";

                return false;
            }

            DateTime fechaNacimiento;

            if (!DateTime.TryParse(
                txtFechaNacimiento.Text,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fechaNacimiento))
            {
                lblMensaje.Text =
                    "Ingrese una fecha de nacimiento válida.";

                return false;
            }

            if (fechaNacimiento.Date > DateTime.Today)
            {
                lblMensaje.Text =
                    "La fecha de nacimiento no puede ser futura.";

                return false;
            }

            // =====================================================
            // SEXO
            // =====================================================

            if (string.IsNullOrWhiteSpace(
                ddlSexo.SelectedValue))
            {
                lblMensaje.Text =
                    "Seleccione el sexo del cliente.";

                return false;
            }

            // =====================================================
            // FORMULARIO CORRECTO
            // =====================================================

            return true;
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